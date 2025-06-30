import { Component } from '@angular/core';
import { ErrorService } from './error-service';
import { environment } from 'src/environments/environment';
import { MDBModalService } from 'ng-uikit-pro-standard';
import { BaseComponent } from '../base/base.component';

@Component({
  selector: 'studio-portal-errors',
  templateUrl: './error.component.html',
  providers: [MDBModalService]
})
export class ErrorComponent extends BaseComponent {
  userFriendlyHeader: string;
  userFriendlyDetails: string;
  httpStatusCode: number;
  production: boolean;

  constructor(public errorService: ErrorService, public modalService: MDBModalService) {
    super(modalService);
    this.production = environment.production;
    if (this.errorService.error) {
      this.httpStatusCode = (this.errorService.error.status === 0) ? 503 : this.errorService.error.status;
      this.userFriendlyHeader = this.getFriendlyHeader(this.httpStatusCode);
      this.userFriendlyDetails = this.getFriendlyDetails(this.httpStatusCode);
    }
  }


  getFriendlyHeader(statusCode: number): string {
    let result = '';
    switch (statusCode) {
      case 503:
        result = 'Webservice currently unavailable';
        break;
      case 404:
        result = 'Webpage not found';
        break;
      default:
        result = 'An unexpected error occured';
    }
    return result;
  }

  getFriendlyDetails(statusCode: number): string {
    let result = '';
    switch (statusCode) {
      case 503:
        result = 'A team of highly trained monkeys has been dispatched to bring it back online.';
        break;
      case 404:
        result = 'Where IS Waldo? We cannot find him or this page';
        break;
      default:
        result = 'You see... what had happened was...';
    }
    return result;
  }

  isObject(val) { return typeof val === 'object'; }
}

