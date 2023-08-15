import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditResultComponent } from './audit-result.component';

describe('AuditResultComponent', () => {
  let component: AuditResultComponent;
  let fixture: ComponentFixture<AuditResultComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditResultComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
