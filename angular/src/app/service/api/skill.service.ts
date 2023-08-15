import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SkillService extends BaseApiService{

  changeUrl() {
    return 'Skill'
  }
  constructor(http: HttpClient) {
    super(http);
  }

  public deleteSkill(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
        params: new HttpParams().set('skillId', id)
    })
}
}
