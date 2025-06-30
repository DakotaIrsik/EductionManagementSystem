import { throwError } from 'rxjs';
import { HttpErrorResponse, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApplicationResolver } from '../base/application.resolver';

export class BaseService {
  public application: string;
  public environment: any;
  public fullApiUrl: string;
  protected apiUrl: string;
  protected apiVersion: string;
  protected apiExtension: string;

  constructor() {
    this.environment = environment;
    this.application = ApplicationResolver.getConfiguration();
    this.apiUrl = environment.emsApi.url;
    this.apiVersion = environment.emsApi.version;
    this.apiExtension = environment.emsApi.apiExtension;
    this.fullApiUrl = this.apiUrl + this.apiExtension + this.apiVersion;
  }

  protected handleError(err: HttpErrorResponse) {
    const errorMessage: string[] = [];
    if (err.error instanceof ErrorEvent) {
      errorMessage.push(`An error occured: ${err.error.message}`);
    } else {
      // tslint:disable-next-line: forin
      for (const er in err.error) {
        errorMessage.push(err.error[er]);
      }
    }

    console.error(errorMessage);
    return throwError(errorMessage);
  }

  toQueryString(options: any) {
    const params = new HttpParams();
    // tslint:disable-next-line: forin
    for (const key in options) {
      params.set(key, options[key]);
    }
  }
}
