import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AuditResultService extends BaseApiService {

  changeUrl() {
    return 'AuditResult'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public getById(id: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetNote?id=' + id);
  }
  public updateNote(id: any, note:any): Observable<any> {
    let requestBody = {
      note: note,
      id: id
    }
    return this.http.put<any>(this.rootUrl + `/UpdateNote`,requestBody);
}
  

}
