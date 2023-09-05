import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditProductProjectChecklistComponent } from './create-edit-product-project-checklist.component';

describe('CreateEditProductProjectChecklistComponent', () => {
  let component: CreateEditProductProjectChecklistComponent;
  let fixture: ComponentFixture<CreateEditProductProjectChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditProductProjectChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditProductProjectChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
