import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PmReportIssueService extends BaseApiService {

  changeUrl() {
    return 'PMReportProjectIssue'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public getProblemsOfTheWeek(projectId: number,pmReportId:number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/ProblemsOfTheWeek?projectId=${projectId}&pmReportId=${pmReportId}`);

  }
  public deleteReportIssue(reportId: any) {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('pmReportProjectIssueId', reportId)
    })
  }
  public createReportIssue(projectId: number, reportIssue:any): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/Create?projectId=${projectId}`, reportIssue);
  }

  public EditMeetingNote(input): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/EditMeetingNote`, input);
  }
}
