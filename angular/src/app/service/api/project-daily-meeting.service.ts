import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectDailyMeetingDto, ProjectDailyReportDto } from '../model/project-daily-meeting.dto';
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

    public updateSummary(summaryData: { id: number, summary: string }): Observable<any> {
        return this.http.put(this.rootUrl + `/UpdateSummary`, summaryData);
    }

    public updateDailyReport(dailyData: { id: number, content: string }): Observable<any> {
        return this.http.put(this.rootUrl + `/UpdateDailyReport`, dailyData);
    }
}
