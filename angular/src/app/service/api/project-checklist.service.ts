import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectChecklistService extends BaseApiService{

  changeUrl() {
    return 'ProjectChecklist'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public GetCheckListItemByProject(projectId:any,auditSessionId?:any): Observable<any> {
    if(auditSessionId){
      return this.http.get<any>(this.rootUrl + '/GetCheckListItemByProject?projectId='+projectId+'&auditSessionId='+auditSessionId);
    }
    else{
      return this.http.get<any>(this.rootUrl + '/GetCheckListItemByProject?projectId='+projectId);
    }
   
  }
  public addCheckListItemByProject(projectId:any,item:any):Observable<any>{
    return this.http.post<any>(this.rootUrl+'/AddCheckListItemByProject?projectId='+projectId,item);
  }
  public DeleteByProject(projectId: any , checkListItemId : any){
    return this.http.delete<any>(this.rootUrl + `/Delete?ProjectId=${projectId}&CheckListItemId=${checkListItemId}`)
  }
 
}