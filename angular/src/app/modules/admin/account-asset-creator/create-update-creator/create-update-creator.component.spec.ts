import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateCreatorComponent } from './create-update-creator.component';

describe('CreateUpdateCreatorComponent', () => {
  let component: CreateUpdateCreatorComponent;
  let fixture: ComponentFixture<CreateUpdateCreatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateCreatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateCreatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
