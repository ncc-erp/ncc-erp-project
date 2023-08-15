import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdatePositionComponent } from './create-update-position.component';

describe('CreateUpdatePositionComponent', () => {
  let component: CreateUpdatePositionComponent;
  let fixture: ComponentFixture<CreateUpdatePositionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdatePositionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdatePositionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
