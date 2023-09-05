import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingResourceManagementComponent } from './training-resource-management.component';

describe('TrainingResourceManagementComponent', () => {
  let component: TrainingResourceManagementComponent;
  let fixture: ComponentFixture<TrainingResourceManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingResourceManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingResourceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
