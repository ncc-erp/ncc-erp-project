import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OnboardUserDialogComponent } from './onboard-user-dialog.component';

describe('OnboardUserDialogComponent', () => {
  let component: OnboardUserDialogComponent;
  let fixture: ComponentFixture<OnboardUserDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OnboardUserDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OnboardUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
