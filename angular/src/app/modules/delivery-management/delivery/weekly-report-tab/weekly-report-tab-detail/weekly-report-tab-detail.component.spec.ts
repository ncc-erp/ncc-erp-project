import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeeklyReportTabDetailComponent } from './weekly-report-tab-detail.component';

describe('WeeklyReportTabDetailComponent', () => {
  let component: WeeklyReportTabDetailComponent;
  let fixture: ComponentFixture<WeeklyReportTabDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeeklyReportTabDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeeklyReportTabDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
