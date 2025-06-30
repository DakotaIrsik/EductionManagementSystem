import { Component } from '@angular/core';
import { MDBModalRef } from 'ng-uikit-pro-standard';
import { FeedbackService } from '../services/feedback.service';
import { Feedback } from '../models/feedback';

@Component({
  selector: 'studio-portal-actionable-metric-modal',
  templateUrl: './actionable-metric.modal.component.html',
  styles: [ `.btn-group { padding-bottom: 20px; }`]
})
export class ActionableMetricModalComponent {

  public workItem: string;
  public importance = 'Not Very';
  public willingToPayForFeature = 'No';
  public willingToPayForFeatureAmount = '$1';
  public willingToPayForSubscription = 'No';
  public willingToPayForSubscriptionAmount = '$1';
  public comments: string;
  constructor(public modalRef: MDBModalRef, private feedbackService: FeedbackService) { }

  public submitFeedback() {
    const feedback = new Feedback();
    feedback.comments = this.comments;
    feedback.importance = this.importance;
    feedback.willingtopayforfeature = this.willingToPayForFeature;
    feedback.willingtopayforfeatureamount = this.willingToPayForFeatureAmount;
    feedback.willingtopayforsubscription = this.willingToPayForSubscription;
    feedback.willingtopayforsubscriptionamount = this.willingToPayForSubscriptionAmount;
    feedback.workitem = this.workItem;
    this.feedbackService.create(feedback).subscribe(result => this.modalRef.hide());
  }
}
