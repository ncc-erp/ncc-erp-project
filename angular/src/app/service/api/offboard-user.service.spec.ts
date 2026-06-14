import { TestBed } from '@angular/core/testing';

import { OffboardUserService } from './offboard-user.service';

describe('OffboardUserService', () => {
  let service: OffboardUserService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OffboardUserService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
