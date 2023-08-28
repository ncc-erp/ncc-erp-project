import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PositionService extends BaseApiService{

  changeUrl() {
    return 'Position'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  
  public getAllNotPagging(): Observable<any> {
    return this.http.get(this.rootUrl + "/GetAllNotPagging");
  }

  public deletePosition(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('positionId', id)
    })
  }
}
