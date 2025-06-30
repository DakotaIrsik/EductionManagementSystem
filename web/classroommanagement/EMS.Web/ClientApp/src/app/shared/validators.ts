import { ValidatorFn, AbstractControl, FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { RegularExpressions } from './regex';
import { ValidationError } from './models/validation-error';

export function zipCodeValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const zip = !RegularExpressions.zipCode.test(control.value);
        return zip ? { invalidZip: { value: control.value } } : null;
    };
}

export function passwordStrengthValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const password = !RegularExpressions.password.test(control.value);
        return password ? { invalidPassword: { value: control.value } } : null;
    };
}

export function strongPasswordValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const isStrong = !RegularExpressions.password.test(control.value);
        return isStrong ? {
            validationError: new ValidationError(null,
                'Min. 6 characters with at least one non alphanumeric character')
        } : null;
    };
}

export function phoneNumberValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const phone = !RegularExpressions.phoneNumber.test(control.value);
        return phone ? { invalidPhone: { value: control.value } } : null;
    };
}

export class CustomErrorStateMatcher implements ErrorStateMatcher {
    isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
        const invalidCtrl = !!(control && control.invalid && control.parent.dirty);
        const invalidParent = !!(control && control.parent && control.parent.invalid && control.parent.dirty);
        return (invalidCtrl || invalidParent);
    }
}

export function emailValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const email = !RegularExpressions.email.test(control.value);
        return email ? { invalidEmail: { value: control.value } } : null;
    };
}

export function matchValidator(controlToMatch: FormControl): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const misMatch = control.value !== controlToMatch.value;
        const thisControlName = getControlName(control);
        const matchControlName = getControlName(controlToMatch);
        return misMatch ? { validationError: new ValidationError(null,
            toSentenceCase(thisControlName) +
            ' must match ' +
            toSentenceCase(matchControlName)) } : null;
    };
}

export function minValueValidator(minValue: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const tooLow = (control.value) ? +control.value.replace(',', '') < +minValue : false;
        return tooLow ? { validationError: new ValidationError(control.value, 'Must be ' + minValue + ' or greater') } : null;
    };
}

export function maxValueValidator(maxValue: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const tooHigh = (control.value) ? +control.value.replace(',', '') > +maxValue : false;
        return tooHigh ? { validationError: new ValidationError(control.value, 'Must be ' + maxValue + ' or less') } : null;
    };
}

export function loginUserNameValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        const containsSpace = (control.value.indexOf(' ') !== -1);
        return containsSpace ? { validationError: new ValidationError(null, 'No spaces allowed') } : null;
    };
}

export function getControlName(c: AbstractControl): string | null {
    const formGroup = c.parent.controls;
    return Object.keys(formGroup).find(name => c === formGroup[name]) || null;
}
export function toSentenceCase(camelCaseText: string) {
    const result = camelCaseText.replace(/([A-Z])/g, ' $1');
    return result.charAt(0).toUpperCase() + result.slice(1); // capitalize the first letter - as an example.
}
