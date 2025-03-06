import { BaseApiService } from '../api/base-api.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AppConsts } from './../../../shared/AppConsts';

@Injectable({
  providedIn: 'root'
})
export class MezonLoginService extends BaseApiService {

  changeUrl() {
    return 'TokenAuth';
  }

  constructor(
    http: HttpClient
  ) {
    super(http);
  }

  name() {
    return 'TokenAuth';
  }

  redirectToOAuth(): void {
    window.location.href = this.baseUrl + '/api/TokenAuth/MezonRedirect';
  }

  mezonAuthenticate(mezonToken: string): Observable<any> {
    const tokenPayload = { Token: mezonToken };
    return this.http.post(AppConsts.remoteServiceBaseUrl + '/api/TokenAuth/MezonAuthenticate', tokenPayload, {
      headers: { 'Content-Type': 'application/json' }
    });
  }  
}
