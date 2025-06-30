import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Shell } from 'src/app/shell/shell.service';
import { ClassSearchComponent } from './class-search/class-search.component';
import { ClassAddComponent } from './class-add/class-add.component';
import { ClassEditComponent } from './class-edit/class-edit.component';
import { ClassDetailComponent } from './class-detail/class-detail.component';

const routes: Routes = [
  Shell.childRoutes([
    { path: 'Class/search', component: ClassSearchComponent, /*  canActivate: [AuthGuard] */ },
    { path: 'class/add', component: ClassAddComponent, /* canActivate: [AuthGuard] */ },
    { path: 'class/:id', component: ClassDetailComponent, /* canActivate: [AuthGuard] */ },
    { path: 'class/:id/edit', component: ClassEditComponent, /* canActivate: [AuthGuard] */ },
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class ClassRoutingModule { }
