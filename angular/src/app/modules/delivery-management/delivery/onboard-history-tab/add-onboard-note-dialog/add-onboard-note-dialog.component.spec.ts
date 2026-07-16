import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOnboardNoteDialogComponent } from './add-onboard-note-dialog.component';

describe('AddOnboardNoteDialogComponent', () => {
  let component: AddOnboardNoteDialogComponent;
  let fixture: ComponentFixture<AddOnboardNoteDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddOnboardNoteDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddOnboardNoteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
