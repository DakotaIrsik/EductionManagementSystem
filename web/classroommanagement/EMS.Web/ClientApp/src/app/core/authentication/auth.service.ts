import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { BehaviorSubject } from 'rxjs';
import { BaseService } from 'src/app/shared/services/base-service';
import { ApplicationResolver } from 'src/app/shared/base/application.resolver';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {
  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this.authNavStatusSource.asObservable();
  private manager: UserManager;
  private user: User | null;

  constructor(private http: HttpClient) {
    super();
    this.manager = new UserManager(getClientSettings(this.environment, this.application));
    this.manager.getUser().then(user => {
      this.user = user;
      this.authNavStatusSource.next(this.isAuthenticated());
    });
  }

  login() {
    return this.manager.signinRedirect();
  }

  async completeAuthentication() {
    this.user = await this.manager.signinRedirectCallback();
    this.authNavStatusSource.next(this.isAuthenticated());
  }

  register(userRegistration: any): any {
    return this.http.post(this.environment.identityServer + '/account/register', userRegistration).pipe(catchError(this.handleError));
  }

  isAuthenticated(): boolean {
    // TODO Implement IS
    return true;
    return this.user != null && !this.user.expired;
  }

  get authorizationHeaderValue(): string {
    if (this.user && this.user.token_type && this.user.access_token) {
      return `${this.user.token_type} ${this.user.access_token}`;
    }
  }

  get sub(): string {
    if (this.user) {
      return `${this.user.profile.sub}`;
    }
  }

  get name(): string {
    // TODO Implement IS
    return 'Test';
    return this.user != null ? this.user.profile.name : '';
  }

  signout() {
    this.manager.signoutRedirect();
  }
}

export function getClientSettings(environment: any, application: string): UserManagerSettings {
  return {
    authority: environment.identityServer,
    client_id: environment.clientId,
    redirect_uri: environment.websiteUrl + application + '/auth-callback',
    post_logout_redirect_uri: environment.websiteUrl,
    response_type: environment.responseType,
    scope: environment.scopes + environment.suite + '.' + application + '.API',
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    silent_redirect_uri: environment.websiteUrl + environment.tokenRefreshUri,
    prompt: 'login'
  };

}
