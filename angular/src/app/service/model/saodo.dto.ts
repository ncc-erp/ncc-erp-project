export class SaodoDto{
    name: string;
    startTime: string;
    endTime: string;
    projectName?: string;
    pmName?: string;
    projectStatus?: string;
    countProjectCreate?: number;
    countProjectCheck?: number;
    countFail?: number;
    id?: number;
}
export class SaodoDetailDto{
    name?: string;
    startTime?: Date;
    endTime?: Date;
    projectName?: string;
    pmName: string;
    auditResultStatus: string;
    countProjectCreate?: number;
    countProjectCheck?: number;
    countFail?: number;
    id?: number;
}
export class ProjectSaodoDto{
    projectId: number;
    auditSessionId: string;
    note?:string;
    id?:number;
    pmId:string;
    status:string;
    pmName?:string;
}
export class SaodoProjectUserDto{
    auditResultId ?: string;
    checkListItemId ?: number ;
    userId ?: number ;
    note ?:  string  ;
    curatorId ?: number ;
    isPass ?: true ;
    id?: number
}