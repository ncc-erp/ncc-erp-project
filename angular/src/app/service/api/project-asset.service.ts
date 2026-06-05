import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectAssetService extends BaseApiService {
  changeUrl() {
    return 'ProjectAsset';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public GetAllAssetsByProjectId(projectId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAllAssetsByProjectId?projectId=${projectId}`);
  }

  public Create(projectId: number, input: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/Create?projectId=${projectId}`, input);
  }

  public GetAllUserProjectAssetByProjectId(projectId: number, viewHistory: boolean = false): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAllUserProjectAssetByProjectId?projectId=${projectId}&viewHistory=${viewHistory}`);
  }

  public UpdateUserAsset(userId: number, projectAssetIds: number[]): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/UpdateUserAsset?userId=${userId}`, projectAssetIds);
  }

  public Edit(projectId: number, input: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + `/Edit?projectId=${projectId}`, input);
  }

  public Delete(projectAssetId: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + `/Delete?projectAssetId=${projectAssetId}`);
  }
}