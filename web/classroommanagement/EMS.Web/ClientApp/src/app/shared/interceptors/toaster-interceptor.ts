import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ToasterInterceptor implements HttpInterceptor {
    constructor(public toasterService: ToastrService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            tap(evt => {
                if (evt instanceof HttpResponse) {
                    if ((req.url.toLowerCase().indexOf('my') === -1) && // TODO Make Studios/MyStudios a GET method in API somehow...
                        (req.url.toLowerCase().indexOf('search') === -1) && // TODO Make Studios/Search a GET method in API somehow...
                        (req.url.toLowerCase().indexOf('city') === -1) &&
                        (req.url.toLowerCase().indexOf('class') === -1) &&
                        (req.url.toLowerCase().indexOf('user') === -1)) { // TODO Make GetCities a GET method in API
                        switch (req.method) {
                            case 'PUT':
                                this.toasterService.success('Successfully updated', this.parseApiFromUrl(req.url),
                                    { positionClass: 'toast-top-right' });
                                break;
                            case 'DELETE':
                                this.toasterService.success('Successfully deleted', this.parseApiFromUrl(req.url),
                                    { positionClass: 'toast-top-right' });
                                break;
                            case 'POST':
                                this.toasterService.success('Successfully created', this.parseApiFromUrl(req.url),
                                    { positionClass: 'toast-top-right' });
                                break;
                            default:
                                break;
                        }
                    }
                }
            }),
            catchError((err: any) => {
                // if we're here, then we're probably getting 500's from the api, it probably isn't the
                // toaster-interceptor's job to foward us to an error page, so for now commenting out this error catching logic.
                if (err instanceof HttpErrorResponse) {
                    /*     try {
                            this.toasterService.error(err.error.message, err.error.title, { positionClass: 'toast-bottom-center' });
                        } catch (e) {
                            this.toasterService.error('An error occurred', '', { positionClass: 'toast-bottom-center' });
                        }
                        // TODO add Logging */
                }
                return of(err);
            }));

    }

    parseApiFromUrl(url: string): string {
        const apiAndMethod = url.substring(url.indexOf('v1/') + 3);
        let endOfApiName = apiAndMethod.indexOf('?');

        if (endOfApiName === -1) {
            endOfApiName = apiAndMethod.indexOf('/');
        }
        if (endOfApiName === -1) {
            endOfApiName = null;
        }
        if (endOfApiName > 0) {
            return apiAndMethod.substring(0, endOfApiName);
        } else {
            return apiAndMethod;
        }
    }
}
