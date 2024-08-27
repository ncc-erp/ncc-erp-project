import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceRequestCVComponent } from './form-resource-requestCV.component';

describe('ResourceRequestCVComponent', () => {
  let component: ResourceRequestCVComponent;
  let fixture: ComponentFixture<ResourceRequestCVComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceRequestCVComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceRequestCVComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
