import { BusinessHour } from '../../models/business-hour';
import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HoursOfOperationsService {
  errors: string[] = [];
  private message = new Subject<BusinessHour>();
  messageChanged$ = this.message.asObservable();
  hoos: BusinessHour[];

  setHoo(hoo: BusinessHour) {
    this.getErrors(hoo);
    if (this.errors.length === 0) {
        this.message.next(hoo);
    }
  }

  hasErrors(day: string) {
    return this.errors.find(e => e.indexOf(day) !== -1);
  }

  getErrors(hoo: BusinessHour) {
    if (hoo.hasOverlap()) {
      this.errors.push('Closing time must be after opening time');
    }
  }

  clearErrors() {
    this.errors = [];
  }
}
