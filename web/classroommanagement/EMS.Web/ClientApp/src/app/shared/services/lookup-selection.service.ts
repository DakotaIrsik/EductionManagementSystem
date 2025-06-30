import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { LookUpRelation } from '../models/lookup-relation';

@Injectable({
  providedIn: 'root'
})
export class LookUpSelectionService {
  private lookUp = new Subject<LookUpRelation>();
  lookUpSelected$ = this.lookUp.asObservable();
  addLookUp(lookUpRelation: LookUpRelation) {
    this.lookUp.next(lookUpRelation);
  }
}
