import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Class } from 'src/app/shared/models/class';

@Component({
  selector: 'app-class-card',
  templateUrl: './class-card.component.html',
  styleUrls: ['./class-card.component.css']
})
export class ClassCardComponent {
  constructor(private router: Router) { }
  @Input() class: Class;

  studioClicked() {
    this.router.navigate(['/detail/' + this.class.Id]);
  }

}
