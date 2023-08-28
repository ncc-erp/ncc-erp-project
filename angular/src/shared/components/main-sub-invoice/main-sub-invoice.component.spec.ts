import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MainSubInvoiceComponent } from './main-sub-invoice.component';

describe('MainSubInvoiceComponent', () => {
  let component: MainSubInvoiceComponent;
  let fixture: ComponentFixture<MainSubInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MainSubInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MainSubInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
