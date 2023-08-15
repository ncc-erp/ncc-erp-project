import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GetTimesheetWorkingComponent } from './get-timesheet-working.component';

describe('GetTimesheetWorkingComponent', () => {
  let component: GetTimesheetWorkingComponent;
  let fixture: ComponentFixture<GetTimesheetWorkingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GetTimesheetWorkingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GetTimesheetWorkingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
