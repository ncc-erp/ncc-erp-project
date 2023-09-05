import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditListProjectComponent } from './create-edit-list-project.component';

describe('CreateEditListProjectComponent', () => {
  let component: CreateEditListProjectComponent;
  let fixture: ComponentFixture<CreateEditListProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditListProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditListProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
