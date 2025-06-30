import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { UserService } from 'src/app/core/user/user.service';
import { ApplicationResolver } from 'src/app/shared/base/application.resolver';

@Component({
  selector: 'studio-portal-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent implements OnInit {
  errorMessage: string;
  error: boolean;
  application: string;

  constructor(private authService: AuthService,
              private userService: UserService,
              private router: Router,
              private route: ActivatedRoute) {
    this.application = ApplicationResolver.getConfiguration();
  }

  async ngOnInit() {
    if (this.route.snapshot.fragment.indexOf('error') >= 0) {
      this.error = true;
      return;
    }

    await this.authService.completeAuthentication();

    this.userService.associateIdentityUserToApplication().subscribe(
      response => {
        this.router.navigate(['/' + this.application + '/home']);
      },
      error => this.errorMessage = error as any
    );
  }
}
