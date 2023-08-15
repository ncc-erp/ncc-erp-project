import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class TechnologyService extends BaseApiService{

  changeUrl() {
    return 'Technology'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  
  public deleteTechnology(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('technologyId', id)
    })
  }
  public getAllTechnology(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/getAllTechnology');
  }
}
