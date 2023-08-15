import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectChecklistComponent } from './training-project-checklist.component';

describe('TrainingProjectChecklistComponent', () => {
  let component: TrainingProjectChecklistComponent;
  let fixture: ComponentFixture<TrainingProjectChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
