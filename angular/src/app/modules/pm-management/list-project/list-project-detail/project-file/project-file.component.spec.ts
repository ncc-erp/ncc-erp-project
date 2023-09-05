import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectFileComponent } from './project-file.component';

describe('ProjectFileComponent', () => {
  let component: ProjectFileComponent;
  let fixture: ComponentFixture<ProjectFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
