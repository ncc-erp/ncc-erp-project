import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimesheetDetailComponent } from './timesheet-detail.component';

describe('TimesheetDetailComponent', () => {
  let component: TimesheetDetailComponent;
  let fixture: ComponentFixture<TimesheetDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimesheetDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimesheetDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
