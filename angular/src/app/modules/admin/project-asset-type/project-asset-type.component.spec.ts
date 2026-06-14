import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectAssetTypeComponent } from './project-asset-type.component';

describe('ProjectAssetTypeComponent', () => {
  let component: ProjectAssetTypeComponent;
  let fixture: ComponentFixture<ProjectAssetTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectAssetTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectAssetTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
