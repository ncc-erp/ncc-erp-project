import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportTailoringComponent } from './import-tailoring.component';

describe('ImportTailoringComponent', () => {
  let component: ImportTailoringComponent;
  let fixture: ComponentFixture<ImportTailoringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportTailoringComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportTailoringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
