import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingRequestTabComponent } from './training-request-tab.component';

describe('TrainingRequestTabComponent', () => {
  let component: TrainingRequestTabComponent;
  let fixture: ComponentFixture<TrainingRequestTabComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrainingRequestTabComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingRequestTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
