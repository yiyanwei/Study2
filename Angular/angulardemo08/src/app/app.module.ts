import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HomeComponent} from './Components/home/home.component';
import {NewsComponent} from './Components/news/news.component';
import {ProductComponent} from './Components/product/product.component';
import { NewscontentComponent } from './Components/newscontent/newscontent.component';
import { Newscontent2Component } from './Components/newscontent2/newscontent2.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NewsComponent,
    ProductComponent,
    NewscontentComponent,
    Newscontent2Component
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
