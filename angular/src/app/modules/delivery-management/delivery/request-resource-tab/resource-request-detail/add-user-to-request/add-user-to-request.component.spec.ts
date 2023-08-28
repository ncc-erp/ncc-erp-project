import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserToRequestComponent } from './add-user-to-request.component';

describe('AddUserToRequestComponent', () => {
  let component: AddUserToRequestComponent;
  let fixture: ComponentFixture<AddUserToRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUserToRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserToRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
