import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceRequestDetailComponent } from './resource-request-detail.component';

describe('ResourceRequestDetailComponent', () => {
  let component: ResourceRequestDetailComponent;
  let fixture: ComponentFixture<ResourceRequestDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourceRequestDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourceRequestDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
