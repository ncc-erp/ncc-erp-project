import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectAssetTypeService extends BaseApiService {
  changeUrl() {
    return 'ProjectAssetType';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public getAll(name?: string) {
    let params = new HttpParams();
    if (name) {
      params = params.set('name', name);
    }

    return this.http.get<any>(this.rootUrl + '/GetAll', { params });
  }

  public create(item: any) {
    return this.http.post<any>(this.rootUrl + '/Create', item);
  }

  public update(item: any) {
    return this.http.put<any>(this.rootUrl + '/Update', item);
  }

  public delete(id: number) {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('id', id.toString())
    });
  }
}
