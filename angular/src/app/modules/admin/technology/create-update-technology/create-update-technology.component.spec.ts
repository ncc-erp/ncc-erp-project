import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateTechnologyComponent } from './create-update-technology.component';

describe('CreateUpdateTechnologyComponent', () => {
  let component: CreateUpdateTechnologyComponent;
  let fixture: ComponentFixture<CreateUpdateTechnologyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateTechnologyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateTechnologyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
