import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-newscontent2',
  templateUrl: './newscontent2.component.html',
  styleUrls: ['./newscontent2.component.scss']
})
export class Newscontent2Component implements OnInit {

  constructor(public route:ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe((data)=>{
      console.log(data);
    });
  }

}
