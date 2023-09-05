import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditChecklistTitleComponent } from './create-edit-checklist-title.component';

describe('CreateEditChecklistTitleComponent', () => {
  let component: CreateEditChecklistTitleComponent;
  let fixture: ComponentFixture<CreateEditChecklistTitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditChecklistTitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditChecklistTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
