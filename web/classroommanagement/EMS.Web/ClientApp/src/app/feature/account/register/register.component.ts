import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators';
import { UserRegistration } from 'src/app/shared/models/user-registration';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { ApplicationResolver } from 'src/app/shared/base/application.resolver';


@Component({
  selector: 'studio-portal-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  success: boolean;
  errors: string[] = [];
  userRegistration: UserRegistration = new UserRegistration();
  submitted = false;
  application: string;

  constructor(private authService: AuthService,
              private spinner: NgxSpinnerService) {
      this.application = ApplicationResolver.getConfiguration();
  }
errorMessage: string;
  ngOnInit() {
  }

  onSubmit() {
    this.spinner.show();
    this.authService.register(this.userRegistration)
      .pipe(finalize(() => {
        this.spinner.hide();
      }))
      .subscribe(
      result => {
         if (result.succeeded) {
           this.authService.login();
         } else {
          this.errors = result.errors.map(error => error.description);
         }
      });
  }
}
