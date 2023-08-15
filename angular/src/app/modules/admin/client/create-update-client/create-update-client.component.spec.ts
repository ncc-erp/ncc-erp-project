import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateClientComponent } from './create-update-client.component';

describe('CreateUpdateClientComponent', () => {
  let component: CreateUpdateClientComponent;
  let fixture: ComponentFixture<CreateUpdateClientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateClientComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
