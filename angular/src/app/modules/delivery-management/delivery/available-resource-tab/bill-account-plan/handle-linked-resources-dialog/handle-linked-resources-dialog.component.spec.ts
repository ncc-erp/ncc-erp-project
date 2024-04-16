import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HandleLinkedResourcesDialogComponent } from './handle-linked-resources-dialog.component';

describe('HandleLinkedResourcesDialogComponent', () => {
  let component: HandleLinkedResourcesDialogComponent;
  let fixture: ComponentFixture<HandleLinkedResourcesDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HandleLinkedResourcesDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HandleLinkedResourcesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
