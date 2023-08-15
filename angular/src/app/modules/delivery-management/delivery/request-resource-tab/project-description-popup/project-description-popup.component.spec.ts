import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectDescriptionPopupComponent } from './project-description-popup.component';

describe('ProjectDescriptionPopupComponent', () => {
  let component: ProjectDescriptionPopupComponent;
  let fixture: ComponentFixture<ProjectDescriptionPopupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectDescriptionPopupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectDescriptionPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
