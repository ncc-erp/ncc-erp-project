import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectTimesheetDialogComponent } from './collect-timesheet-dialog.component';

describe('CollectTimesheetDialogComponent', () => {
  let component: CollectTimesheetDialogComponent;
  let fixture: ComponentFixture<CollectTimesheetDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CollectTimesheetDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CollectTimesheetDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
