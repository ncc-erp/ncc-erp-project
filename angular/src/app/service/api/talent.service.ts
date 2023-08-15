import { BranchTalentDto, PosistionTalentDto, SendRecuitmentDto } from './../model/talent.dto';
import { ApiResponse } from './../model/api-response.dto';
import { HttpClient } from '@angular/common/http';
import { BaseApiService } from './base-api.service';
import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TalentService extends BaseApiService {

  constructor(http: HttpClient) {
    super(http);
  }
  changeUrl() {
    return 'Talent';
  }

  public getBranches(): Observable<ApiResponse<BranchTalentDto[]>> {
    return this.http.get<ApiResponse<BranchTalentDto[]>>(this.rootUrl + '/GetBranches');
  }
  public getPositions(): Observable<ApiResponse<PosistionTalentDto[]>> {
    return this.http.get<ApiResponse<PosistionTalentDto[]>>(this.rootUrl + '/GetPositions');
  }
  public sendRecruitment(data: SendRecuitmentDto): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.rootUrl + '/SendRecruitmentToTalent', data);
  }
}
