import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

import {
  AbpResponse, GeneratedKnowledgeBaseDto, KnowledgeBaseDto,
  KnowledgeBaseFileOptionDto, KnowledgeBaseReferenceFileDto, SavedKnowledgeBaseDto
} from '../model/project-knowledge-base.dto';

@Injectable({ providedIn: 'root' })
export class ProjectKnowledgeBaseService extends BaseApiService {
  constructor(http: HttpClient) { super(http); }
  public changeUrl(): string { return 'ProjectKnowledgeBase'; }

  public getKnowledgeBase(projectId: string): Observable<AbpResponse<KnowledgeBaseDto>> {
    return this.http.get<AbpResponse<KnowledgeBaseDto>>(this.rootUrl + '/GetKnowledgeBase', { params: { projectId } });
  }

  public getSelectableFiles(projectId: string): Observable<AbpResponse<KnowledgeBaseFileOptionDto[]>> {
    return this.http.get<AbpResponse<KnowledgeBaseFileOptionDto[]>>(this.rootUrl + '/GetSelectableFiles', { params: { projectId } });
  }

  public updateReferenceFiles(projectId: string, projectFileIds: string[]): Observable<AbpResponse<KnowledgeBaseReferenceFileDto[]>> {
    return this.http.put<AbpResponse<KnowledgeBaseReferenceFileDto[]>>(this.rootUrl + '/UpdateReferenceFiles', {
      projectId, projectFileIds
    });
  }

  public generateKnowledgeBase(projectId: string): Observable<AbpResponse<GeneratedKnowledgeBaseDto>> {
    return this.http.post<AbpResponse<GeneratedKnowledgeBaseDto>>(this.rootUrl + '/GenerateKnowledgeBase', null, { params: { projectId } });
  }

  public updateKnowledgeBase(projectId: string, content: string): Observable<AbpResponse<SavedKnowledgeBaseDto>> {
    return this.http.put<AbpResponse<SavedKnowledgeBaseDto>>(this.rootUrl + '/UpdateKnowledgeBase', { projectId, content });
  }
}
