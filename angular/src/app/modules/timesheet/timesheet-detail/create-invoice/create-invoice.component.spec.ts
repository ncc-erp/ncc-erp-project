import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateInvoiceComponent } from './create-invoice.component';

describe('CreateInvoiceComponent', () => {
  let component: CreateInvoiceComponent;
  let fixture: ComponentFixture<CreateInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
