import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OffboardHistoryTabComponent } from './offboard-history-tab.component';

describe('OffboardHistoryTabComponent', () => {
  let component: OffboardHistoryTabComponent;
  let fixture: ComponentFixture<OffboardHistoryTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OffboardHistoryTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OffboardHistoryTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
