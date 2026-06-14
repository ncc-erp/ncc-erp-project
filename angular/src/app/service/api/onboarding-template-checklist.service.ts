import { Injectable } from "@angular/core";
import { BaseApiService } from "./base-api.service";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({ providedIn: "root" })
export class OnboardingTemplateChecklistService extends BaseApiService {
  changeUrl() {
    return "OnboardingTemplate";
  }

  constructor(http: HttpClient) {
    super(http);
  }

  getAll(): Observable<any> {
    return this.http.get<any>(this.rootUrl + `/GetAll`);
  }

  save(input: any): Observable<void> {
    return this.http.post<void>(this.rootUrl + `/Save`, input);
  }
  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.rootUrl + `/Delete?id=${id}`);
  }
}
