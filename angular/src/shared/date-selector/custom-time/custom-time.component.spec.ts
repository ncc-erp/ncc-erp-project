import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomTimeComponent } from './custom-time.component';

describe('CustomTimeComponent', () => {
  let component: CustomTimeComponent;
  let fixture: ComponentFixture<CustomTimeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomTimeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomTimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
