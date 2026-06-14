import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditConfigItComponent } from './create-edit-config-it.component';

describe('CreateEditConfigItComponent', () => {
  let component: CreateEditConfigItComponent;
  let fixture: ComponentFixture<CreateEditConfigItComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditConfigItComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditConfigItComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
