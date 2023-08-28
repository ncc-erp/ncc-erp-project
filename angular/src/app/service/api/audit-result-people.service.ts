import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuditResultPeopleService extends BaseApiService {

  changeUrl() {
    return 'AuditResultPeople'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  

}
