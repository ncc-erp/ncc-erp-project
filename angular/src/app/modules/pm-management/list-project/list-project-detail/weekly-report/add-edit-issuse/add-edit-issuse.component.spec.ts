import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditIssuseComponent } from './add-edit-issuse.component';

describe('AddEditIssuseComponent', () => {
  let component: AddEditIssuseComponent;
  let fixture: ComponentFixture<AddEditIssuseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditIssuseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditIssuseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
