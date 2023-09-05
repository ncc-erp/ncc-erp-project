import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewQaNoteComponent } from './view-qa-note.component';

describe('ViewQaNoteComponent', () => {
  let component: ViewQaNoteComponent;
  let fixture: ComponentFixture<ViewQaNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewQaNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewQaNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
