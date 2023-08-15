import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportAuditResultComponent } from './import-audit-result.component';

describe('ImportAuditResultComponent', () => {
  let component: ImportAuditResultComponent;
  let fixture: ComponentFixture<ImportAuditResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportAuditResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportAuditResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
