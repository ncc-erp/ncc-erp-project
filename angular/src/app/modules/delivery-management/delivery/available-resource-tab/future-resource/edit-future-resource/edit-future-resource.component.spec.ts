import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditFutureResourceComponent } from './edit-future-resource.component';

describe('EditFutureResourceComponent', () => {
  let component: EditFutureResourceComponent;
  let fixture: ComponentFixture<EditFutureResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditFutureResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditFutureResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
