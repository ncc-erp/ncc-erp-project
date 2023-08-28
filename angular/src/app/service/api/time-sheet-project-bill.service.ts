import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class TimeSheetProjectBillService extends BaseApiService {
  changeUrl() {
    return 'TimeSheetProjectBill'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public getProjectBill(projectId: any, timesheetId: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAll?timesheetId=${timesheetId}&projectId=${projectId}`)
  }
  public updateProjectBill(item: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/Update', item);
  }
  public updateListProjectBill(item: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/Update', item);
  }
  public createProjectBill(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Create', item);
  }

  public UpdateFromProjectUserBill(projectId: any, timesheetId: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + `/UpdateFromProjectUserBill?projectId=${projectId}&timesheetId=${timesheetId}`, {});
  }
  public getAllUserUnused(projectId: any, timesheetId: any, isEdited: boolean): Observable<any> {
    return this.http.get(this.rootUrl + `/GetUserForTimesheetProjectBill?projectId=${projectId}&timesheetId=${timesheetId}&isEdited=${isEdited}`);
  }
  public updateTS(data: any): Observable<any>{
    return this.http.post<any>(this.rootUrl + '/updateTSOfPM', data);
  }
  public removeAccountTS(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/RemoveAccountTS', {
        params: new HttpParams().set('id', id)
    })
  }
}
