import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectGeneralComponent } from './training-project-general.component';

describe('TrainingProjectGeneralComponent', () => {
  let component: TrainingProjectGeneralComponent;
  let fixture: ComponentFixture<TrainingProjectGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
