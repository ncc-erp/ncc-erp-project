import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BackupContributionComponent } from './backup-contribution.component';

describe('BackupContributionComponent', () => {
  let component: BackupContributionComponent;
  let fixture: ComponentFixture<BackupContributionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BackupContributionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BackupContributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
