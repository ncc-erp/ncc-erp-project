import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BillAccountPlanComponent } from './bill-account-plan.component';

describe('BillAccountPlanComponent', () => {
  let component: BillAccountPlanComponent;
  let fixture: ComponentFixture<BillAccountPlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BillAccountPlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BillAccountPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
