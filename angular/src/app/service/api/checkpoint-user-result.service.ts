import { PagedRequestDto } from './../../../shared/paged-listing-component-base';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BaseApiService } from './base-api.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CheckpointUserResultService extends BaseApiService{
  changeUrl() {
    return 'CheckpointUserResult'
  }

  constructor(http:HttpClient) {
    super(http);
  }
  public getAllUserResult(id:any):Observable<any>{
    return this.http.get<any>(this.rootUrl+ '/getAll?phaseId='+id);
  }
  public Done(id:any):Observable<any>{
    return this.http.post<any>(this.rootUrl+ `/Done?checkPointUserResultId=${id}`,id);
  }
  public GetAllTagNotSelect(id:any):Observable<any>{
    return this.http.get<any>(this.rootUrl+'/GetAllTagNotSelect?checkPointUserResultId='+id);
  }
  public EditMain(item:any):Observable<any>{
    return this.http.put<any>(this.rootUrl+'/EditMain',item);
  }
  public GetAllPagingMain(id:any,request: PagedRequestDto): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/GetAllPagingMain?phaseId=${id}`, request);
  }
}
