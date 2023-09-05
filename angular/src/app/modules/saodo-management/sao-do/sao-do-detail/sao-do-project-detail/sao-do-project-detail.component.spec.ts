import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaoDoProjectDetailComponent } from './sao-do-project-detail.component';

describe('SaoDoProjectDetailComponent', () => {
  let component: SaoDoProjectDetailComponent;
  let fixture: ComponentFixture<SaoDoProjectDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaoDoProjectDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaoDoProjectDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
