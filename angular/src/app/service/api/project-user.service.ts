import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseApiService } from "./base-api.service";

import { Injectable } from "@angular/core";
import { IProjectHistoryUser, ResponseWrapper } from "../model/project.dto";

@Injectable({
  providedIn: "root",
})
export class ProjectUserService extends BaseApiService {
  changeUrl() {
    return "ProjectUser";
  }
  constructor(http: HttpClient) {
    super(http);
  }
  getAllProjectUser(id: number, viewHistory?: boolean): Observable<any> {
    return this.http.get<any>(
      this.rootUrl +
      `/GetAllWorkingUserByProject?projectId=${id}&viewHistory=${viewHistory}`
    );
  }

  GetAllPlannedUserByProject(projectId: number): Observable<any> {
    return this.http.get<any>(
      this.rootUrl +
      `/GetAllPlannedUserByProject?projectId=${projectId}`
    );
  }
  getAllProjectUserInProject(id: number): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + "/GetAllProjectUserInProject?projectId=" + id
    );
  }
  GetAllWorkingProjectByUserId(userId: number): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + "/GetAllWorkingProjectByUserId?userId=" + userId
    );
  }
  removeProjectUser(userId: number): Observable<any> {
    return this.http.delete<any>(
      this.rootUrl + `/Delete?projectUserId=${userId}`
    );
  }

  AddUserToProject(user): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/AddUserToProject`, user
    );
  }
  AddUserToOutSourcingProject(user): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/AddUserToOutSourcingProject`, user
    );
  }
  AddUserToProductProject(user): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/AddUserToProductProject`, user
    );
  }
  AddUserToTrainingProject(user): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/AddUserToTrainingProject`, user
    );
  }
  UpdateCurrentResourceDetail(user): Observable<any> {
    return this.http.put<any>(
      this.rootUrl + `/UpdateCurrentResourceDetail`, user
    );
  }

  getProjectHistoryByUser(
    userId: number
  ): Observable<ResponseWrapper<IProjectHistoryUser[]>> {
    return this.http.get<ResponseWrapper<IProjectHistoryUser[]>>(
      this.rootUrl + `/GetProjectHistoryByUser?UserId=${userId}`
    );
  }


  ReleaseUserToPool(input): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/ReleaseUser`, input
    );
  }


  CancelResourcePlan(id): Observable<any> {
    return this.http.delete<any>(
      this.rootUrl + `/CancelResourcePlan?projectUserId=${id}`,
    );
  }

  EditProjectUserPlan(projectUser): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/EditProjectUserPlan`, projectUser
    );
  }
  PlanNewResourceToProject(projectUser): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/PlanNewResourceToProject`, projectUser
    );
  }

  ConfirmOutProject(projectUser): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/ConfirmOutProject`, projectUser
    );
  }

  ConfirmJoinProjectOutsourcing(projectUserId, startTime): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + `/ConfirmJoinProjectOutsourcing?projectUserId=${projectUserId}&startTime=${startTime}`,
    );
  }
  ConfirmJoinProjectProduct(projectUserId, startTime): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + `/ConfirmJoinProjectProduct?projectUserId=${projectUserId}&startTime=${startTime}`,
    );
  }
  ConfirmJoinProjectTraining(projectUserId, startTime): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + `/ConfirmJoinProjectTraining?projectUserId=${projectUserId}&startTime=${startTime}`,
    );
  }
  EditCurentResourceNote(id, note): Observable<any>{
    return this.http.put<any>(this.rootUrl + `/EditCurentResourceNote?id=${id}&note=${note}`, null);
  }
}
