import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportInvoiceComponent } from './export-invoice.component';

describe('ExportInvoiceComponent', () => {
  let component: ExportInvoiceComponent;
  let fixture: ComponentFixture<ExportInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
