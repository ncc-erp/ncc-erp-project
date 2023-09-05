import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CriteriaManagementComponent } from './criteria-management.component';

describe('CriteriaManagementComponent', () => {
  let component: CriteriaManagementComponent;
  let fixture: ComponentFixture<CriteriaManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CriteriaManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CriteriaManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
