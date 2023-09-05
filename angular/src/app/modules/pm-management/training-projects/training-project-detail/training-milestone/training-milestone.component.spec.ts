import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingMilestoneComponent } from './training-milestone.component';

describe('TrainingMilestoneComponent', () => {
  let component: TrainingMilestoneComponent;
  let fixture: ComponentFixture<TrainingMilestoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingMilestoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingMilestoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
