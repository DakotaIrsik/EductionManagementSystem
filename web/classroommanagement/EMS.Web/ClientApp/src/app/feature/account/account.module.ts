import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { AuthService } from 'src/app/core/authentication/auth.service';
import { FormsModule } from '@angular/forms';
import { AccountRoutingModule } from './account-routing-module.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
  ],
  providers: [AuthService],
  imports: [
    CommonModule,
    FormsModule,
    AccountRoutingModule,
    SharedModule,
  ]
})
export class AccountModule { }
