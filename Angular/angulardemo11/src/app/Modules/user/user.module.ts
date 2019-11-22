import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { AddressComponent } from './Components/address/address.component';

@NgModule({
  declarations: [UserComponent, ProfileComponent, AddressComponent],
  imports: [
    CommonModule,
    UserRoutingModule
  ]
})
export class UserModule { }
