import { PagedRequestDto } from '@shared/paged-listing-component-base';
import { AppConsts } from './../../../shared/AppConsts';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseApiService } from './base-api.service';
import { ApiResponse } from '../model/api-response.dto';
import { ResponseResultProjectDto } from '../model/responseResultProject.dto'
import { TimesheetInfoDto } from '../model/timesheet.dto';
@Injectable({
  providedIn: 'root'
})
export class TimesheetProjectService extends BaseApiService {
  changeUrl() {
    return 'TimesheetProject';
  }
  constructor(http: HttpClient) {
    super(http)
  }
  public create(item: any): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/Create', item);
  }
  public setComplete(item: any): Observable<any> {
    return this.http.put<any>(this.rootUrl + '/SetComplete', item);
  }
  public delete(id: any): Observable<any> {
    return this.http.delete<any>(this.rootUrl + '/Delete', {
      params: new HttpParams().set('timesheetProjectId', id)
    })
  }

  public UpdateFileTimeSheetProject(file, id): Observable<any> {
    const formData = new FormData();
    // if (navigator.msSaveBlob) {
    //     formData.append('File', file);
    // } else {

    // }
    formData.append('File', file);
    formData.append('TimesheetProjectId', id);
    const uploadReq = new HttpRequest(
      'POST', AppConsts.remoteServiceBaseUrl + '/api/services/app/TimesheetProject/UpdateFileTimeSheetProject', formData,
      {
        reportProgress: true
      }
    );
    return this.http.request(uploadReq);
  }
  public GetTimesheetDetail(id: any, request: PagedRequestDto): Observable<any> {
    return this.http.post<any>(this.rootUrl + '/GetAllProjectTimesheetByTimesheet?timesheetId=' + id, request);
  }

  public getAllByProject(projectId: number) {
    return this.http.get<any>(this.rootUrl + '/GetAllByProject?projectId=' + projectId);

  }
  public DownloadFileTimesheetProject(projectId: number) {
    return this.http.get<any>(this.rootUrl + '/DownloadFileTimesheetProject?timesheetProjectId=' + projectId);

  }
  public GetTimesheetFile(id: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/DownloadFileTimesheetProject?timesheetProjectId=' + id);
  }
  public getClient(id: any): Observable<any> {
    return this.http.get<any>(this.rootUrl + '/ViewInvoice?timesheetId=' + id);
  }
  public createInvoice(timesheetId: number, mergeInvoice: any[]): Observable<any> {
    let requestBoy = {
      timesheetId: timesheetId,
      mergeInvoice: mergeInvoice
    }
    return this.http.post<any>(this.rootUrl + '/CreateInvoice', requestBoy);
  }
  public exportInvoice(timesheetId, projectId,exportInvoiceMode): Observable<any> {
    let invoiceExcelDto = {
      timesheetId: timesheetId,
      projectIds: [projectId],
      mode:exportInvoiceMode
    }
    return this.http.post(this.rootUrl + `/ExportInvoice`, invoiceExcelDto)
  }
  public exportInvoiceClient(data: any): Observable<any> {
    return this.http.post(this.rootUrl + "/ExportInvoice", data);
  }

  public exportInvoiceAsPDF(timesheetId, projectId,exportInvoiceMode): Observable<any> {
    let invoiceExcelDto = {
      timesheetId: timesheetId,
      projectIds: [projectId],
      mode:exportInvoiceMode
    }
    return this.http.post(this.rootUrl + `/ExportInvoiceAsPDF`, invoiceExcelDto)
  }
  public exportInvoiceClientAsPDF(data: any): Observable<any> {
    return this.http.post(this.rootUrl + "/ExportInvoiceAsPDF", data);
  }


  public updateNote(data): Observable<any>{
    return this.http.post<any>(this.rootUrl + '/updateNote', data);
  }

  public getAllPM(): Observable<any>{
    return this.http.get<any>(this.rootUrl + '/getAllPM');
  }

  public GetBillInfoChart(projectId,fromDate?,toDate?): Observable<any>{
    return this.http.get<any>(this.rootUrl + `/GetBillInfoChart?projectId=${projectId}&fromDate=${fromDate}&toDate=${toDate}`);
  }

  public exportInvoiceForTax(data: any): Observable<any> {
    return this.http.post(this.rootUrl + "/ExportInvoiceForTax", data);
  }

  updateTimesheetProject(item: any): Observable<any> {
    //update invoice number and working day
    return this.http.put<any>(this.rootUrl + '/UpdateTimesheetProject', item);
  }

  checkTimesheetProjectSetting(timesheetId: number):Observable<ApiResponse<string>>{
    return this.http.get<ApiResponse<string>>(this.rootUrl + `/CheckTimesheetProjectSetting?timesheetId=${timesheetId}`)
  }

  sendInvoiceToFinfast(timesheetId: number): Observable<ApiResponse<ResponseResultProjectDto>>{
    return this.http.get<ApiResponse<ResponseResultProjectDto>>(this.rootUrl + `/SendInvoiceToFinfast?timesheetId=${timesheetId}`)
  }
  getExchangeRate(date: string, baseCurrency: string, symbols: string, places: number): Observable<ApiResponse<any>>{
    return this.http.get<ApiResponse<any>>(this.rootUrl + `/GetExchangeRate?date=${date}&baseCurrency=${baseCurrency}&symbols=${symbols}&places=${places}`);
  }
  exportAllTimeSheetProjectToExcel( currencies: TimesheetInfoDto): Observable<any>{
    return this.http.post(this.rootUrl + `/ExportAllTimeSheetProjectToExcel`, currencies);
  }
  reAcTiveTimesheet(timesheetProjectIds): Observable<any>{
    return this.http.post<any>(this.rootUrl + `/ReActiveTimesheetProject`, timesheetProjectIds);
  }
}
