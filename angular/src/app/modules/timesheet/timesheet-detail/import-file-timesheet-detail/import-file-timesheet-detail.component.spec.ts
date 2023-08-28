import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportFileTimesheetDetailComponent } from './import-file-timesheet-detail.component';

describe('ImportFileTimesheetDetailComponent', () => {
  let component: ImportFileTimesheetDetailComponent;
  let fixture: ComponentFixture<ImportFileTimesheetDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportFileTimesheetDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportFileTimesheetDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
