import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BranchService extends BaseApiService{

  changeUrl() {
    return 'Branch'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  
  public deleteBranch(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('branchId', id)
    })
  }
  public getAllBranch(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/getAllBranch');
  }
  public getAllNotPagging(): Observable<any> {
    return this.http.get(this.rootUrl + "/GetAllNotPagging");
  }
}
