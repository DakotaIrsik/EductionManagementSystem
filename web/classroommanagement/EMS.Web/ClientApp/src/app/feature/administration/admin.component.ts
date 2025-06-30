import { Component, OnInit } from '@angular/core';
import { TabsetConfig } from 'ng-uikit-pro-standard';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  providers: [TabsetConfig]
})
export class AdminComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
