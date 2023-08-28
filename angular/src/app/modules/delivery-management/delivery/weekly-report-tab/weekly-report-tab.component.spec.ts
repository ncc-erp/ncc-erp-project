import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeeklyReportTabComponent } from './weekly-report-tab.component';

describe('WeeklyReportTabComponent', () => {
  let component: WeeklyReportTabComponent;
  let fixture: ComponentFixture<WeeklyReportTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeeklyReportTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeeklyReportTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
