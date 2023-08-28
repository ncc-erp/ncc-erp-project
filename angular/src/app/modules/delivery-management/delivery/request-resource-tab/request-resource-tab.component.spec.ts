import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestResourceTabComponent } from './request-resource-tab.component';

describe('RequestResourceTabComponent', () => {
  let component: RequestResourceTabComponent;
  let fixture: ComponentFixture<RequestResourceTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestResourceTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestResourceTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
