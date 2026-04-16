import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';


@Injectable({
    providedIn: 'root'
})
export class ProjectDailyMeetingService extends BaseApiService {

    changeUrl() {
        return 'DailyMeeting'
    }
    constructor(http: HttpClient) {
        super(http);
    }

    public getProjectWeeklySummary(projectId: number, pmReportId: number): Observable<any> {
        return this.http.get(this.rootUrl + `/Get?projectId=${projectId}&pmReportId=${pmReportId}`);
    }

    public updateMeetingReportCriteria(id: number, payload: { criteriaName: string, content: string, status: number }): Observable<any> {
        return this.http.put(this.rootUrl + `/UpdateMeetingReportCriteria?id=${id}`, payload);
    }

    public deleteMeetingReportCriteria(id: number): Observable<any> {
        return this.http.delete(this.rootUrl + `/DeleteMeetingReportCriteria?id=${id}`);
    }
}
