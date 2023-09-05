import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditPhaseComponent } from './create-edit-phase.component';

describe('CreateEditPhaseComponent', () => {
  let component: CreateEditPhaseComponent;
  let fixture: ComponentFixture<CreateEditPhaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditPhaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditPhaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
