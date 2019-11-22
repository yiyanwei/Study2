import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductRoutingModule } from './product-routing.module';
import { ProductComponent } from './product.component';
import { PlistComponent } from './Components/plist/plist.component';
import { CartComponent } from './Components/cart/cart.component';
import { PcateComponent } from './Components/pcate/pcate.component';

@NgModule({
  declarations: [ProductComponent, PlistComponent, CartComponent, PcateComponent],
  imports: [
    CommonModule,
    ProductRoutingModule
  ]
})
export class ProductModule { }
