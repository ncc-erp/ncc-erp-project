import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditSaoDoProjectComponent } from './create-edit-sao-do-project.component';

describe('CreateEditSaoDoProjectComponent', () => {
  let component: CreateEditSaoDoProjectComponent;
  let fixture: ComponentFixture<CreateEditSaoDoProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditSaoDoProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditSaoDoProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
