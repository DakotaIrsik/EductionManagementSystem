import { Component, OnInit, Input } from '@angular/core';
import { MDBModalService, MDBModalRef } from 'ng-uikit-pro-standard';
import { ActionableMetricModalComponent } from '../modals/actionable-metric.modal.component';


@Component({
  selector: 'studio-portal-base',
  templateUrl: './base.component.html',
})
export class BaseComponent implements OnInit {
  @Input() workItem: number;
  public environment: any;

  constructor(public modalService: MDBModalService) {
  }

  modalRef: MDBModalRef;
  modalOptions = {
    backdrop: true,
    keyboard: true,
    focus: true,
    show: false,
    ignoreBackdropClick: true,
    class: '',
    containerClass: '',
    animated: true,
    data: {
      heading: '',
      content: { heading: '', description: '' },
      workItem: 0
    }
  };

  ngOnInit(): void {
    if (this.workItem && this.workItem > 0) {
      this.openModal(this.workItem);
    }
  }

  openModal(workItem: number) {
    this.modalOptions.data.workItem = workItem;
    this.modalRef = this.modalService.show(ActionableMetricModalComponent, this.modalOptions);
  }
}
