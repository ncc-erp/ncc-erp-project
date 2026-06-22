import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OnboardHistoryTabComponent } from './onboard-history-tab.component';

describe('OnboardHistoryTabComponent', () => {
  let component: OnboardHistoryTabComponent;
  let fixture: ComponentFixture<OnboardHistoryTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OnboardHistoryTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OnboardHistoryTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
