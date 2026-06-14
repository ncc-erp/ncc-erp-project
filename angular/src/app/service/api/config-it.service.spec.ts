import { TestBed } from '@angular/core/testing';

import { ConfigItService } from './config-it.service';

describe('ConfigItService', () => {
  let service: ConfigItService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfigItService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
