import { Component } from '@angular/core';
import { Router,  } from '@angular/router';
import { ApplicationResolver } from './shared/base/application.resolver';
@Component({
  selector: 'app-silverleaf-classroommanagement',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = ApplicationResolver.getConfiguration();
  constructor(router: Router) {
/*     router.events.pipe(
      filter(event => event instanceof NavigationEnd)),
      filter(() => !!this.appScrollbar),
      tap((event: NavigationEnd) => this.appScrollbar.scrollToTop()); */
  }
}
