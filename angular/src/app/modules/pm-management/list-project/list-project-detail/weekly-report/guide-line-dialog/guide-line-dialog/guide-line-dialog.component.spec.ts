import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuideLineDialogComponent } from './guide-line-dialog.component';

describe('GuideLineDialogComponent', () => {
  let component: GuideLineDialogComponent;
  let fixture: ComponentFixture<GuideLineDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuideLineDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuideLineDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
