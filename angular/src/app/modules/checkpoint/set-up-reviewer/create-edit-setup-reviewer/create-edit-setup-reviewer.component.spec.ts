import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditSetupReviewerComponent } from './create-edit-setup-reviewer.component';

describe('CreateEditSetupReviewerComponent', () => {
  let component: CreateEditSetupReviewerComponent;
  let fixture: ComponentFixture<CreateEditSetupReviewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditSetupReviewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditSetupReviewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
