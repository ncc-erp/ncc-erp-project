import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewYourselfComponent } from './review-yourself.component';

describe('ReviewYourselfComponent', () => {
  let component: ReviewYourselfComponent;
  let fixture: ComponentFixture<ReviewYourselfComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewYourselfComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewYourselfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
