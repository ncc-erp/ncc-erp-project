import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectHistoryByUserComponent } from './project-history-by-user.component';

describe('ProjectHistoryByUserComponent', () => {
  let component: ProjectHistoryByUserComponent;
  let fixture: ComponentFixture<ProjectHistoryByUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectHistoryByUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectHistoryByUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
