import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { PagedRequestDto } from '@shared/paged-listing-component-base';

@Injectable({
  providedIn: 'root'
})

export class PlanningBillInfoService extends BaseApiService {
  changeUrl() {
    return 'ProjectUserBill'
  }
  
  constructor(http: HttpClient) {
    super(http);
  }
  public GetAllBillInfo(request: PagedRequestDto): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAllBillInfo',request);
  }

  public GetAllProjectUserBill(): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAllProjectUserBill`);
  }

  public GetAllProjectClientBill(): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAllProjectClientBill`);
  }

  public UpdateBillNote(billNote: any): Observable<any> {
    return this.http.put<any>(
      this.rootUrl + '/UpdateBillNote', billNote
    );
  }
  public Get(userId,projectId):Observable<any>{
    return this.http.get<any>(this.rootUrl + `/Get?userId=${userId}&projectId=${projectId}`)
    }
  public GetAllResource(showVendor?: boolean): Observable<any> {
    let params = new HttpParams();
    if (showVendor !== undefined) {
      params = params.set('showVendor', String(showVendor));
    }
    return this.http.get<any>(this.rootUrl + '/GetAllResource', { params });
  }
    public RemoveLinkedResource(input): Observable<any> {
        return this.http.post<any>(this.rootUrl + '/RemoveLinkedResource', input);
    }
    public GetLinkResources(projectUserBillId): Observable<any> {
        return this.http.get<any>(this.rootUrl + `/GetLinkResources?projectUserBillId=${projectUserBillId}`);
    }
}



