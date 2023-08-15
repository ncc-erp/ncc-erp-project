import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaoDoDetailComponent } from './sao-do-detail.component';

describe('SaoDoDetailComponent', () => {
  let component: SaoDoDetailComponent;
  let fixture: ComponentFixture<SaoDoDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaoDoDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaoDoDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
