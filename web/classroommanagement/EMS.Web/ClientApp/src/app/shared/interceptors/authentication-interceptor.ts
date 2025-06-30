import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
  private AUTH_HEADER = 'Authorization';
  private token: string;

  constructor(private authService: AuthService, public toasterService: ToastrService) {
    this.token = this.authService.authorizationHeaderValue;
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if (!req.headers.has('Content-Type') && !req.url.includes('Image')) {
      req = req.clone({
        headers: req.headers.set('Content-Type', 'application/json')
      });
    }

    if (this.authService.isAuthenticated()) {
      // TODO Fix this when IS is up.
      //req = this.addAuthenticationToken(req);
    }


    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          const messageDelay = 5000;
          const loginExpirationMessage = 'Your login session has expired redirecting you to login momentarily.';
          const isToastShowing = this.toasterService.findDuplicate(loginExpirationMessage, false, false);
          if (!isToastShowing) {
            this.toasterService.error(loginExpirationMessage, 'Login Expired',
              {
                timeOut: messageDelay,
                progressBar: true,
                progressAnimation: 'decreasing'
              });

            setTimeout(() => this.authService.login(), messageDelay);
          }
          // TODO implement refresh token if that's even a thing with JWT.
          // this.refreshToken(req, next);
        } else {
          return throwError(error);
        }
      })
    );
  }

  private addAuthenticationToken(request: HttpRequest<any>): HttpRequest<any> {
    if (!this.token) {
      return request;
    }
    return request.clone({
      headers: request.headers.set(this.AUTH_HEADER, this.token)
    });
  }

  //#region Non-functional Refresh Token
  /* private refreshTokenInProgress = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  private refreshToken(req: HttpRequest<any>, next: HttpHandler) {
  if (this.refreshTokenInProgress == true) {
  // If refreshTokenInProgress is true, we will wait until refreshTokenSubject has a non-null value
  // which means the new token is ready and we can retry the request again
  return this.refreshTokenSubject.pipe(
  filter(result => result !== null),
  take(1),
  switchMap(() => next.handle(this.addAuthenticationToken(req)))
  );
  } else {
  this.refreshTokenInProgress = true;

  // Set the refreshTokenSubject to null so that subsequent API calls will wait until the new token has been retrieved
  this.refreshTokenSubject.next(null);

  return this.refreshAccessToken().pipe(
  switchMap((success: boolean) => {
  this.refreshTokenSubject.next(success);
  return next.handle(this.addAuthenticationToken(req));
  }),
  // When the call to refreshToken completes we reset the refreshTokenInProgress to false
  // for the next time the token needs to be refreshed
  finalize(() => this.refreshTokenInProgress = false)
  );
  }
  }

  private refreshAccessToken(): Observable<any> {
  //return of("secret token");
  return of(this.token);
  }
  */
  //#endregion
}
