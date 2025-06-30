import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { ChartsModule } from 'ng-uikit-pro-standard';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';


@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    ChartsModule
  ],
  declarations: [
    AdminComponent
  ],
  providers: [
    AuthService,
    AuthGuard
  ]
})

export class AdminModule { }
