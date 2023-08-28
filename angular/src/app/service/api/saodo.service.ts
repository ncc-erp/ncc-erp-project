import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SaodoService extends BaseApiService {

  changeUrl() {
    return 'AuditSession'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public deleteAuditSession(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('id', id)
  })
  
}
public getDetailById(id: any , searchText : string): Observable<any> {
  return this.http.get<any>(this.rootUrl + '/Get?id=' + id+"&searchText="+searchText);
}


}
