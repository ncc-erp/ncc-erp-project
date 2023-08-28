import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ClientService extends BaseApiService{

  changeUrl() {
    return 'Client'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  
  public deleteClient(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('clientId', id)
    })
  }
  public getAllClient(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/getAllClient');
  }
  public getIdClientByCodeNcc(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/getIdClientByCodeNcc');
  }
  public getAllPaymentDueBy(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetAllPaymentDueBy');
  }
  public getAllInvoiceDate(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetAllInvoiceDate');
  }
  
}
