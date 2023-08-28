import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddResultCriteriaComponent } from './add-result-criteria.component';

describe('AddResultCriteriaComponent', () => {
  let component: AddResultCriteriaComponent;
  let fixture: ComponentFixture<AddResultCriteriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddResultCriteriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddResultCriteriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
