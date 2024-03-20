import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultipleSelectResourceRequestCodeComponent } from './multiple-select-resource-request-code.component';

describe('MultipleSelectResourceRequestCodeComponent', () => {
  let component: MultipleSelectResourceRequestCodeComponent;
  let fixture: ComponentFixture<MultipleSelectResourceRequestCodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultipleSelectResourceRequestCodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultipleSelectResourceRequestCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
});
