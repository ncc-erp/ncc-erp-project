import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultipleSelectStringValueComponent } from './multiple-select-string-value.component';

describe('MultiSelectComponent', () => {
  let component: MultipleSelectStringValueComponent;
  let fixture: ComponentFixture<MultipleSelectStringValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultipleSelectStringValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultipleSelectStringValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
