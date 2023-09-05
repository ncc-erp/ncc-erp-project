import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectTimesheetComponent } from './product-project-timesheet.component';

describe('ProductProjectTimesheetComponent', () => {
  let component: ProductProjectTimesheetComponent;
  let fixture: ComponentFixture<ProductProjectTimesheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectTimesheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectTimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
