import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListProjectDetailComponent } from './list-project-detail.component';

describe('ListProjectDetailComponent', () => {
  let component: ListProjectDetailComponent;
  let fixture: ComponentFixture<ListProjectDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListProjectDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListProjectDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
