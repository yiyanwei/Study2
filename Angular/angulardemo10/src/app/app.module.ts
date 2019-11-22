import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import{HttpClientModule,HttpClientJsonpModule} from '@angular/common/http';

//添加http请求服务
import {HttpService} from './Services/http.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IndexComponent } from './Components/index/index.component';
import { PcontentComponent } from './Components/pcontent/pcontent.component';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    PcontentComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    HttpClientJsonpModule
  ],
  providers: [HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
