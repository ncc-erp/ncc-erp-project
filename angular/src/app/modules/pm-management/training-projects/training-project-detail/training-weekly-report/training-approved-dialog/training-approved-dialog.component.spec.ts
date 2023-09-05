import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingApprovedDialogComponent } from './training-approved-dialog.component';

describe('TrainingApprovedDialogComponent', () => {
  let component: TrainingApprovedDialogComponent;
  let fixture: ComponentFixture<TrainingApprovedDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingApprovedDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingApprovedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
