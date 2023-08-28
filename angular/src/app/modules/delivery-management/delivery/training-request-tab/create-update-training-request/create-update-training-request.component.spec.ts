import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateTrainingRequestComponent } from './create-update-training-request.component';

describe('CreateUpdateTrainingRequestComponent', () => {
  let component: CreateUpdateTrainingRequestComponent;
  let fixture: ComponentFixture<CreateUpdateTrainingRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateUpdateTrainingRequestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateTrainingRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
