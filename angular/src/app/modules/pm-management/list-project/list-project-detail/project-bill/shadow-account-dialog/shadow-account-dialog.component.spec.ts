import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShadowAccountDialogComponent } from './shadow-account-dialog.component';

describe('ShadowAccountDialogComponent', () => {
  let component: ShadowAccountDialogComponent;
  let fixture: ComponentFixture<ShadowAccountDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShadowAccountDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShadowAccountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
