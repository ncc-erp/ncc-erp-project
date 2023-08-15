import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetUpReviewerComponent } from './set-up-reviewer.component';

describe('SetUpReviewerComponent', () => {
  let component: SetUpReviewerComponent;
  let fixture: ComponentFixture<SetUpReviewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetUpReviewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetUpReviewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
