import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectProjectTailoringComponent } from './select-project-tailoring.component';

describe('SelectProjectTailoringComponent', () => {
  let component: SelectProjectTailoringComponent;
  let fixture: ComponentFixture<SelectProjectTailoringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectProjectTailoringComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectProjectTailoringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
