import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMeetingNoteDialogComponent } from './edit-meeting-note-dialog.component';

describe('EditMeetingNoteDialogComponent', () => {
  let component: EditMeetingNoteDialogComponent;
  let fixture: ComponentFixture<EditMeetingNoteDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditMeetingNoteDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditMeetingNoteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
