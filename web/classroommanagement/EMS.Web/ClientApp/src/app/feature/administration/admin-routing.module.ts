import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Shell } from 'src/app/shell/shell.service';
import { FeatureRequestComponent } from './feature-request/feature-request.component';
import { DiagnosticsComponent } from './diagnostics/diagnostics.component';

const routes: Routes = [
  Shell.childRoutes([

    { path: 'diagnostics', component: DiagnosticsComponent, /*  canActivate: [AuthGuard] */ },
    { path: 'featurerequests', component: FeatureRequestComponent, /* canActivate: [AuthGuard] */ },
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class AdminRoutingModule { }
