import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAccountResourceDialogComponent } from './add-account-resource-dialog.component';

describe('AddAccountResourceDialogComponent', () => {
  let component: AddAccountResourceDialogComponent;
  let fixture: ComponentFixture<AddAccountResourceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAccountResourceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAccountResourceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
