import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BillAccountDialogComponent } from './bill-account-dialog.component';

describe('BillAccountDialogComponent', () => {
  let component: BillAccountDialogComponent;
  let fixture: ComponentFixture<BillAccountDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BillAccountDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BillAccountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
