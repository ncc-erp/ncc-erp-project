import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateChecklistItemComponent } from './create-checklist-item.component';

describe('CreateChecklistItemComponent', () => {
  let component: CreateChecklistItemComponent;
  let fixture: ComponentFixture<CreateChecklistItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateChecklistItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateChecklistItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
