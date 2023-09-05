import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceManagementComponent } from './resource-management.component';

describe('ResourceManagementComponent', () => {
  let component: ResourceManagementComponent;
  let fixture: ComponentFixture<ResourceManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
