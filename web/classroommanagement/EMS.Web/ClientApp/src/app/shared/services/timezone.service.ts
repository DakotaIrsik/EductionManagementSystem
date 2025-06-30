import { Injectable } from '@angular/core';
import * as moment from 'moment-timezone';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimezoneService {

  constructor() {
    const abbrs = {
      EST: 'Eastern Time',
      EDT: 'Eastern Time',
      CST: 'Central Time',
      CDT: 'Central Time',
      MST: 'Mountain Time',
      MDT: 'Mountain Time',
      PST: 'Pacific Time',
      PDT: 'Pacific Time',
      AKST: 'Alaska Time',
      AKDT: 'Alaska Time',
      HST: 'Hawaii Time',
      HDT: 'Hawaii Time'
    };

    moment.fn.zoneName = function () {
      const abbr = this.zoneAbbr();
      return abbrs[abbr] || abbr;
    };
  }

  getAllZones(): Observable<string[]> {
    return of([
      'Hawaii Time',
      'Alaska Time',
      'Pacific Time',
      'Mountain Time',
      'Central Time',
      'Eastern Time'
    ]);
  }

  guessUserTimezone(): Observable<string> {
    return of(moment.tz(moment.tz.guess()).format('zz'));
  }
}
