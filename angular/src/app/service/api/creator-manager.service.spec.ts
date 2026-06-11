import { TestBed } from '@angular/core/testing';

import { CreatorManagerService } from './creator-manager.service';

describe('CreatorManagerService', () => {
  let service: CreatorManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreatorManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
