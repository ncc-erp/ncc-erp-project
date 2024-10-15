import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvstatusComponent } from './cvstatus.component';

describe('CvstatusComponent', () => {
  let component: CvstatusComponent;
  let fixture: ComponentFixture<CvstatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvstatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvstatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
