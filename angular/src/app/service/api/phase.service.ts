import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseApiService } from './base-api.service';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PhaseService extends BaseApiService {
  changeUrl() {
    return 'Phase'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public Active(id:number): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/Active?phaseId='+id, {});
  }
  public DeActive(id:number): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/DeActive?phaseId='+id, {});
  }
  public Done(id:number):Observable<any>{
    return this.http.put<any>(this.rootUrl + '/Done?phaseId='+id,{});
  }
  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('phaseId', id)
  })
  
  }
  public getParent(year:number): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetAll?year='+year);

  }
  getAllPhase(year:number): Observable<any>{
    return this.http.get<any>(this.rootUrl +'/GetAllPhase?year='+year);
  }

 
  
}
