import { Paging } from '../paging';

export class CitySearchRequest extends Paging  {

    state: string;

    constructor(state: string) {
        super();
        this.size = 100;
        this.fields = 'name';
        this.sort = '+name';
        this.state = state;
    }
}
