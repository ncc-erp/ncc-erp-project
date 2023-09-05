import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectDescriptionComponent } from './product-project-description.component';

describe('ProductProjectDescriptionComponent', () => {
  let component: ProductProjectDescriptionComponent;
  let fixture: ComponentFixture<ProductProjectDescriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectDescriptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
