import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ProjectDetailService extends BaseApiService {
  changeUrl() {
    return 'Project'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  isShowTabSummary = new BehaviorSubject(false);
  _isShowTabSummary = this.isShowTabSummary.asObservable();

  public getProjectSummary(id: number){
    return this.http.get<any>(this.rootUrl + `/GetProjectInfoCheckbyId?projectId=${id}`);
  }

  public setValueSummary(isShow: boolean){
    this.isShowTabSummary.next(isShow);
  }

}