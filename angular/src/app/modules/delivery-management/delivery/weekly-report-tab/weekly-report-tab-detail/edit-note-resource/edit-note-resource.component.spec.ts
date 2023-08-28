import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditNoteResourceComponent } from './edit-note-resource.component';

describe('EditNoteResourceComponent', () => {
  let component: EditNoteResourceComponent;
  let fixture: ComponentFixture<EditNoteResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditNoteResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditNoteResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
