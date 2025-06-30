import { Component, Input } from '@angular/core';

@Component({
  selector: 'studio-portal-validation-error',
  templateUrl: './validation-error.component.html'
})
export class ValidationErrorComponent {
@Input() validatedControl: any;
}
