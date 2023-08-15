import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActiveTimesheetProjectComponent } from './active-timesheet-project.component';

describe('ActiveTimesheetProjectComponent', () => {
  let component: ActiveTimesheetProjectComponent;
  let fixture: ComponentFixture<ActiveTimesheetProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActiveTimesheetProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActiveTimesheetProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
