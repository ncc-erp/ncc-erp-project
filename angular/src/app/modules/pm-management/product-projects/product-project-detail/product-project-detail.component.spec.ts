import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectDetailComponent } from './product-project-detail.component';

describe('ProductProjectDetailComponent', () => {
  let component: ProductProjectDetailComponent;
  let fixture: ComponentFixture<ProductProjectDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
