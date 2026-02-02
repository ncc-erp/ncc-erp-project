import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PMReportProjectContributionService extends BaseApiService {
  changeUrl() {
    return 'PMReportProjectContribution'
  }
  constructor(http: HttpClient) {
    super(http);
  }

  public UpdateWeeklyHistory(input:any): Observable<any> {
    console.log(input);
    console.log(this.rootUrl +`CreateOrUpdate`);
    return this.http.post(this.rootUrl +`/CreateOrUpdate`, input)
  }
}