import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ChecklistService extends BaseApiService{

  changeUrl() {
    return 'CheckListItem'
  }
  constructor(http: HttpClient) {
    super(http);
  }
}
