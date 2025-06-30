import { Component, ViewChild } from '@angular/core';
import { NgScrollbar } from 'ngx-scrollbar';
import { Router, NavigationEnd } from '@angular/router';
import { filter, tap } from 'rxjs/operators';
import { ApplicationResolver } from '../shared/base/application.resolver';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.css']
})
export class ShellComponent {
  @ViewChild(NgScrollbar, {static: true}) shellScrollbar: NgScrollbar;

  application: string;

  constructor(router: Router) {
    this.application = ApplicationResolver.getConfiguration();
    router.events.pipe(
      filter(event => event instanceof NavigationEnd)),
      filter(() => !!this.shellScrollbar),
      tap((event: NavigationEnd) => this.shellScrollbar.scrollToTop());
  }
}
