import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserToTempProjectDialogComponent } from './add-user-to-temp-project-dialog.component';

describe('AddUserToTempProjectDialogComponent', () => {
  let component: AddUserToTempProjectDialogComponent;
  let fixture: ComponentFixture<AddUserToTempProjectDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUserToTempProjectDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserToTempProjectDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
