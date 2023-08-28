import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllResourceComponent } from './all-resource.component';

describe('AllResourceComponent', () => {
  let component: AllResourceComponent;
  let fixture: ComponentFixture<AllResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
