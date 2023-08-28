import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectFileComponent } from './training-project-file.component';

describe('TrainingProjectFileComponent', () => {
  let component: TrainingProjectFileComponent;
  let fixture: ComponentFixture<TrainingProjectFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
