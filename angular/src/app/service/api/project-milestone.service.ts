import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';
@Injectable({
  providedIn: 'root'
})
export class ProjectMilestoneService extends BaseApiService {
  changeUrl() {
    return 'ProjectMilestone'
  }
  constructor(http: HttpClient) {
    super(http);
 
  }

}