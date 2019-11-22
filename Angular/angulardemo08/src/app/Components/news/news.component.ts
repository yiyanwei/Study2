import { Component, OnInit } from '@angular/core';
import { listLazyRoutes } from '@angular/compiler/src/aot/lazy_routes';
import { Router, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.scss']
})
export class NewsComponent implements OnInit {

  public list: any[] = []
  constructor(public router: Router) {
    for (var i = 0; i < 10; i++) {
      this.list.push('我是第' + i + '新闻详情组件');
    }
  }

  ngOnInit() {

  }

  redirectGetJs() {
    //get方式传参js跳转
    var navExtras: NavigationExtras = {
      queryParams: {
        aid: 125
      }
    };
    this.router.navigate(['newscontent'],navExtras);
  }

  redirectRouteJs() {
    //路由传参方式js跳转
    this.router.navigate(['newscontent2', '123']);
  }

}
