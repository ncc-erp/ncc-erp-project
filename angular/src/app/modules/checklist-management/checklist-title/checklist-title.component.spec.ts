import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChecklistTitleComponent } from './checklist-title.component';

describe('ChecklistTitleComponent', () => {
  let component: ChecklistTitleComponent;
  let fixture: ComponentFixture<ChecklistTitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChecklistTitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChecklistTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
