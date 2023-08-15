import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditCriteriaCategoryComponent } from './create-edit-criteria-category.component';

describe('CreateEditCriteriaCategoryComponent', () => {
  let component: CreateEditCriteriaCategoryComponent;
  let fixture: ComponentFixture<CreateEditCriteriaCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditCriteriaCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditCriteriaCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
