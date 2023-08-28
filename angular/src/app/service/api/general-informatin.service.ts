
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {  Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';
@Injectable({
  providedIn: 'root'
})
export class GeneralInformationService extends BaseApiService {
  changeUrl() {
    return 'ProjectUserBill'
  }
  constructor(http: HttpClient) {
    super(http);
  }

  public getBillAccount(id: number){
    return this.http.post<any>(this.rootUrl + `/GetSubProjectBills`,{parentProjectId:id});
  }

}
