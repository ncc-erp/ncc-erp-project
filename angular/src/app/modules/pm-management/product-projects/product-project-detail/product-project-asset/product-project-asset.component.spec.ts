import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectAssetComponent } from './product-project-asset.component';

describe('ProductProjectAssetComponent', () => {
  let component: ProductProjectAssetComponent;
  let fixture: ComponentFixture<ProductProjectAssetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectAssetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
