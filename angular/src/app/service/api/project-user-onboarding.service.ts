import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseApiService } from "./base-api.service";

import { Injectable } from "@angular/core";
import { IProjectHistoryUser, ResponseWrapper } from "../model/project.dto";

@Injectable({
  providedIn: "root",
})
export class ProjectUserOnboardingService extends BaseApiService {
  changeUrl() {
    return "ProjectUserOnboarding";
  }
  constructor(http: HttpClient) {
    super(http);
  }

  getOnboardingInfo(projectUserId: number): Observable<any> {
    return this.http.get<any>(
      this.rootUrl +
      `/GetOnboardingInfor?projectUserId=${projectUserId}`
    );
  }
  
  onboardingUser(input: any): Observable<void> {
    return this.http.post<void>(
      this.rootUrl + `/onboardingUser`,
      input
    );
  }

  forceDone(projectUserId: number): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/ForceDone?projectUserId=${projectUserId}`,
      {}
    );
  }

  remind(projectUserId: number): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/Remind?projectUserId=${projectUserId}`,
      {}
    );
  }

  GetAllOnboardHistory(request: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAllOnboardHistory', request);
  }

  updateOnboardHistoryNote(input: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/UpdateOnboardHistoryNote', input);
  }

  Delete(onboardHistoryId: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete?onboardHistoryId=' + onboardHistoryId);
  }
}
