import { Observable } from 'rxjs';
import { PagedRequestDto } from './../../../shared/paged-listing-component-base';
import { HttpClient } from '@angular/common/http';
import { BaseApiService } from './base-api.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService extends BaseApiService{
  changeUrl() {
    return 'Currency'
  }

  constructor(public http:HttpClient) {super(http) }
  deleteCurrency(id:any):Observable<any>{
    return this.http.delete<any>(this.rootUrl + "/Delete?currencyId="+id);
  }


}
