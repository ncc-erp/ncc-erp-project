export class ImportFileDto {
    file: File;
    projectId: number;
    note: string;
    auditDate: string;
}
export class AuditInfo {
    id: number;
    note: string;
    auditDate: string;
    score: number;
    status: number;
    pmName: string;
}
export class CriteriaResult {

    id: number;
    projectId: number;
    processCriteriaId: number;
    projectProcessResultId: number;
    note: string;
    status: number;
    score: number;
}
export class ResponseFailDto
{
    row:number
    reasonFail:string
}
export class ImportProcessCriteriaResultDto {
    auditInfo: AuditInfo;
    criteriaResult: CriteriaResult[];
    failedList: ResponseFailDto[];
}
export class GetPMFilterDto{
    id: number;
    emailAddress: string;
    fullName: string;
}
export class GetClientFilterDto{
    id: number;
    name: string;
    code: string;
}
