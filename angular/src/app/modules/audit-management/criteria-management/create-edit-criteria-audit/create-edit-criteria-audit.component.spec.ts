import { CreateEditCriteriaAuditComponent } from "./../create-edit-criteria-audit/create-edit-criteria-audit.component";
import { async, ComponentFixture, TestBed } from "@angular/core/testing";

describe("CreateEditCriteriaComponent", () => {
  let component: CreateEditCriteriaAuditComponent;
  let fixture: ComponentFixture<CreateEditCriteriaAuditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CreateEditCriteriaAuditComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditCriteriaAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it("should create", () => {
    expect(component).toBeTruthy();
  });
});
