import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewUserComponent } from './review-user.component';

describe('ReviewUserComponent', () => {
  let component: ReviewUserComponent;
  let fixture: ComponentFixture<ReviewUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
