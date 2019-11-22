import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private domain: string = "http://a.itying.com/"
  constructor(private http: HttpClient) { }
  get(api: string) {
    //返回一个http请求的Observable对象
    return this.http.get(this.domain+api);
  }
  getParams(api:string){
    //this.http.get()
  }
  getByPromise(api:string)
  {
    return new Promise((resolve,reject)=>{
      this.http.get(this.domain+api).subscribe((data)=>{
        resolve(data);
      });
    });
  }
}
