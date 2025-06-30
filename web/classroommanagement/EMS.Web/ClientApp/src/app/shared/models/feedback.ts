import { DateTime } from 'luxon';

export class Feedback {
    public workitem: string;
    public importance: string;
    public willingtopayforfeature: string;
    public willingtopayforfeatureamount: string;
    public willingtopayforsubscription: string;
    public willingtopayforsubscriptionamount: string;
    public comments: string;
    public createdate: DateTime;
    public userid: string;
}