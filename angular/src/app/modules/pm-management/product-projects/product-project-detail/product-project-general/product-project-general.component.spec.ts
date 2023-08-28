import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectGeneralComponent } from './product-project-general.component';

describe('ProductProjectGeneralComponent', () => {
  let component: ProductProjectGeneralComponent;
  let fixture: ComponentFixture<ProductProjectGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
