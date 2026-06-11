import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditProjectResourceComponent } from './create-edit-project-resource.component';

describe('CreateEditProjectResourceComponent', () => {
  let component: CreateEditProjectResourceComponent;
  let fixture: ComponentFixture<CreateEditProjectResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditProjectResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditProjectResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
