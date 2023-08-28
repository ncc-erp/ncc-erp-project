import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditCriteriaComponent } from './create-edit-criteria.component';

describe('CreateEditCriteriaComponent', () => {
  let component: CreateEditCriteriaComponent;
  let fixture: ComponentFixture<CreateEditCriteriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditCriteriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditCriteriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
