import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddReportNoteComponent } from './add-report-note.component';

describe('AddReportNoteComponent', () => {
  let component: AddReportNoteComponent;
  let fixture: ComponentFixture<AddReportNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddReportNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddReportNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
