import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BaseService } from 'src/app/shared/services/base-service';
@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseService {

  private userApi = 'User';

  constructor(private http: HttpClient) {
    super();
   }

  associateIdentityUserToApplication() {
    return this.http.post(this.fullApiUrl + this.userApi, null)
        .pipe(catchError(this.handleError));
  }

  get() {
    return this.http.get(this.fullApiUrl + this.userApi)
        .pipe(catchError(this.handleError));
  }
}
