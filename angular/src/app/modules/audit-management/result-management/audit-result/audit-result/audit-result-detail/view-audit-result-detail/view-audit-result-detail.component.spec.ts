import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAuditResultDetailComponent } from './view-audit-result-detail.component';

describe('ViewAuditResultDetailComponent', () => {
  let component: ViewAuditResultDetailComponent;
  let fixture: ComponentFixture<ViewAuditResultDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAuditResultDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAuditResultDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
