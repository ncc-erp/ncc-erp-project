import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingWeeklyReportComponent } from './training-weekly-report.component';

describe('TrainingWeeklyReportComponent', () => {
  let component: TrainingWeeklyReportComponent;
  let fixture: ComponentFixture<TrainingWeeklyReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingWeeklyReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingWeeklyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
