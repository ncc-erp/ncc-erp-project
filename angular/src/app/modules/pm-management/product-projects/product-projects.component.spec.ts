import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectsComponent } from './product-projects.component';

describe('ProductProjectsComponent', () => {
  let component: ProductProjectsComponent;
  let fixture: ComponentFixture<ProductProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
