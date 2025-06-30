import { Component, Input } from '@angular/core';
import { HoursOfOperationsService } from './hours-of-operation-service';
import { BusinessHour } from '../../models/business-hour';

@Component({
  selector: 'studio-portal-hours-of-operation-selection',
  templateUrl: './hours-of-operation-selection.component.html',
  styleUrls: ['hours-of-operation-selection.component.css'],
})
export class HoursOfOperationSelectionComponent {
  @Input() day: string;
  @Input() start: string;
  @Input() end: string;
  @Input() open: boolean;
  errors: string[];

  constructor(private hoursOfOperationService: HoursOfOperationsService) {
  }

  openTodayChanged(event: any) {
    this.open = event.checked;
    if (!this.open) {
      this.start = null;
      this.end = null;
    }
    this.setHoo();
  }

  startTimeChanged(time: any) {
    this.start = time;
    this.setHoo();
  }

  endTimeChanged(time: any) {
    this.end = time;
    this.setHoo();
  }

  setHoo() {
    const hoo = new BusinessHour(this.day, this.start, this.end, this.open);
    this.hoursOfOperationService.clearErrors();
    this.hoursOfOperationService.setHoo(hoo);
    this.errors = this.hoursOfOperationService.errors;
  }
}
