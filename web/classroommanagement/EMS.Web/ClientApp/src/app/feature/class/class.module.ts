import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClassSearchComponent } from './class-search/class-search.component';
import { ClassRoutingModule } from './class-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { ClassAddComponent } from './class-add/class-add.component';
import { ClassEditComponent } from './class-edit/class-edit.component';
import { ClassDetailComponent } from './class-detail/class-detail.component';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { ChartsModule } from 'ng-uikit-pro-standard';
import { ClassService } from './class.service';
import { ClassCardComponent } from './class-card/class-card.component';


@NgModule({
  imports: [
    CommonModule,
    ClassRoutingModule,
    SharedModule,
    ChartsModule
  ],
  declarations: [
    ClassSearchComponent,
    ClassAddComponent,
    ClassEditComponent,
    ClassDetailComponent,
    ClassCardComponent
  ],
  providers: [
    AuthService,
    ClassService,
    //AuthGuard
  ]
})

export class ClassModule { }
