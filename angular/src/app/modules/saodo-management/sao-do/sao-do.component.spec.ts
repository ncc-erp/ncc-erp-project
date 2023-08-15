import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaoDoComponent } from './sao-do.component';

describe('SaoDoComponent', () => {
  let component: SaoDoComponent;
  let fixture: ComponentFixture<SaoDoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaoDoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaoDoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
