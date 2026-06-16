import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateTypeLoginComponent } from './create-update-type-login.component';

describe('CreateUpdateTypeLoginComponent', () => {
  let component: CreateUpdateTypeLoginComponent;
  let fixture: ComponentFixture<CreateUpdateTypeLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateTypeLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateTypeLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
