import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectChecklistComponent } from './product-project-checklist.component';

describe('ProductProjectChecklistComponent', () => {
  let component: ProductProjectChecklistComponent;
  let fixture: ComponentFixture<ProductProjectChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
