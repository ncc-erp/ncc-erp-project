import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class TypeLoginService extends BaseApiService {
  changeUrl() {
    return 'TypeLogin';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public getAllPagging(request: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAllPagging', request);
  }

  public getAll(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAll');
  }

  public create(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Create', item);
  }

  public update(item: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/Update', item);
  }

  public delete(typeLoginId: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('typeLoginId', typeLoginId.toString())
    });
  }
}
