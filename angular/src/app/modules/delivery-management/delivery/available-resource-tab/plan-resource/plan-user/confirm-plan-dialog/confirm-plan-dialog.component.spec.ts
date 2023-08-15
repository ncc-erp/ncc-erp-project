import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmPlanDialogComponent } from './confirm-plan-dialog.component';

describe('ConfirmPlanDialogComponent', () => {
  let component: ConfirmPlanDialogComponent;
  let fixture: ComponentFixture<ConfirmPlanDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmPlanDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmPlanDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
