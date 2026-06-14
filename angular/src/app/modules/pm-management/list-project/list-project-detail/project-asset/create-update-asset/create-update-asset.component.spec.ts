import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateAssetComponent } from './create-update-asset.component';

describe('CreateUpdateAssetComponent', () => {
  let component: CreateUpdateAssetComponent;
  let fixture: ComponentFixture<CreateUpdateAssetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateAssetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
