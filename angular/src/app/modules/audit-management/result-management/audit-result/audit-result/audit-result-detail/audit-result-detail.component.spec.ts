import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditResultDetailComponent } from './audit-result-detail.component';

describe('AuditResultDetailComponent', () => {
  let component: AuditResultDetailComponent;
  let fixture: ComponentFixture<AuditResultDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditResultDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditResultDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
