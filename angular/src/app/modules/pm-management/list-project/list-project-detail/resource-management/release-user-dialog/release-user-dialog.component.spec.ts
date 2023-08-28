import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReleaseUserDialogComponent } from './release-user-dialog.component';

describe('ReleaseUserDialogComponent', () => {
  let component: ReleaseUserDialogComponent;
  let fixture: ComponentFixture<ReleaseUserDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReleaseUserDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReleaseUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
