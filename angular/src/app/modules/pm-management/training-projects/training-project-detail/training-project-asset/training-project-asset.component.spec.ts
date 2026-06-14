import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProjectAssetComponent } from './training-project-asset.component';

describe('TrainingProjectAssetComponent', () => {
  let component: TrainingProjectAssetComponent;
  let fixture: ComponentFixture<TrainingProjectAssetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingProjectAssetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingProjectAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
