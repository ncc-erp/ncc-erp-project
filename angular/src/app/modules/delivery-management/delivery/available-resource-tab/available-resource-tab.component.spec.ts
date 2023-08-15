import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableResourceTabComponent } from './available-resource-tab.component';

describe('AvailableResourceTabComponent', () => {
  let component: AvailableResourceTabComponent;
  let fixture: ComponentFixture<AvailableResourceTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AvailableResourceTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AvailableResourceTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
