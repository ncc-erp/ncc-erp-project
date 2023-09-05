import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PMReportProjectService extends BaseApiService {

  public projectHealth: string

  changeUrl() {
    return 'PMReportProject'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public getChangesDuringWeek(projectId: number, pmReportId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetchangedResourceByPmReport?projectId=${projectId}&pmReportId=${pmReportId}`);
  }
  public getChangesInFuture(projectId: number, pmReportId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetPlannedResourceByPmReport?projectId=${projectId}&pmReportId=${pmReportId}`);
  }
  public GetAllPmReportProjectForDropDown(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAllPmReportProjectForDropDown');
  }
  public GetAllByPmReport(pmReportId: number, projectType: any, health: any, sort: any, sortReview: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAllByPmReport?pmReportId=' + pmReportId+'&projectType='+ projectType + '&health=' + (health == 'ALL'? '': health) + '&sort=' + sort + '&sortReview=' + sortReview);
  }
  public sendReport(projectId: number, pmReportId: number, status: string): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/SendReport?projectId=${projectId}&pmReportId=${pmReportId}&status=${status}`, {});
  }
  public problemsOfTheWeekForReport(projectId: number, pmReportId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/ProblemsOfTheWeekForReport?projectId=${projectId}&pmReportId=${pmReportId}`);
  }
  public updateHealth(pmReportProjectId: any, projectHealth: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/UpdateHealth?pmReportProjectId=' + pmReportProjectId + '&projectHealth=' + projectHealth)
  }
  public GetAllByProject(projectId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAllByProject?projectId=${projectId}`);
  }
  public updateNote(note: string, pmReportProjectId: number): Observable<any> {
    // if (note) {
    //   note = JSON.stringify(note)
    // }
    let requestBody ={
      note:  note,
      id: pmReportProjectId
    }
    return this.http.put<any>(this.rootUrl + `/UpdateNote`, requestBody);
  }

  public updateAutomationNote(note: string, pmReportProjectId: number): Observable<any> {
    let requestBody ={
      note:  note,
      id: pmReportProjectId
    }
    return this.http.put<any>(this.rootUrl + `/UpdateAutomationNote`, requestBody);
  }

  public SetDoneIssue( id:any) {
    return this.http.get(this.rootUrl + `/SetDoneIssue?id=`+id)
  }

  public GetInfoProject(pmReportProjectId: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetInfoProject?pmReportProjectId=${pmReportProjectId}`);

  }
  public GetCurrentResourceOfProject(projectId:number){
    return this.http.get<any>(this.rootUrl + `/GetWorkingResourceOfProject?projectId=${projectId}`);

  }

  public GetTimesheetWorking(pmReportProjectId: number,startTime:any,endTime:any): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/GetWorkingTimeFromTimesheet?pmReportProjectId=${pmReportProjectId}&startTime=${startTime}&endTime=${endTime}`, {});
  }


  public CancelResourcePlan(projectUserId: number) {
    return this.http.delete(this.rootUrl + `/CancelResourcePlan?projectUserId=${projectUserId}`)
  }


  public ConfirmOutProject(input: any) {
    return this.http.post(this.rootUrl + `/ConfirmOutProject`, input)

  }
  public ConfirmJoinProject(projectUserId: number, startTime: any) {
    return this.http.get(this.rootUrl + `/ConfirmJoinProject?projectUserId=${projectUserId}&startTime=${startTime}`)
  }


  public EditProjectUserPlan( input:any) {
    return this.http.post(this.rootUrl + `/EditProjectUserPlan`,input)
  }

  public PlanNewResourceToProject( input:any) {
    return this.http.post(this.rootUrl + `/PlanEmployeeJoinProject`,input)
  }

  public reverseDelete(pmReportProjectId: number, { }): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/ReverseSeen?pmReportProjectId=${pmReportProjectId}`, {});
  }
  
  public checkNecessaryReview(pmReportProjectId: number, { }): Observable<any> {
    return this.http.post<any>(this.rootUrl + `/CheckNecessaryReview?pmReportProjectId=${pmReportProjectId}`, {});
  }

  // Chart api
  public GetTimesheetWeeklyChartOfProject(projectCode:any, startDate: string, endDate:string): Observable<any> {
    return this.http.get(this.configURI.timesheetURI +`api/services/app/Public/GetTimesheetWeeklyChartOfProject?projectCode=${projectCode}&startDate=${startDate}&endDate=${endDate}`)
  }
  public GetTimesheetWeeklyChartOfUserInProject(projectCode:string,emailAddress:any, startDate: string, endDate:string): Observable<any> {
    return this.http.get(this.configURI.timesheetURI + `api/services/app/Public/GetTimesheetWeeklyChartOfUserInProject?projectCode=${projectCode}&emailAddress=${emailAddress}&startDate=${startDate}&endDate=${endDate}`)
  }
  public GetTimesheetOfUserInProject(projectCode:string,emailAddress:any, startDate: string, endDate:string): Observable<any> {
    return this.http.get(this.configURI.timesheetURI +`api/services/app/Public/GetTimesheetOfUserInProject?projectCode=${projectCode}&emailAddress=${emailAddress}&startDate=${startDate}&endDate=${endDate}`)
  }
  public GetTimesheetOfUserInProjectNew(projectCode:string,emailAddress:any, startDate: string, endDate:string): Observable<any> {
    return this.http.get(this.configURI.timesheetURI +`api/services/app/Public/GetTimesheetOfUserInProjectNew?projectCode=${projectCode}&emailAddress=${emailAddress}&startDate=${startDate}&endDate=${endDate}`)
  }
  public GetUserInProjectFromTimesheet(projectCode:string,usersEmail: string[], startDate: string, endDate:string): Observable<any> {
    return this.http.post(this.configURI.timesheetURI +`api/services/app/Public/GetUserInProjectFromTimesheet?projectCode=${projectCode}&startDate=${startDate}&endDate=${endDate}`, usersEmail)
  }
  public GetTimesheetWeeklyChartOfUserGroupInProject(input:any): Observable<any> {
    return this.http.post(this.configURI.timesheetURI +`api/services/app/Public/GetTimesheetWeeklyChartOfUserGroupInProject`, input)
  }
  public GetEffortMonthlyChartOfUserGroupInProject(input:any): Observable<any> {
    return this.http.post(this.configURI.timesheetURI +`api/services/app/Public/GetEffortMonthlyChartOfUserGroupInProject`, input)
  }

  public GetEffortMonthlyChartProject(input:any): Observable<any> {
    return this.http.post(this.configURI.timesheetURI +`api/services/app/Public/GetEffortMonthlyChartProject`, input)
  }

 
}
