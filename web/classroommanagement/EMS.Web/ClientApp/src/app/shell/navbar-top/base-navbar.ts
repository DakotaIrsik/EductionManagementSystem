import { OnInit, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApplicationResolver } from 'src/app/shared/base/application.resolver';


export class BaseNavbar implements OnInit, OnDestroy {
  environment: any;
  application: string;
  constructor(private authService: AuthService, private spinner: NgxSpinnerService) {
    this.environment = environment;
    this.application = ApplicationResolver.getConfiguration();
  }

  name: string;
  isAuthenticated: boolean;
  subscription: Subscription;
  environmentName: string;
  production: boolean;

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => this.isAuthenticated = status);
    this.name = this.authService.name;
  }

  login() {
    this.spinner.show();
    this.authService.login();
  }

  signout() {
    this.authService.signout();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
