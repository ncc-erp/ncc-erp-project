import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProjectResourceRequestService extends BaseApiService {

  changeUrl() {
    return 'ResourceRequest'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  getAllResourceRequest(id:number):Observable<any>{
    return this.http.get<any>(this.rootUrl + `/GetAllByProject?projectId=${id}`);
  }
  deleteProjectRequest(id: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + `/Delete?resourceRequestId=${id}`)
  }
  public rejectRequest(id:number):Observable<any>{
    return this.http.get<any>(this.rootUrl + `/RejectUser?projectUserId=${id}`);
  }
  public approveRequest(projectUser:any): Observable<any>{
    return this.http.post<any>(this.rootUrl + `/ApproveUser`, projectUser);
  }
  public getProjectForDM(projectId:any,pmReportId:any){
    return this.http.get<any>(this.rootUrl+ '/GetProjectForDM?projectId='+projectId+'&pmReportId='+pmReportId);
  } 
}
