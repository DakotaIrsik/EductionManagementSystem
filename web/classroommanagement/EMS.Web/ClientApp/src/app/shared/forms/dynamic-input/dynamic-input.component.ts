import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { SocialMedia } from '../../models/social-media';

@Component({
    selector: 'portal-dynamic-input',
    templateUrl: './dynamic-input.component.html',
    styleUrls: ['./dynamic-input.component.scss']
})
export class DynamicInputComponent implements OnInit {

    public myForm: FormGroup;

    @Output() customUrlAdded = new EventEmitter<SocialMedia[]>();

    constructor(private _fb: FormBuilder) { }

    ngOnInit() {
        this.myForm = this._fb.group({
            links: this._fb.array([
                this.initlanguage(),
            ])
        });

        this.myForm.valueChanges.subscribe(data => this.someChanges(data));
    }

    someChanges(event: any) {
        const x = event.links as SocialMedia[];
        this.customUrlAdded.emit(x);
    }

    initlanguage() {
        return this._fb.group({
            name: ['Other'],
            url: ['']
        });
    }

    addLanguage() {
        const control = this.myForm.get('links') as FormArray;
        control.push(this.initlanguage());
    }

    removeLanguage(i: number) {
        const control = this.myForm.get('links') as FormArray;
        control.removeAt(i);
    }
}
