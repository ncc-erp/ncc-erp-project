import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTimesheetProjectDialogComponent } from './edit-timesheet-project-dialog.component';

describe('EditTimesheetProjectDialogComponent', () => {
  let component: EditTimesheetProjectDialogComponent;
  let fixture: ComponentFixture<EditTimesheetProjectDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditTimesheetProjectDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTimesheetProjectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
