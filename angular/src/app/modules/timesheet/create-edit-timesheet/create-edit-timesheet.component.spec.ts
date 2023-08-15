import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditTimesheetComponent } from './create-edit-timesheet.component';

describe('CreateEditTimesheetComponent', () => {
  let component: CreateEditTimesheetComponent;
  let fixture: ComponentFixture<CreateEditTimesheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditTimesheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditTimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
