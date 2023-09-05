import { RetroReviewInternHistoriesDto } from './../model/resource-plan.dto';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedRequestDto } from '../../../shared/paged-listing-component-base';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ResourceManagerService extends BaseApiService{

  changeUrl() {
    return 'Resource';
  }
  constructor(http: HttpClient) {
    super(http);
  }



  public GetVendorResource(
    request: PagedRequestDto
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/GetVendorResource',
      request
    );
  }

  public GetAllPoolResource(
    request: any, skillId?: any
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/GetAllPoolResource',
      request
    );
  }
  public GetAllResource(
    request: PagedRequestDto
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/GetAllResource',
      request
    );
  }

  public CancelPoolResourcePlan(
    id: number
  ): Observable<any> {
    return this.http.delete<any>(
      this.rootUrl + '/CancelPoolResourcePlan?projectUserId=' + id,

    );
  }
  public CancelAllResourcePlan(
    id: number
  ): Observable<any> {
    return this.http.delete<any>(
      this.rootUrl + '/CancelAllResourcePlan?projectUserId=' + id,

    );
  }  public CancelVendorResourcPlan(
    id: number
  ): Observable<any> {
    return this.http.delete<any>(
      this.rootUrl + '/CancelVendorResourcPlan?projectUserId=' + id,

    );
  }

  public updatePoolNote(
    poolNote: any
  ): Observable<any> {
    return this.http.put<any>(
      this.rootUrl + '/UpdateUserPoolNote', poolNote

    );
  }

  public ConfirmOutProject(input: any) {
    return this.http.post(this.rootUrl + `/ConfirmOutProject`, input)

  }
  // public ConfirmJoinProject(projectUserId: number, startTime: any) {
  //   return this.http.get(this.rootUrl + `/ConfirmJoinProject?projectUserId=${projectUserId}&startTime=${startTime}`)
  // }
  public ConfirmJoinProjectFromTabPool(projectUserId: number, startTime: any) {
    return this.http.get(this.rootUrl + `/ConfirmJoinProjectFromTabPool?projectUserId=${projectUserId}&startTime=${startTime}`)
  }
  public ConfirmJoinProjectFromTabAllResource(projectUserId: number, startTime: any) {
    return this.http.get(this.rootUrl + `/ConfirmJoinProjectFromTabAllResource?projectUserId=${projectUserId}&startTime=${startTime}`)
  }
  public ConfirmJoinProjectFromTabVendor(projectUserId: number, startTime: any) {
    return this.http.get(this.rootUrl + `/ConfirmJoinProjectFromTabVendor?projectUserId=${projectUserId}&startTime=${startTime}`)
  }

  public EditProjectUserPlan( input:any) {
    return this.http.post(this.rootUrl + `/EditProjectUserPlan`,input)
  }

  public PlanNewResourceToProject( input:any) {
    return this.http.post(this.rootUrl + `/PlanEmployeeJoinProject`,input)
  }

  public AddUserToTempProject( input:any) {
    return this.http.post(this.rootUrl + `/AddUserFromPoolToTempProject`,input)
  }
  public planUser(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/PlanEmployeeJoinOrOutProject', item);
  }
  public GetTimesheetOfRetroReviewInternHistories(input:RetroReviewInternHistoriesDto): Observable<any> {
    return this.http.post(this.rootUrl + `/GetRetroReviewInternHistories`, input)
  }
}
