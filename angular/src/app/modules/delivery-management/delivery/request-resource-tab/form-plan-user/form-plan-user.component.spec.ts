import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormPlanUserComponent } from './form-plan-user.component';

describe('FormPlanUserComponent', () => {
  let component: FormPlanUserComponent;
  let fixture: ComponentFixture<FormPlanUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormPlanUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormPlanUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
