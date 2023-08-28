import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanResourceComponent } from './plan-resource.component';

describe('PlanResourceComponent', () => {
  let component: PlanResourceComponent;
  let fixture: ComponentFixture<PlanResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
