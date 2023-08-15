import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectChecklistComponent } from './project-checklist.component';

describe('ProjectChecklistComponent', () => {
  let component: ProjectChecklistComponent;
  let fixture: ComponentFixture<ProjectChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
