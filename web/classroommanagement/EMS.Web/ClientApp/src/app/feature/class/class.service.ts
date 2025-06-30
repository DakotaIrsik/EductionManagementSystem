import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Class } from '../../shared/models/class';
import { StandardResponse } from 'src/app/shared/models/api/responses/standard-response';
import { ClassSearchRequest } from './class-search/class-search-request';
import { BaseService } from 'src/app/shared/services/base-service';

@Injectable({
  providedIn: 'root'
})
export class ClassService extends BaseService {

  private classApi = 'Class';

  constructor(private http: HttpClient) {
    super();
   }

  search(request: ClassSearchRequest) {
    return this.http.post<StandardResponse<Class[]>>(this.fullApiUrl + this.classApi, JSON.stringify(request)).pipe(
      catchError(this.handleError));
  }

  get(classId: number) {
    return this.http.get<Class>(this.fullApiUrl + this.classApi + '/' + classId).pipe(
      catchError(this.handleError));
  }

  delete(classId: number) {
    return this.http.delete(this.fullApiUrl + this.classApi + '/' + classId).pipe(
      catchError(this.handleError));
  }
}
