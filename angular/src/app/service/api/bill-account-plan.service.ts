import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
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
  public GetAllPlanningBillInfo(
    request: PagedRequestDto
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/GetAllPlanningBillInfo',
      request
    );
  }
  public GetAllBillInfo(
    request: PagedRequestDto
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/GetAllBillInfo',
      request
    );
  }
  public GetAllProjectUserBill(): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAllProjectUserBill`);
  }
}



