import { LookUp } from './api/responses/lookup';


export class State extends LookUp {
    abbreviation: string;
    constructor(abbreviation: string)
    {
        super();
        this.abbreviation = abbreviation;
    }
}

