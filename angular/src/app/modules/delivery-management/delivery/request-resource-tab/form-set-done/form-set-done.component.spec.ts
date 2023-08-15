import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormSetDoneComponent } from './form-set-done.component';

describe('FormSetDoneComponent', () => {
  let component: FormSetDoneComponent;
  let fixture: ComponentFixture<FormSetDoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSetDoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSetDoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
