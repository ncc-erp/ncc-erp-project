import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserInRoleComponent } from './add-user-in-role.component';

describe('AddUserInRoleComponent', () => {
  let component: AddUserInRoleComponent;
  let fixture: ComponentFixture<AddUserInRoleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUserInRoleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserInRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
