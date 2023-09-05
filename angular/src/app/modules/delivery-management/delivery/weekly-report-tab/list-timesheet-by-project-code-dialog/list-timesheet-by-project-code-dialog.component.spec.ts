import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListTimesheetByProjectCodeDialogComponent } from './list-timesheet-by-project-code-dialog.component';

describe('ListTimesheetByProjectCodeDialogComponent', () => {
  let component: ListTimesheetByProjectCodeDialogComponent;
  let fixture: ComponentFixture<ListTimesheetByProjectCodeDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListTimesheetByProjectCodeDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListTimesheetByProjectCodeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
