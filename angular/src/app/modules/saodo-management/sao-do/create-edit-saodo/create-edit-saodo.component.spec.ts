import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditSaodoComponent } from './create-edit-saodo.component';

describe('CreateEditSaodoComponent', () => {
  let component: CreateEditSaodoComponent;
  let fixture: ComponentFixture<CreateEditSaodoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditSaodoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditSaodoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
