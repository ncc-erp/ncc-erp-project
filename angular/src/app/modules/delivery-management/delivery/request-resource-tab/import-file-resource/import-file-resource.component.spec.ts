import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportFileResourceComponent } from './import-file-resource.component';

describe('ImportFileResourceComponent', () => {
  let component: ImportFileResourceComponent;
  let fixture: ComponentFixture<ImportFileResourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportFileResourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportFileResourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
