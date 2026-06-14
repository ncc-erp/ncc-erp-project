import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEditProjectAssetTypeComponent } from './create-edit-project-asset-type.component';

describe('CreateEditProjectAssetTypeComponent', () => {
  let component: CreateEditProjectAssetTypeComponent;
  let fixture: ComponentFixture<CreateEditProjectAssetTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateEditProjectAssetTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateEditProjectAssetTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
