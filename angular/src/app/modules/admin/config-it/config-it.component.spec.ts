import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigItComponent } from './config-it.component';

describe('ConfigItComponent', () => {
  let component: ConfigItComponent;
  let fixture: ComponentFixture<ConfigItComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfigItComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfigItComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
