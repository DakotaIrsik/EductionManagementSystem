import { Component, OnInit, Input } from '@angular/core';
import { BusinessHour } from '../../models/business-hour';

@Component({
  selector: 'studio-portal-hours-of-operation-view',
  templateUrl: './hours-of-operation-view.component.html',
  styleUrls: ['./hours-of-operation-view.component.css']
})
export class HoursOfOperationViewComponent implements OnInit {
  @Input()
  hoursOfOperation: BusinessHour[];
  constructor() { }

  ngOnInit() {
  }

}
