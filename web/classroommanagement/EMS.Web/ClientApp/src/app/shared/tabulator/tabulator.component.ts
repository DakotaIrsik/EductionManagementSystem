import { Component, OnInit, Input, OnChanges, SimpleChanges, Injectable } from '@angular/core';
import Tabulator from 'tabulator-tables';

@Component({
  selector: 'studio-portal-tabulator',
  templateUrl: './tabulator.component.html',
  styleUrls: ['./tabulator.component.scss']
})
export class TabulatorComponent implements OnChanges {

  @Input() tableData: any[] = [];
  @Input() columnNames: any[] = [];
  @Input() height: string = '600px';
  // list properties you want to set per implementation here...

  tab = document.createElement('div');

  constructor() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.drawTable();
  }

  public drawTable(): void {
// tslint:disable-next-line: no-unused-expression
    const x = new Tabulator(this.tab, {
      data: this.tableData,
      reactiveData: true,
      columns: this.columnNames,
      layout: 'fitColumns',
      height: this.height,

    });
    document.getElementById('my-tabular-table').appendChild(this.tab);
    x.setSort('createdate', 'desc');
  }
}
