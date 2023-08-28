import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductProjectMilestoneComponent } from './product-project-milestone.component';

describe('ProductProjectMilestoneComponent', () => {
  let component: ProductProjectMilestoneComponent;
  let fixture: ComponentFixture<ProductProjectMilestoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductProjectMilestoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductProjectMilestoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
