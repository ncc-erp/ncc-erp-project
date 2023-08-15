import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateProjectProcessCriteriaDto } from '../model/project-process-criteria.dto';
import { BaseApiService } from './base-api.service';
@Injectable({
    providedIn: 'root'
})
export class ProjectProcessCriteriaAppService extends BaseApiService {

    changeUrl() {
        return 'ProjectProcessCriteria'
    }
    constructor(http: HttpClient) {
        super(http);
    }



    public getAll(projectId?: number, processCriteriaId?: number): Observable<any> {
        return this.http.post(this.rootUrl + `/GetAll`, { projectId, processCriteriaId });
    }
    public getProjectHaveNotBeenTailor(): Observable<any> {
        return this.http.get(this.rootUrl + `/GetProjectHaveNotBeenTailor`);
    }
    public addMultiCriteriaToMultiProject(projectIds: CreateProjectProcessCriteriaDto): Observable<any> {
        return this.http.post(this.rootUrl + `/AddMultiCriteriaToMultiProject`, projectIds);
    }
    public addMultiCriteriaToOneProject(projectId: number, processCriteriaIds: number[]): Observable<any> {
        return this.http.post(this.rootUrl + `/AddMultiCriteriaToOneProject`, { projectId, processCriteriaIds });
    }
    public deleteCriteria(projectId: number, processCriteriaIds: number[]): Observable<any> {
        return this.http.put(this.rootUrl + `/DeleteCriteria`, { projectId, processCriteriaIds });
    }
    public importProjectProcessCriteriaFromExcel(file, projectId): Observable<any> {
        let formData = new FormData();
        formData.append('File', file);
        formData.append('ProjectId', projectId);
        return this.http.post(this.rootUrl + `/ImportProjectProcessCriteriaFromExcel`, formData);
    }

    public deleteProject(projectId: any): Observable<any> {
        return this.http.delete(this.rootUrl + `/DeleteProject?projectId=${projectId}`);
    }

    public getDetail(projectId: number): Observable<any> {
        return this.http.post(this.rootUrl + `/GetDetail`, { projectId });
    }
    public getProcessCriteriaByProjectId(projectId: number): Observable<any> {
        return this.http.get(this.rootUrl + `/GetAllProcessCriteriaByProjectId?projectId=${projectId}`);
    }

    public downloadTemplate(): Observable<any> {
        return this.http.post(this.rootUrl + `/DownloadTemplate`, {});
    }
    public updatePPC(id: number, note: any, applicable): Observable<any> {
        return this.http.post(this.rootUrl + `/UpdateProjectProcessCriteria`, { id, note, applicable });
    }
    public searchDetail(projectId: number, searchText: string ,applicable): Observable<any> {
        return this.http.post<any>(this.rootUrl + `/GetDetail`, { projectId, searchText ,applicable});
    }
    public deleteProjectProcessCriteria(id): Observable<any>{
        return this.http.delete(this.rootUrl + `/DeleteProjectProcessCriteria?id=${id}`);
    }
}
