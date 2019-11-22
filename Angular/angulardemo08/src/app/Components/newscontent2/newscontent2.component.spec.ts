import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { Newscontent2Component } from './newscontent2.component';

describe('Newscontent2Component', () => {
  let component: Newscontent2Component;
  let fixture: ComponentFixture<Newscontent2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Newscontent2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Newscontent2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
