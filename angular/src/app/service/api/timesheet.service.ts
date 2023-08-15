import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PagedRequestDto } from '@shared/paged-listing-component-base';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';


@Injectable({
  providedIn: 'root'
})
export class TimesheetService extends BaseApiService{
  changeUrl() {
    return 'Timesheet';
  }
  constructor(http:HttpClient) {
    super(http)
  }
  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('timesheetId', id)
    })
  }
  public ReverseActive(id:number): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/ReverseActive?id=${id}`, {});
  }

  public ForceDelete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/ForceDelete?', {
      params: new HttpParams().set('timesheetId', id)
  })
  }


}
