import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectBillComponent } from './project-bill.component';

describe('ProjectBillComponent', () => {
  let component: ProjectBillComponent;
  let fixture: ComponentFixture<ProjectBillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectBillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectBillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
