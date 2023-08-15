import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectTimesheetComponent } from './training-project-timesheet.component';

describe('TrainingProjectTimesheetComponent', () => {
  let component: TrainingProjectTimesheetComponent;
  let fixture: ComponentFixture<TrainingProjectTimesheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectTimesheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectTimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
