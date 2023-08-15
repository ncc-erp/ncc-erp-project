import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateResourceRequestComponent } from './create-update-resource-request.component';

describe('CreateUpdateResourceRequestComponent', () => {
  let component: CreateUpdateResourceRequestComponent;
  let fixture: ComponentFixture<CreateUpdateResourceRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateResourceRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateResourceRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
