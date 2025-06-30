import { AbstractControl, Validator, NG_VALIDATORS, FormControl } from '@angular/forms';
import { minValueValidator, maxValueValidator, matchValidator, strongPasswordValidator, loginUserNameValidator } from '../validators';
import { Directive, Input } from '@angular/core';

@Directive({
    selector: '[minValue]',
    providers: [{ provide: NG_VALIDATORS, useExisting: MinValueValidatorDirective, multi: true }]
})
export class MinValueValidatorDirective implements Validator {
    @Input('minValue') model: number;
    validate(control: AbstractControl): { [key: string]: any } | null {
        return this.model ? minValueValidator(this.model)(control)
            : null;
    }
}

@Directive({
    selector: '[maxValue]',
    providers: [{ provide: NG_VALIDATORS, useExisting: MaxValueValidatorDirective, multi: true }]
})
export class MaxValueValidatorDirective implements Validator {
    @Input('maxValue') model: number;
    validate(control: AbstractControl): { [key: string]: any } | null {
        return this.model ? maxValueValidator(this.model)(control)
            : null;
    }
}

@Directive({
    selector: '[match]',
    providers: [{ provide: NG_VALIDATORS, useExisting: MatchValidatorDirective, multi: true }]
})
export class MatchValidatorDirective implements Validator {
    @Input('match') model: string;
    validate(control: AbstractControl): { [key: string]: any } | null {
        const controlToMatch = control.parent.get(this.model) as FormControl;
        return this.model ? matchValidator(controlToMatch)(control)
            : null;
    }
}

@Directive({
    selector: '[strongPassword]',
    providers: [{ provide: NG_VALIDATORS, useExisting: StrongPasswordDirective, multi: true }]
})
export class StrongPasswordDirective implements Validator {
    validate(control: AbstractControl): { [key: string]: any } | null {
        return strongPasswordValidator()(control);
    }
}


@Directive({
    selector: '[loginUserName]',
    providers: [{ provide: NG_VALIDATORS, useExisting: LoginUserNameDirective, multi: true }]
})
export class LoginUserNameDirective implements Validator {
    validate(control: AbstractControl): { [key: string]: any } | null {
        return loginUserNameValidator()(control);
    }
}
