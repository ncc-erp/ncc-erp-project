import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CvstatusService extends BaseApiService {
  changeUrl() {
    return 'CvStatus'
  }

  constructor(http: HttpClient) {
    super(http);
  }
}
