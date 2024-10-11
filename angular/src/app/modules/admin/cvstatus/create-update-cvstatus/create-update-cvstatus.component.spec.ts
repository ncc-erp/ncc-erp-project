import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateCvstatusComponent } from './create-update-cvstatus.component';

describe('CreateUpdateCvstatusComponent', () => {
  let component: CreateUpdateCvstatusComponent;
  let fixture: ComponentFixture<CreateUpdateCvstatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateCvstatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateCvstatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
