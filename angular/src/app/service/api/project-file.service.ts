import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectFileService extends BaseApiService {

  changeUrl() {
    return 'ProjectFile'
  }
  constructor(http: HttpClient) {
    super(http);

  }
  getAllFile(id: number): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetFiles?projectId=${id}`);
  }
  // UploadFiles(id: number): Observable<any> {
  //   return this.http.get<any>(this.rootUrl + `/UploadFiles`);
  // }

  UploadFiles(fileList: any, projectId): Observable<any> {
    let formData = new FormData();
    formData.append('ProjectId', projectId)
    fileList.forEach(element => {
      formData.append('Files', element)
    });
    return this.http.post(this.rootUrl + '/UploadFiles', formData);
  }
  
  removeFile(fileName:string,projectId: number): Observable<any> {
    return this.http.delete<any>(this.rootUrl + `/DeleteFile?fileName=${fileName}&projectId=${projectId}`);
  }
}
