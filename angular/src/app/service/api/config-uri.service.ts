import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ConfigUriService {

  constructor(private http: HttpClient) {
  }
  public GetConfigUri(baseURI): Observable<any> {
    return this.http.get<any>(baseURI + '/api/services/app/Public/GetConfigUri');
  }
}
