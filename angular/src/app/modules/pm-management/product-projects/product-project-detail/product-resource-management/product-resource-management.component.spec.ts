import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductResourceManagementComponent } from './product-resource-management.component';

describe('ProductResourceManagementComponent', () => {
  let component: ProductResourceManagementComponent;
  let fixture: ComponentFixture<ProductResourceManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductResourceManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductResourceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
