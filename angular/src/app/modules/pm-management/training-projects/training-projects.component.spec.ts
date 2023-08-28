import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectsComponent } from './training-projects.component';

describe('TrainingProjectsComponent', () => {
  let component: TrainingProjectsComponent;
  let fixture: ComponentFixture<TrainingProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
