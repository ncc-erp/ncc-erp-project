import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
    providedIn: 'root'
  })

export class PmReportRiskService extends BaseApiService {

  changeUrl() {
    return 'PMReportProjectRisk'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public getRiskOfTheWeek(projectId: number,pmReportId:number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/RisksOfTheWeek?projectId=${projectId}&pmReportId=${pmReportId}`);

  }
  public deleteReportRisk(reportId: any) {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('pmReportProjectIssueId', reportId)
    })
  }
  public createReportRisk(projectId: number, reportRisk:any): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/Create?projectId=${projectId}`, reportRisk);
  }

  public UpdateReportRisk(input): Observable<any> {
    return this.http.put<any>(this.rootUrl + `/Update`, input);
  }
}
