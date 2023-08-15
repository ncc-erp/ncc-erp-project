import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditResultReviewerComponent } from './edit-result-reviewer.component';

describe('EditResultReviewerComponent', () => {
  let component: EditResultReviewerComponent;
  let fixture: ComponentFixture<EditResultReviewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditResultReviewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditResultReviewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
