import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { Class } from 'src/app/shared/models/class';
import { NgScrollbar } from 'ngx-scrollbar';
import { ClassService } from '../class.service';
import { fromEvent } from 'rxjs';
import { map, filter, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ClassSearchRequest } from './class-search-request';


@Component({
  selector: 'app-class-search',
  templateUrl: './class-search.component.html',
  styleUrls: ['./class-search.component.css']
})
export class ClassSearchComponent implements OnInit {

  page = 1;
  classes: Class[] = [];

  @Input() searchTerm = '';
  @ViewChild(NgScrollbar, {static: true}) searchResultScrollBar: NgScrollbar;
  @ViewChild('smartSearchInput', {static: true}) smartSearchInput: ElementRef;

  constructor(private classService: ClassService) {
  }

  ngOnInit() {
    fromEvent(this.smartSearchInput.nativeElement, 'keyup').pipe(
      map((event: any) => {
        return event.target.value;
      })
      // , filter(res => res.length > 2)
      , debounceTime(1000)
      , distinctUntilChanged()).subscribe((text: string) => {
        this.loadClasss(text);
      });
    this.loadClasss(this.searchTerm);
  }

  loadClasss($searchText) {
    this.classes = [];
    const classSearchRequest = new ClassSearchRequest();
    classSearchRequest.Title = $searchText;
    classSearchRequest.size = 1500;
    classSearchRequest.sort = '+Lesson';
    this.classService.search(classSearchRequest).subscribe(
      response => this.classes = response.Data
    );
  }

  pageChange($event) {
    this.page = $event;
    this.searchResultScrollBar.smoothScroll.scrollToTop(500);
  }
}
