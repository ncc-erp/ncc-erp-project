import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormResourceRequestCVUserComponent } from './form-resourceRequestCVUser.component';

describe('FormResourceRequestCVUserComponent', () => {
  let component:FormResourceRequestCVUserComponent;
  let fixture: ComponentFixture<FormResourceRequestCVUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormResourceRequestCVUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormResourceRequestCVUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
