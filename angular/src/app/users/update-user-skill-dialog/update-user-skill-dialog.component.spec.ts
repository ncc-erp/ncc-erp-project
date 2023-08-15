import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateUserSkillDialogComponent } from './update-user-skill-dialog.component';

describe('UpdateUserSkillDialogComponent', () => {
  let component: UpdateUserSkillDialogComponent;
  let fixture: ComponentFixture<UpdateUserSkillDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateUserSkillDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateUserSkillDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
