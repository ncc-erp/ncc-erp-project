import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RadioDropdownComponent } from './radio-dropdown.component';

describe('RadioDropdownComponent', () => {
  let component: RadioDropdownComponent;
  let fixture: ComponentFixture<RadioDropdownComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RadioDropdownComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RadioDropdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
