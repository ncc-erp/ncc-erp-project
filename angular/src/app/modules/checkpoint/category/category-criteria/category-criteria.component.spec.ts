import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryCriteriaComponent } from './category-criteria.component';

describe('CategoryCriteriaComponent', () => {
  let component: CategoryCriteriaComponent;
  let fixture: ComponentFixture<CategoryCriteriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoryCriteriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryCriteriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
