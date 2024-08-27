import { ApiResponse,PagedRequestDto } from './../../../shared/paged-listing-component-base';
import { HttpClient, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError, Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';
import { AppConsts } from '@shared/AppConsts';
import { ResourceRequestCVDto } from '@app/service/model/resource-requestCV.dto';
@Injectable({
  providedIn: 'root',
})
export class DeliveryResourceRequestService extends BaseApiService {
  changeUrl() {
    return 'ResourceRequest';
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public getResourceRequestDetail(id: any): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + '/ResourceRequestDetail?resourceRequestId=' + id
    );
  }

    // Api resourceRequestCV
    public getResouceRequestCV(resourceRequestId: number): Observable<ApiResponse<ResourceRequestCVDto[]>>{
      return this.http.get<any>(
           `${this.rootUrl}/GetResourceRequestCV?resourceRequestId=${resourceRequestId}`
      ).pipe();
    }
    public getResourceRequestCVById(resourceRequestCVId: number):Observable<ApiResponse<ResourceRequestCVDto>>{
      return this.http.get<any>(
        `${this.rootUrl}/GetResourceRequestCVById?resourceRequestCVId=${resourceRequestCVId}`
      );
    }
  public updateResourceRequestCV(resourceRequestCV: any):Observable<any> {
    
      return this.http.put<any>(this.rootUrl + '/UpdateResourceRequestCV',resourceRequestCV);
    }  
    public deleteResourceRequestCV( resourceRequestCVId :number ){
      return this.http.delete<any>(
        this.rootUrl +
        `/DeleteResourceRequestCV?resourceRequestCVId=${resourceRequestCVId}`
      );
    }
   public addCV(resourceRequestCV : any):Observable<any>{
       return this.http.post<any>(this.rootUrl+'/AddCV',resourceRequestCV);
   }
   public UploadCVPath(linkCv ,id): Observable<any>{
    const formData = new FormData();
    formData.append('File', linkCv);
    formData.append('ResourceRequestCVId', id);
  
    const uploadReq = new HttpRequest(
      'POST', AppConsts.remoteServiceBaseUrl + '/api/services/app/ResourceRequest/UploadCVPathResourceRequestCV', formData,
      {
        reportProgress: true
      }
    );
    return this.http.request(uploadReq);
  }

  public searchAvailableUserForRequest(
    item: any,
    request: PagedRequestDto
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/SearchAvailableUserForRequest?startDate=' + item,
      request
    );
  }
  public AddUserToRequest(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/AddUserToRequest', item);
  }
  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('resourceRequestId', id),
    });
  }
  public getAvailableResource(
    request: PagedRequestDto,
    skillId?: any
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/AvailableResource?skillId=' + skillId,
      request
    );
  }

  public availableResourceFuture(request: PagedRequestDto): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + '/AvailableResourceFuture',
      request
    );
  }
  public createSkill(skill): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/CreateSkill', skill);
  }
  public deleteSkill(resourceRequestSkillId: any): Observable<any> {
    return this.http.delete<any>(
      this.rootUrl +
      `/DeleteSkill?resourceRequestSkillId=${resourceRequestSkillId}`
    );
  }

  public GetSkillDetail(id: any): Observable<any> {
    return this.http.get<any>(
      this.rootUrl + '/GetSkillDetail?resourceRequestId=' + id
    );
  }
  public getResourcePaging(
    request: PagedRequestDto,
    option: string
  ): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/GetAllPaging`,
      request
    );
  }

  public async getPlanResource(projecUserId, id) {
    return await this.http.get<any>(this.rootUrl + '/GetResourceRequestPlan?projectUserId=' + projecUserId + '&resourceRequestId=' + id).toPromise()
  }

  public getPlanResourceUser(): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetResourceRequestPlanUser', '')
  }

  public createPlanUser(data: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/CreateResourceRequestPlan', data)
  }

  public updatePlanUser(data: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/UpdateResourceRequestPlan', data)
  }

    public RemoveResourceRequestPlan(id: number): Observable<any> {
        return this.http.delete<any>(this.rootUrl + '/RemoveResourceRequestPlan?requestId=' + id, {})
  }

  public deletePlanUser(id: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/DeleteResourceRequestPlan?requestId=' + id)
  }

  public UpdateBillInfoPlan(input: any) {
    return this.http.post(this.rootUrl + `/UpdateBillInfoTemp`, input)
  }

  public CreateBillInfoPlan(input: any) {
    return this.http.post(this.rootUrl + `/CreateBillInfoPlan`, input)
  }

  public cancelResourceRequest(id: number): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/CancelRequest?requestId=' + id, {});
  }

  public activeResourceRequest(id: number): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/ActiveRequest?requestId=' + id, {});
  }

  public getSkills(): Observable<any> {
    return this.http.get<any>(this.baseUrl + '/api/services/app/Skill/GetAll');
  }

  public getLevels(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetRequestLevels');
  }

  public getPriorities(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetPriorities');
  }

  public getStatuses(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetStatuses');
  }

  public getProjectUserRoles(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetProjectUserRoles');
  }

  public getResourceRequestById(id: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetById?requestId=' + id)
  }

  public getTrainingLevels(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetTrainingRequestLevels');
  }

  public updateNote(data: any, type: string): Observable<any> {
    if (type == 'Description') {
      return this.http.post<any>(this.rootUrl + '/Description', data)
    }
    else {
      return this.http.post<any>(this.rootUrl + '/Note', data)
    }
  }
  public setDoneRequest(data: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/SetDone', data)
  }
  public getAllResourceRequestByProject(projectId: number, status: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetAllByProject?projectId=' + projectId + '&status=' + status)
  }

  public ConfirmOutProject(input: any) {
    return this.http.post(this.rootUrl + `/ConfirmOutProject`, input)

  }
  public ConfirmJoinProject(projectUserId: number, startTime: any) {
    return this.http.get(this.rootUrl + `/ConfirmJoinProject?projectUserId=${projectUserId}&startTime=${startTime}`)
  }


  public EditProjectUserPlan(input: any) {
    return this.http.post(this.rootUrl + `/EditProjectUserPlan`, input)
  }

  public PlanNewResourceToProject(input: any) {
    return this.http.post(this.rootUrl + `/PlanEmployeeJoinProject`, input)
  }

  public AddUserToTempProject(input: any) {
    return this.http.post(this.rootUrl + `/AddUserFromPoolToTempProject`, input)
  }

  public deleteMyRequest(id): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/DeleteMyRequest?resourceRequestId=' + id)
  }
  public DownloadCVLink(resourceRequestId: number) {
    return this.http.get<any>(this.rootUrl + '/DownloadCVLink?resourceRequestId=' + resourceRequestId);
  }

  public createTraining(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/CreateTraining', item);
  }
  public getListRequestCode(): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/GetResourceRequestCode');
  }
}
