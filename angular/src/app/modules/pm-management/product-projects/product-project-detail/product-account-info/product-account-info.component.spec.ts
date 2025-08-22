import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAccountInfoComponent } from './product-account-info.component';

describe('ProductAccountInfoComponent', () => {
  let component: ProductAccountInfoComponent;
  let fixture: ComponentFixture<ProductAccountInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductAccountInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductAccountInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
