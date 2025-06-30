import { MdbSelect } from './mdb-select';

export class LookUpRelation {
    key: string;
    lookUp: MdbSelect[] = [];
    constructor(key: string, lookUps: MdbSelect[]) {
      this.key = key;
      this.lookUp = lookUps;
    }
  }