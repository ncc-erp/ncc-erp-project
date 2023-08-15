import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FutureResourceComponent } from './future-resource.component';

describe('FutureResourceComponent', () => {
  let component: FutureResourceComponent;
  let fixture: ComponentFixture<FutureResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FutureResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FutureResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
