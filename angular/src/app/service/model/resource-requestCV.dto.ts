export class ResourceRequestCVDto{
    id : number;
    userId: number;
    user : any;
    status ?: any;
    cvName : string ;
    cvPath : string ;
    linkCVPath : string;
    note : string ;
    kpiPoint ?: number;
    interviewDate ?: Date;
    sendCVDate ?: Date;
    resourceRequestId: number;
    cvStatusId:number;
    cvStatus: object;
}