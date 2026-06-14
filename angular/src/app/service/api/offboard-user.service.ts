import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class OffboardUserService extends BaseApiService {
  changeUrl() {
    return 'OffboardUser';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public GetAllOffboardHistory(request: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAllOffboardHistory', request);
  }

  public UpdateOffboardStatus(input: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/UpdateOffboardStatus', input);
  }

  public GetOffboardChecklist(offboardHistoryId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetOffboardChecklist?offboardHistoryId=' + offboardHistoryId);
  }

  public SaveOffboardChecklist(input: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/SaveOffboardChecklist', input);
  }

  public MoveToIT(offboardHistoryId: number): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/MoveToIT?offboardHistoryId=' + offboardHistoryId, {});
  }

  public MoveToComplete(offboardHistoryId: number): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/MoveToComplete?offboardHistoryId=' + offboardHistoryId, {});
  }

  public CheckOffboardHistory(projectUserId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/CheckOffboardHistory?projectUserId=' + projectUserId);
  }
}
