import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanUserComponent } from './plan-user.component';

describe('PlanUserComponent', () => {
  let component: PlanUserComponent;
  let fixture: ComponentFixture<PlanUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
