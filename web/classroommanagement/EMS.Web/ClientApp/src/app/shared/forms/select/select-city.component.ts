import { Component, Input, OnInit, Injectable } from '@angular/core';
import { LookUpService } from '../../services/lookup.service';
import { MdbSelect } from '../../models/mdb-select';
import { SelectFromApiComponent } from './select.component';
import { LookUpSelectionService } from '../../services/lookup-selection.service';
import { MDBModalService } from 'ng-uikit-pro-standard';

@Component({
  selector: 'studio-portal-city-select',
  templateUrl: './select.component.html'
})
export class SelectCityComponent extends SelectFromApiComponent implements OnInit {
  constructor(lookUpSelectionService: LookUpSelectionService,
              lookUpService: LookUpService,
              modalService: MDBModalService) {
    super(lookUpService, lookUpSelectionService, modalService);
    this.lookUpSelectionService.lookUpSelected$.subscribe(lookUpRelation => {
      if (lookUpRelation.key === 'state') {
        this.getCities(lookUpRelation.lookUp[0].value);
      }
    });
  }

  ngOnInit() {}

  getCities(state: string) {
    this.lookUpService.getCities(state).subscribe(response => {
      this.selects = response.map(data => new MdbSelect(data.name, data.name));
    });
  }
}
