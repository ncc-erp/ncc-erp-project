import { TestBed } from '@angular/core/testing';

import { ProjectAssetService } from './project-asset.service';

describe('ProjectAssetService', () => {
  let service: ProjectAssetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectAssetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
