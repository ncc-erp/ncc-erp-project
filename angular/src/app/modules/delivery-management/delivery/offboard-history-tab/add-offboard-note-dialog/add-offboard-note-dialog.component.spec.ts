import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOffboardNoteDialogComponent } from './add-offboard-note-dialog.component';

describe('AddOffboardNoteDialogComponent', () => {
  let component: AddOffboardNoteDialogComponent;
  let fixture: ComponentFixture<AddOffboardNoteDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddOffboardNoteDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddOffboardNoteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
