import { PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { PagedRequestDto } from './../../../shared/paged-listing-component-base';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BaseApiService } from './base-api.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SetupReviewerService extends BaseApiService {
  changeUrl() {
    return 'CheckPointUser';
  }
  constructor(http:HttpClient){
    super(http);
  }
  public getAllPagging(request: PagedRequestDto , id:any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAllPagging?phaseId='+id, request);
  }
  public generateReviewer(id: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GenerateReviewer?phaseId=' + id);
  }
  public getUserUnreview(id:any):Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetUserUnreview?phaseId='+id)
  }
  public updateReviewer(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Update', item);
}
  public createReviewer(isAdmin: boolean,item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Create?isAdmin='+isAdmin, item);
  }
  public Get(id: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/Get?Id=' + id);
  }
  public GetAllPhase():Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetAllPhase');
  }
  public getAllReviewForSelf(request: PagedRequestDto):Observable<any>{
    return this.http.post<any>(this.rootUrl + '/GetAllReviewForSelf', request);
  }
  public getAllReviewBySelf(request: PagedRequestDto):Observable<any>{
    return this.http.post<any>(this.rootUrl + '/GetAllReviewBySelf', request);
  }
  public getAllPagingSub(id:any,request: PagedRequestDto):Observable<any>{
    return this.http.post<any>(this.rootUrl+ `/GetAllPagingSub?phaseId=${id}`, request)
  }
}
