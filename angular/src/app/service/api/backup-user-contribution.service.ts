import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FilterMonthlyUserContributionDto } from '../model/user-contribution.dto';

@Injectable({
  providedIn: 'root'
})
export class BackupUserContributionService extends BaseApiService {
  changeUrl() {
    return 'Backup';
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public GetAllBackupMonthlyUserContribution(request: FilterMonthlyUserContributionDto): Observable<any>{
    return this.http.post<any>(this.rootUrl + '/GetAllBackupMonthlyUserContribution', request);
  }

  public BackupMonthlyUserContribution(monthYearTime: string): Observable<any>{
    const params = new HttpParams().set('monthYearTime', monthYearTime);
    return this.http.post<any>(this.rootUrl + '/BackupMonthlyUserContribution', params);
  }
}
