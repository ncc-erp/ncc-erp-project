import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewGuilineComponent } from './view-guiline.component';

describe('ViewGuilineComponent', () => {
  let component: ViewGuilineComponent;
  let fixture: ComponentFixture<ViewGuilineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewGuilineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewGuilineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
