import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRiskDialogComponent } from './add-risk-dialog.component';

describe('AddRiskDialogComponent', () => {
  let component: AddRiskDialogComponent;
  let fixture: ComponentFixture<AddRiskDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddRiskDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRiskDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
