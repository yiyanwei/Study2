import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './Components/home/home.component';
import { ProductComponent } from './Components/product/product.component';
import { WelcomeComponent } from './Components/home/welcome/welcome.component';
import { SysconfigComponent } from './Components/home/sysconfig/sysconfig.component';
import { PcateComponent } from './Components/product/pcate/pcate.component';
import { PlistComponent } from './Components/product/plist/plist.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    children: [
      {
        path: 'welcome',
        component: WelcomeComponent
      },
      {
        path: 'sysconfig',
        component: SysconfigComponent
      }, {
        path: '**',
        redirectTo: 'welcome'
      }
    ]
  },
  {
    path: 'product',
    component: ProductComponent,
    children: [
      {
        path: 'pcate',
        component: PcateComponent
      },
      {
        path: 'plist',
        component: PlistComponent
      }, {
        path: '**',
        redirectTo: 'plist'
      }
    ]
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
