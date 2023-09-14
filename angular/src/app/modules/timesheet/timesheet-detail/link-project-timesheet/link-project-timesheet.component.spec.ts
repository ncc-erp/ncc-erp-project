import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkProjectTimesheetComponent } from './link-project-timesheet.component';

describe('LinkProjectTimesheetComponent', () => {
  let component: LinkProjectTimesheetComponent;
  let fixture: ComponentFixture<LinkProjectTimesheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinkProjectTimesheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkProjectTimesheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
