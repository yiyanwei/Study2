import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//在user模块的路由里引入相关组件
import { ProductComponent } from './product.component';
import { CartComponent } from './Components/cart/cart.component';
import { PlistComponent } from './Components/plist/plist.component';
import { PcateComponent } from './Components/pcate/pcate.component';

const routes: Routes = [{
  path: '', component: ProductComponent,
  children: [{
    path: 'cart',
    component: CartComponent
  }, {
    path: 'plist',
    component: PlistComponent
  }, {
    path: '**',
    redirectTo: 'plist'
  }]
}, {
  path: 'pcate',
  component: PcateComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductRoutingModule { }
