import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ConfigItService extends BaseApiService {

  changeUrl() {
    return 'ConfigIT';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  getAll(searchText?: string): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAll', {
      params: searchText ? { searchText } : {}
    });
  }

  getAllUser(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAllUser');
  }

  create(userId: number): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Create', null, {
      params: new HttpParams().set('userId', userId.toString())
    });
  }

  updateConfig(id: number, userId: number): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/Update', null, {
      params: { id: id.toString(), userId: userId.toString() }
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: { id: id.toString() }
    });
  }
}
