import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditReviewUserComponent } from './create-edit-review-user.component';

describe('CreateEditReviewUserComponent', () => {
  let component: CreateEditReviewUserComponent;
  let fixture: ComponentFixture<CreateEditReviewUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditReviewUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditReviewUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
