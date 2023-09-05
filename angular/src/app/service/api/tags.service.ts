import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseApiService } from './base-api.service';
import { Injectable } from '@angular/core';
import { TagsDto } from '../model/tags.dto';

@Injectable({
  providedIn: 'root'
})
export class TagsService extends BaseApiService{
  changeUrl() {
    return 'Tag'
  }
  constructor(http: HttpClient) {
    super(http);
  }
  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('tagId', id)
  })
  }
  
}
