import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RetroReviewHistoryByUserComponent } from './retro-review-history-by-user.component';

describe('RetroReviewHistoryByUserComponent', () => {
  let component: RetroReviewHistoryByUserComponent;
  let fixture: ComponentFixture<RetroReviewHistoryByUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RetroReviewHistoryByUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RetroReviewHistoryByUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
