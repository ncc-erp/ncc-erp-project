import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectFileComponent } from './product-project-file.component';

describe('ProductProjectFileComponent', () => {
  let component: ProductProjectFileComponent;
  let fixture: ComponentFixture<ProductProjectFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
