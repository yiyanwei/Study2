import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './Components/home/home.component';
import { NewsComponent } from './Components/news/news.component';
import { ProductComponent } from './Components/product/product.component';
import { NewscontentComponent } from './Components/newscontent/newscontent.component';
import {Newscontent2Component} from './Components/newscontent2/newscontent2.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'news',
    component: NewsComponent
  },
  {
    path: 'product',
    component: ProductComponent
  },
  {
    path: 'newscontent',
    component: NewscontentComponent
  },
  {
    path:'newscontent2/:aid',
    component:Newscontent2Component
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
