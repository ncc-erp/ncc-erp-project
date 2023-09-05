import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
    providedIn: 'root'
})
export class ProjectProcessCriteriaResultAppService extends BaseApiService {

    changeUrl() {
        return 'ProjectProcessCriteriaResult';
    }

    constructor(http: HttpClient) {
        super(http);
    }

    public search(projectProcessResultId: number, projectId: number, searchText: string, status?): Observable<any> {

        return this.http.post<any>(this.rootUrl + `/GetTreeListProjectProcessCriteriaResults`, { projectProcessResultId, projectId, searchText, status });
    }
    public getTreeListPPCR(projectProcessResultId: number, projectId: number): Observable<any> {
        return this.http.post<any>(this.rootUrl + `/GetTreeListProjectProcessCriteriaResults`, { projectProcessResultId, projectId })
    }

    public updateProjectProcessCriteriaResult(item: any): Observable<any> {
        return this.http.put<any>(this.rootUrl + '/UpdateProjectProcessCriteriaResult', item);
    }

    public getProjectProcessCriteriaById(id: number): Observable<any> {
        return this.http.get<any>(this.rootUrl + '/GetProjectProcessCriteriaResultById?Id=' + id);
    }
}
