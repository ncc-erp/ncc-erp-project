import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ChecklistCategoryService extends BaseApiService{

  changeUrl() {
    return 'CheckListCategory'
  }
  constructor(http: HttpClient) {
    super(http);
  }
}
