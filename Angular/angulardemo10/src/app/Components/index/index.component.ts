import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../Services/http.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  public prolist: any[] = [];
  constructor(public http: HttpService) { }

  ngOnInit() {
    //使用promise的方式
    // this.http.getByPromise("api/productlist").then((data)=>{
    //   console.log(data);
    // });
    //使用Observable的方式
    this.http.get("api/productlist")
      .subscribe(
        (data:any) => {
          console.log(data);
          this.prolist = data.result;
        }, (error) => {

        });
  }

}
