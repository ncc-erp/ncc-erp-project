import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAuditResultDetailComponent } from './edit-audit-result-detail.component';

describe('EditAuditResultDetailComponent', () => {
  let component: EditAuditResultDetailComponent;
  let fixture: ComponentFixture<EditAuditResultDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAuditResultDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAuditResultDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
