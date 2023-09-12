import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectClearOptionComponent } from './select-clear-option.component';

describe('SelectClearOption', () => {
  let component: SelectClearOptionComponent;
  let fixture: ComponentFixture<SelectClearOptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectClearOptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectClearOptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
