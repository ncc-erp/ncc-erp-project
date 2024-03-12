import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { AllResourceNoteDialog } from "./all-resource-note-dialog.component";

describe("AllResourceNoteDialog", () => {
  let component: AllResourceNoteDialog;
  let fixture: ComponentFixture<AllResourceNoteDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AllResourceNoteDialog],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllResourceNoteDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
