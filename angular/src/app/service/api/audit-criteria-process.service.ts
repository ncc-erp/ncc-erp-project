import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root',
})
export class AuditCriteriaProcessService extends BaseApiService {

  changeUrl() {
    return 'ProcessCriteria'
  }
  constructor(http: HttpClient) {
    super(http);
  }



  public getCriteriaActive():Observable<any> {
      return this.http.post<any>(this.rootUrl + '/GetAll',{isActive:true,isLeaf:true});
    }
  public searchCriteriaActive(searchText?:string):Observable<any> {
      return this.http.post<any>(this.rootUrl + `/GetAll`,{ isActive:true,IsLeaf:true,searchText });
    }

  public getAll(): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAll',{});
  }
  public search(searchText?: string): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAll', {searchText});
  }
  public create(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Create', item);
  }
  public getCriteriaById(id: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/Get?id=' + id);
  }
  public getForDropDown(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetForDropDown');
  }
  public update(item: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/Update', item);
  }
  public DeActive(id): Observable<any>{
    return this.http.post<any>(this.rootUrl + '/DeActive?Id=' + id, {});
  }
  public Active(id): Observable<any>{
    return this.http.post<any>(this.rootUrl + '/Active?Id=' + id, {});
  }
  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('Id', id)
    });
  }

  public GetAllProcessTailoringContain(id): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetAllProcessTailoringContain?id=' + id);
  }
  public RemoveCriteriaFromTailoring(id): Observable<any>{
    return this.http.delete<any>(this.rootUrl + `/RemoveCriteriaFromTailoring?Id=${id}`);
  }
  public ValidToDeleteListCriteria(id): Observable<any>{
    return this.http.post<any>(this.rootUrl + `/ValidToDeleteListCriteria?id=${id}`, []);
  }
  public ValidToDeleteLeafCriteria(id): Observable<any>{
    return this.http.post<any>(this.rootUrl + `/ValidToDeleteLeafCriteria?id=${id}`, {});
  }
  public ValidTailoringContain(id): Observable<any>{
    return this.http.post<any>(this.rootUrl + `/ValidTailoringContain?id=${id}`, {});
  }
  public ChangeApplicable(id): Observable<any>{
    return this.http.put<any>(this.rootUrl + `/ChangeApplicable?id=${id}`, {});
  }
}
