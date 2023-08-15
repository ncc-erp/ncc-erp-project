import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSubInvoiceDialogComponent } from './add-sub-invoice-dialog.component';

describe('AddSubInvoiceDialogComponent', () => {
  let component: AddSubInvoiceDialogComponent;
  let fixture: ComponentFixture<AddSubInvoiceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddSubInvoiceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddSubInvoiceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
