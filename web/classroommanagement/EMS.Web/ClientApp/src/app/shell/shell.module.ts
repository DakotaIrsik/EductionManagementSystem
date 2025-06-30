import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ShellComponent } from './shell.component';
import { NavbarComponent } from './navbar-top/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { NavbarModule, WavesModule, ButtonsModule } from 'ng-uikit-pro-standard';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NgScrollbarModule,
    NavbarModule,
    WavesModule,
    ButtonsModule
  ],
  declarations: [
    ShellComponent,
    NavbarComponent,
    FooterComponent
  ]
})
export class ShellModule { }
