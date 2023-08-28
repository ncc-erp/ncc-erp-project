import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateConfirmModalComponent } from './update-confirm-modal.component';

describe('UpdateConfirmModalComponent', () => {
  let component: UpdateConfirmModalComponent;
  let fixture: ComponentFixture<UpdateConfirmModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateConfirmModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateConfirmModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
