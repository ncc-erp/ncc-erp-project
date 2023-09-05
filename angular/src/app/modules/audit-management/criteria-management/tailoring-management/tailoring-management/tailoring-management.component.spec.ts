import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TailoringManagementComponent } from './tailoring-management.component';

describe('TailoringManagementComponent', () => {
  let component: TailoringManagementComponent;
  let fixture: ComponentFixture<TailoringManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TailoringManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TailoringManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
