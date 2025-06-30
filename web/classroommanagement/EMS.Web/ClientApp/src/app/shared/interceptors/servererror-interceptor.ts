import { Injectable } from '@angular/core';
import { HttpEvent, HttpRequest, HttpHandler, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { ErrorService } from '../errors/error-service';

@Injectable()
export class ServerErrorInterceptor implements HttpInterceptor {

  constructor(private errorService: ErrorService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      retry(1),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0 ||
          // error.status === 404 || don't filter 404's as errors, good
          // restful api's can return a 404 simply meaning the resource wasn't found.
          error.status === 500 || error.status === 503) {
          this.errorService.handleError(error);
          return;
        } else if (error.status === 404) {
          // TODO what should this return?
          return new Observable<HttpEvent<{}>>();
        } else {
        return throwError(error);
        }
      })
    );
  }
}
