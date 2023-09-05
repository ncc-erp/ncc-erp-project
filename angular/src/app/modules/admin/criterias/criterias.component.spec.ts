import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CriteriasComponent } from './criterias.component';

describe('CriteriasComponent', () => {
  let component: CriteriasComponent;
  let fixture: ComponentFixture<CriteriasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CriteriasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CriteriasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
