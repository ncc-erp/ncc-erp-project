export class FilterRequest {
    includes: string = '';
    filters: string = '';
    sorts: string = '';
    page: any = 1;
    pageSize: any = 10;
    }
export class DialogDataDto{
    command:string;
    dialogData:any
}