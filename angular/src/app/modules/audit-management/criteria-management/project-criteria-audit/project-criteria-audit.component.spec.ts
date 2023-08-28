import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectCriteriaAuditComponent } from './project-criteria-audit.component';

describe('ProjectCriteriaAuditComponent', () => {
  let component: ProjectCriteriaAuditComponent;
  let fixture: ComponentFixture<ProjectCriteriaAuditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectCriteriaAuditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectCriteriaAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
