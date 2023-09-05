import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditProductProjectComponent } from './create-edit-product-project.component';

describe('CreateEditProductProjectComponent', () => {
  let component: CreateEditProductProjectComponent;
  let fixture: ComponentFixture<CreateEditProductProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditProductProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditProductProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
