import { TestBed } from '@angular/core/testing';

import { TypeLoginService } from './type-login.service';

describe('TypeLoginService', () => {
  let service: TypeLoginService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TypeLoginService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
