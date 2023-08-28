import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResultReviewerComponent } from './result-reviewer.component';

describe('ResultReviewerComponent', () => {
  let component: ResultReviewerComponent;
  let fixture: ComponentFixture<ResultReviewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResultReviewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResultReviewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
