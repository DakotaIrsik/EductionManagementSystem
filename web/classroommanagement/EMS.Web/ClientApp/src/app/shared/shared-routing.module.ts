import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Shell } from 'src/app/shell/shell.service';
import { ErrorComponent } from './errors/error.component';

const routes: Routes = [
  Shell.childRoutes([
    { path: 'error', component: ErrorComponent, /*canActivate: [AuthGuard]*/ }
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class SharedRoutingModule { }
