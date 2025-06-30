import { Paging } from '../paging';

export class StandardResponse<T> extends Paging {
    Data: T;
}
