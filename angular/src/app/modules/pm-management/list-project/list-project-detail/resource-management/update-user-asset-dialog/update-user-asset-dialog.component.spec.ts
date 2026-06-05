import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateUserAssetDialogComponent } from './update-user-asset-dialog.component';

describe('UpdateUserAssetDialogComponent', () => {
  let component: UpdateUserAssetDialogComponent;
  let fixture: ComponentFixture<UpdateUserAssetDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateUserAssetDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateUserAssetDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
