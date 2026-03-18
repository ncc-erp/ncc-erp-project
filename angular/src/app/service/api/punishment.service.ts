import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BaseApiService } from "./base-api.service";

@Injectable({
  providedIn: "root",
})
export class PunishmentService extends BaseApiService {
  changeUrl() {
    return "Punishment";
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + "/Delete", {
      params: new HttpParams().set("id", id),
    });
  }

  public importPunishment(item: any, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('Month', item.month.toString());
    formData.append('Year', item.year.toString());
    formData.append('Note', item.note ? item.note : '');
    formData.append('File', file);
    return this.http.post<any>(this.rootUrl + "/Create", formData);
  }

  public update(item: any, file?: File | null): Observable<any> {
    const formData = new FormData();
    formData.append('Id', item.id.toString());
    formData.append('Month', item.month.toString());
    formData.append('Year', item.year.toString());
    formData.append('Note', item.note ? item.note : '');
    if (file) {
      formData.append('File', file);
    }
    return this.http.put<any>(this.rootUrl + "/Update", formData);
  }

  public getById(id: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + "/GetPunishment", {
      params: new HttpParams().set("id", id),
    });
  }

  public downloadFile(punishmentId: number) {
    return this.http.get<any>(this.rootUrl + '/DownloadPunishmentFile?id=' + punishmentId);
  }

  public getTemplate() {
    return this.http.get<any>(this.rootUrl + '/DownloadPunishmentTemplate');
  }
}
