import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFutureResourceDialogComponent } from './add-future-resource-dialog.component';

describe('AddFutureResourceDialogComponent', () => {
  let component: AddFutureResourceDialogComponent;
  let fixture: ComponentFixture<AddFutureResourceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddFutureResourceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddFutureResourceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
