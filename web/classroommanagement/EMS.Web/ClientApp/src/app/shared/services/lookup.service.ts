import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { BaseService } from './base-service';
import { CitySearchRequest } from '../models/api/requests/city-search-request';
import { LookUp } from '../models/api/responses/lookup';

@Injectable({
  providedIn: 'root'
})
export class LookUpService extends BaseService {
  private cityApi = 'City';
  private countryApi = 'Country';
  private socialMediaApi = 'SocialMedia';

  constructor(private http: HttpClient) {
    super();
  }

  get(api: string) {
    return this.http.get<LookUp[]>(this.fullApiUrl + api)
      .pipe(catchError(this.handleError));
  }

  getCities(state: string) {
    return this.http.post<Array<LookUp>>(this.fullApiUrl + this.cityApi, JSON.stringify(new CitySearchRequest(state)))
      .pipe(catchError(this.handleError));
  }

  getCountries() {
    return this.http.get<Array<LookUp>>(this.fullApiUrl + this.countryApi)
      .pipe(catchError(this.handleError));
  }


  getSocialMedia() {
    return this.http.get<Array<LookUp>>(this.fullApiUrl + this.socialMediaApi)
      .pipe(catchError(this.handleError));
  }
}

