import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IndexComponent } from './Components/index/index.component';
import { PcontentComponent } from './Components/pcontent/pcontent.component';

const routes: Routes = [{
  path: 'index',
  component: IndexComponent
}, {
  path: 'pcontent/:id',
  component: PcontentComponent
}, {
  path: '**',
  redirectTo: 'index'
}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
