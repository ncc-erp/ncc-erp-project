import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditTrainingProjectComponent } from './create-edit-training-project.component';

describe('CreateEditTrainingProjectComponent', () => {
  let component: CreateEditTrainingProjectComponent;
  let fixture: ComponentFixture<CreateEditTrainingProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditTrainingProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditTrainingProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
