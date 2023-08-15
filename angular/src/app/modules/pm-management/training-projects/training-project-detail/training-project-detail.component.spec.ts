import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectDetailComponent } from './training-project-detail.component';

describe('TrainingProjectDetailComponent', () => {
  let component: TrainingProjectDetailComponent;
  let fixture: ComponentFixture<TrainingProjectDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
