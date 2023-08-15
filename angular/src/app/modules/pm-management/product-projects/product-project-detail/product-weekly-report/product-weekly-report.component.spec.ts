import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductWeeklyReportComponent } from './product-weekly-report.component';

describe('ProductWeeklyReportComponent', () => {
  let component: ProductWeeklyReportComponent;
  let fixture: ComponentFixture<ProductWeeklyReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductWeeklyReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductWeeklyReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
