import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectDescriptionComponent } from './training-project-description.component';

describe('TrainingProjectDescriptionComponent', () => {
  let component: TrainingProjectDescriptionComponent;
  let fixture: ComponentFixture<TrainingProjectDescriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectDescriptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
