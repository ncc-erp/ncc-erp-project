import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListProjectGeneralComponent } from './list-project-general.component';

describe('ListProjectGeneralComponent', () => {
  let component: ListProjectGeneralComponent;
  let fixture: ComponentFixture<ListProjectGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListProjectGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListProjectGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
