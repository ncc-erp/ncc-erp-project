import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BaseApiService } from "./base-api.service";

@Injectable({
  providedIn: "root",
})
export class PMReportProjectContributionService extends BaseApiService {
  changeUrl() {
    return "PMReportProjectContribution";
  }
  constructor(http: HttpClient) {
    super(http);
  }

  public updateWeeklyHistory(input: any): Observable<any> {
    return this.http.post(this.rootUrl + `/CreateOrUpdate`, input);
  }

  public getAllWeeklyContribution(input: any): Observable<any> {
    return this.http.post(this.rootUrl + `/GetAllPaging`, input);
  }

  public getAllPagingContributions(
    pmReportId: number,
    input: any,
  ): Observable<any> {
    return this.http.post(this.rootUrl + `/GetAllPagingContributions`, input, {
      params: { pmReportId: pmReportId.toString() },
    });
  }

  public getTotalContribution(
    pmReportId: number,
    input: any,
  ): Observable<any> {
    return this.http.post(this.rootUrl + `/GetTotalContribution`, input, {
      params: { pmReportId: pmReportId.toString() },
    });
  }
}
