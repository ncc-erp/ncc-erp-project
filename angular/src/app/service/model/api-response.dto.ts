import { HttpErrorResponse } from "@angular/common/http";

export class ApiResponse<T>{
    result?: T;
    targetUrl?: string;
    success: boolean;
    error: HttpErrorResponse;
    unAuthorizedRequest?: boolean;
}