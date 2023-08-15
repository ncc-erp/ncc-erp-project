import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportGuidelineDetailComponent } from './report-guideline-detail.component';

describe('ReportGuidelineDetailComponent', () => {
  let component: ReportGuidelineDetailComponent;
  let fixture: ComponentFixture<ReportGuidelineDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportGuidelineDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportGuidelineDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
