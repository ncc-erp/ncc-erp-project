import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { BaseApiService } from "./base-api.service";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class ShadowAccountService extends BaseApiService {
  changeUrl() {
    return "ShadowAccount";
  }
  constructor(http: HttpClient) {
    super(http);
  }

  public getShadowAccountByProject(projectId, userId): Observable<any> {
    return this.http.get<any>(
      this.rootUrl +
        `/GetShadowAccountByProject?projectId=${projectId}&userId=${userId}`
    );
  }

  // public getUsersByProject(projectId, userId): Observable<any> {
  //   return this.http.get<any>(
  //     this.rootUrl +
  //       `/GetShadowAccountByProject?projectId=${projectId}&userId=${userId}`
  //   );
  // }
}
