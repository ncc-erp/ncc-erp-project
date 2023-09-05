import { IUser } from '@app/service/model/user.inteface';
import { PagedRequestDto } from './../../../shared/paged-listing-component-base';
import { Observable } from 'rxjs';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { AppConsts } from '@shared/AppConsts';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseApiService {
  changeUrl() {
    return 'User';
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public GetAllUserActive(onlyStaff: boolean, isFake?: any): Observable<any> {
    if (isFake) {
      return this.http.get<any>(
        this.rootUrl +
          `/GetAllUserActive?onlyStaff=${onlyStaff}&isFake=${isFake}`
      );
    } else {
      return this.http.get<any>(
        this.rootUrl + `/GetAllUserActive?onlyStaff=${onlyStaff}`
      );
    }
  }

  public getAllActiveUser(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/GetAllActiveUser');
  }
  public uploadImageFile(file, id): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('userId', id);
    // const uploadReq = new HttpRequest(
    //     'POST', AppConsts.remoteServiceBaseUrl + '/api/services/app/User/UpdateAvatar', formData,
    //     {
    //         reportProgress: true
    //     }
    // );
    // return this.http.request(uploadReq);
    return this.http.post<any>(this.rootUrl + '/UpdateAvatar', formData);
  }

  public upLoadOwnAvatar(file): Observable<any> {
    const formData = new FormData();
   
      formData.append('file', file);
 
    const uploadReq = new HttpRequest(
      'POST',
      AppConsts.remoteServiceBaseUrl +
        '/api/services/app/User/UpdateYourOwnAvatar',
      formData,
      {
        reportProgress: true,
      }
    );
    return this.http.request(uploadReq);
  }
  uploadFile(file: File): Observable<any> {
    const formData = new FormData();
    const url = '/api/services/app/User/ImportUsersFromFile';
    formData.append('file', file);
    const uploadReq = new HttpRequest(
      'POST',
      AppConsts.remoteServiceBaseUrl + url,
      formData,
      {
        reportProgress: true,
      }
    );
    return this.http.request(uploadReq);
  }
  autoUpdateUserFromHRM(): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/AutoUpdateUserFromHRM', {});
  }
  getUserPaging(request: PagedRequestDto): Observable<any> {
    return this.http.post<any>(
      this.rootUrl + `/GetAllPaging`,
      request
    );
  }

  updatePoolNote(user: IUser) {
    return this.http.put<IUser>(this.rootUrl + '/Update', user);
  }

  updateUserActive(userId:number, isActive:boolean): Observable<any> {
    return this.http.put<any>(this.rootUrl + `/updateUserActive?userId=${userId}&&isActive=${isActive}`,{});
  }

  deleteFakeUser(userId:number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + `/DeleteFakeUser?userId=${userId}`,);
  }

  updateUserSkills(userSkill): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/updateUserSkill', userSkill);
  }

  UpdateUserRole(user): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/UpdateUserRole', user);
  }

  getHistoryProjectsByUserId(userId:number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetHistoryProjectsByUserId?userId=${userId}`);
  }
}
