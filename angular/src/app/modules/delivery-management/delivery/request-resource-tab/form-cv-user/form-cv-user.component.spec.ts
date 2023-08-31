import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCvUserComponent } from './form-cv-user.component';

describe('FormCvUserComponent', () => {
  let component: FormCvUserComponent;
  let fixture: ComponentFixture<FormCvUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCvUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCvUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
