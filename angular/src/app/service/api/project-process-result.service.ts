import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';
import {ImportFileDto} from "app/service/model/project-process-result.dto"

@Injectable({
    providedIn: 'root'
})
export class ProjectProcessResultAppService extends BaseApiService {

    changeUrl() {
        return 'ProjectProcessResult';
    }

    constructor(http: HttpClient) {
        super(http);
    }

    public getProjectToImportResult(): Observable<any>{
        return this.http.get(this.rootUrl + '/GetProjectToImportResult');
    }
    public importToProjectProcessResult(importfile: ImportFileDto): Observable<any> {
        let formData = new FormData();
        formData.append('File', importfile.file);
        formData.append('ProjectId', importfile.projectId.toString());
        formData.append('Note', importfile.note);
        formData.append('AuditDate', importfile.auditDate);

        return this.http.post(this.rootUrl + `/ImportToProjectProcessResult`, formData);
    }
    public exportProjectProcessResultTemplate(projectId: number):Observable<any> {
        return this.http.get(this.rootUrl + `/ExportProjectProcessResultTemplate?projectID=${projectId}`);
    }
    public deleteResult(id: number): Observable<any> {
        return this.http.delete(this.rootUrl + `/Delete?Id=${id}`);
    }
    public updateNote(id, note): Observable<any>{
        return this.http.post(this.rootUrl + '/UpdateNote', { id, note });
    }
    public getPMProcessResultInfors(): Observable<any>{
        return this.http.get(this.rootUrl + '/GetPMProcessResultInfors');
    }
    public getClientProcessResultInfors(): Observable<any>{
        return this.http.get(this.rootUrl + '/GetClientProcessResultInfors');
    }
}
