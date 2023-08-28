import { BaseApiService } from './base-api.service';

import { HttpClient, HttpParams } from '@angular/common/http';
import { PagedRequestDto } from './../../../shared/paged-listing-component-base';
import { Observable } from 'rxjs';


import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CriteriaCategoryService extends BaseApiService{
  changeUrl() {
    return 'CriteriaCategory'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public GetAllNoPagging(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAllNoPagging');
  }
//   public getAll(): Observable<any> {
//     return this.http.get<any>(this.rootUrl + '/GetAll');
// }

  
  
}
