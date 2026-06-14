import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OnboardingChecklistComponent  } from './onboarding-checklist.component';

describe('OnboardingChecklistComponent', () => {
  let component: OnboardingChecklistComponent;
  let fixture: ComponentFixture<OnboardingChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OnboardingChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OnboardingChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
