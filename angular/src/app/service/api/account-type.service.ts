import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class AccountTypeService extends BaseApiService {
  changeUrl() {
    return 'AccountType';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public deleteAccountType(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('accountTypeId', id)
    });
  }
}
