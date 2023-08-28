import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditProjectChecklistComponent } from './create-edit-project-checklist.component';

describe('CreateEditProjectChecklistComponent', () => {
  let component: CreateEditProjectChecklistComponent;
  let fixture: ComponentFixture<CreateEditProjectChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditProjectChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditProjectChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
