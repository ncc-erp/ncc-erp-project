import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaloringProjectComponent } from './taloring-project.component';

describe('TaloringProjectComponent', () => {
  let component: TaloringProjectComponent;
  let fixture: ComponentFixture<TaloringProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaloringProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaloringProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
