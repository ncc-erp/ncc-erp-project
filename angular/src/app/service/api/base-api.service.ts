import { ConfigURIDto } from './../model/configURI.dto';
import { AppConsts } from './../../../shared/AppConsts';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { FilterRequest } from '../model/common-DTO';
import { PagedRequestDto } from './../../../shared/paged-listing-component-base';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');

export abstract class BaseApiService {
    public configURI = AppConsts.configURI
    protected baseUrl = AppConsts.remoteServiceBaseUrl;

    protected get rootUrl() {
        return this.baseUrl + '/api/services/app/' + this.changeUrl();
    }

    protected http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
    }

    abstract changeUrl();

    protected getUrl(url: string) {
        return this.rootUrl + '/' + url;
    }

    public getOne(id: any, includes?: any): Observable<any> {
        return this.http.get(this.rootUrl + '/Get?' + `id=${id}`);
    }

    public filterAndPaging(request: FilterRequest): Observable<any> {
        return this.http.post<any>(this.rootUrl + '/GetAllPaging', request);
    }

    public getAllPaging(request: PagedRequestDto): Observable<any> {
        return this.http.post<any>(this.rootUrl + '/GetAllPaging', request);
    }
    public getAll(): Observable<any> {
        return this.http.get<any>(this.rootUrl + '/GetAll');
    }
    public getById(id: any): Observable<any> {
        return this.http.get<any>(this.rootUrl + '/Get?id=' + id);
    }

    public delete(id: any): Observable<any> {
        return this.http.delete<any>(this.rootUrl + '/Delete', {
            params: new HttpParams().set('Id', id)
        })
    }

    public update(item: any): Observable<any> {
        return this.http.put<any>(this.rootUrl + '/Update', item);
    }

    public create(item: any): Observable<any> {
        return this.http.post<any>(this.rootUrl + '/Create', item);
    }

    public filter(key: FilterRequest): Observable<any> {
        return this.http.get(this.rootUrl + '/Filter?' + `Includes=${key.includes}&Filters=${key.filters}&Sorts=${key.sorts}&Page=${key.page}&PageSize=${key.pageSize}`);
    }

    public save(data: object): Observable<any> {
        return this.http.post(this.rootUrl + '/Save', data);
    }

    public handleError(error: any) {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
            errorMessage = `Error: ${error.error.message}`;
        } else {
            errorMessage = `Error: ${error.error.error.message}`;
        }
        abp.notify.error(errorMessage);
        return throwError(errorMessage);
    }
}
