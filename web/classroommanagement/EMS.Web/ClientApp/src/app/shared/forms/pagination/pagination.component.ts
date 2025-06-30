import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { PaginationInstance } from 'ngx-pagination';

@Component({
  selector: 'studio-portal-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PaginationComponent  {

  @Input('data') meals: string[] = [];

  public config: PaginationInstance = {
      id: 'custom',
      itemsPerPage: 10,
      currentPage: 1
  };

}
