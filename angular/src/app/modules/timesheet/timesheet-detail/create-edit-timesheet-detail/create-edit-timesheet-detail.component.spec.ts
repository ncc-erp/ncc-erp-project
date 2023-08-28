import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditTimesheetDetailComponent } from './create-edit-timesheet-detail.component';

describe('CreateEditTimesheetDetailComponent', () => {
  let component: CreateEditTimesheetDetailComponent;
  let fixture: ComponentFixture<CreateEditTimesheetDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditTimesheetDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditTimesheetDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
