import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PunishmentComponent } from './punishment.component';

describe('PunishmentComponent', () => {
  let component: PunishmentComponent;
  let fixture: ComponentFixture<PunishmentComponent>;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PunishmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PunishmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
