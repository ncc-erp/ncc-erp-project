import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationFooterComponent } from './pagination-footer.component';

describe('PaginationFooterComponent', () => {
  let component: PaginationFooterComponent;
  let fixture: ComponentFixture<PaginationFooterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaginationFooterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaginationFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
