import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectCriteriaResultDto } from '../model/project-criteria-result.dto';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectCriteriaResultService extends BaseApiService {

  changeUrl() {
    return 'ProjectCriteriaResult'
  }
  constructor(http: HttpClient) {
    super(http);
  }

  public getAllCriteriaResult(projectId: number, pmReportId: number): Observable<any> {
    return this.http.get(this.rootUrl + `/GetAll?projectId=${projectId}&pmReportId=${pmReportId}`)
  }
  public updateAllCriteriaResult(listCriteriaResult: ProjectCriteriaResultDto[]): Observable<any>{
    return this.http.post(this.rootUrl + `/UpdateAllCriteriaResult`, listCriteriaResult);
  }
}
