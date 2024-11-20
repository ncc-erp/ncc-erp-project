import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadCvBillAccountComponent } from './upload-cv-bill-account.component';

describe('UploadCvBillAccountComponent', () => {
  let component: UploadCvBillAccountComponent;
  let fixture: ComponentFixture<UploadCvBillAccountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadCvBillAccountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadCvBillAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
