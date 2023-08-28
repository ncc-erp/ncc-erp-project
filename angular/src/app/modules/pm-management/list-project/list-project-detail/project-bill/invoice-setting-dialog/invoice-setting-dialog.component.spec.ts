import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceSettingDialogComponent } from './invoice-setting-dialog.component';

describe('InvoiceSettingDialogComponent', () => {
  let component: InvoiceSettingDialogComponent;
  let fixture: ComponentFixture<InvoiceSettingDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvoiceSettingDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceSettingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
