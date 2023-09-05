import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSendRecruitmentComponent } from './form-send-recruitment.component';

describe('FormSendRecruitmentComponent', () => {
  let component: FormSendRecruitmentComponent;
  let fixture: ComponentFixture<FormSendRecruitmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSendRecruitmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSendRecruitmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
