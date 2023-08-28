import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductApprovedDialogComponent } from './product-approved-dialog.component';

describe('ProductApprovedDialogComponent', () => {
  let component: ProductApprovedDialogComponent;
  let fixture: ComponentFixture<ProductApprovedDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductApprovedDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductApprovedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
