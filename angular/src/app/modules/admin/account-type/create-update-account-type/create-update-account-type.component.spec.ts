import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateAccountTypeComponent } from './create-update-account-type.component';

describe('CreateUpdateAccountTypeComponent', () => {
  let component: CreateUpdateAccountTypeComponent;
  let fixture: ComponentFixture<CreateUpdateAccountTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateAccountTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateAccountTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
