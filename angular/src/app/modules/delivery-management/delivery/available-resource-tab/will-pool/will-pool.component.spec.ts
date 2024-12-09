import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WillPoolComponent } from './will-pool.component';

describe('WillPoolComponent', () => {
  let component: WillPoolComponent;
  let fixture: ComponentFixture<WillPoolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WillPoolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WillPoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
