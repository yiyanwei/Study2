import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import { HttpService } from '../../Services/http.service';

@Component({
  selector: 'app-pcontent',
  templateUrl: './pcontent.component.html',
  styleUrls: ['./pcontent.component.scss']
})
export class PcontentComponent implements OnInit {

  public product:any[]=[];
  public domain:string="http://a.itying.com/";

  constructor(public route:ActivatedRoute,public http:HttpService) { }

  ngOnInit() {
    //通过动态路由的方式获取传值id
    this.route.params.subscribe((data:any)=>{
      this.requestContent(data.id);
    });
  }

  requestContent(id:string){
    var api = 'api/productcontent?id='+id;
    this.http.get(api).subscribe((data:any)=>{
      console.log(data);
      this.product = data.result[0];
    });
  }
}
