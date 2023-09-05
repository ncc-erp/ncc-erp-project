import { number } from "echarts";
import { IdNameDto } from "./project-invoice-setting.dto";

export class TimesheetDto {

    name: string;
    month: number;
    year: number;
    totalProject?: number;
    totalTimesheet?: number;
    totalWorkingDay: number;
    status: number;
    isActive: boolean;
    id?: number;
}
export class TimesheetDetailDto {
    projectId: number;
    projectName: string;
    pmId: number;
    pmName: string;
    clientId: number;
    clientName: string;
    file: string;
    note: string;
    id: number;
    timesheetId: number;
    projectBillInfomation: ProjectBillInfoDto[];
    isComplete:boolean;
    chargeType: string;
    totalAmountProjectBillInfomation: number;
    roundTotalAmountProjectBillInfomation: number;
    isMainProjectInvoice: boolean;
    mainProjectId?: number;
    mainProjectName: string;
    subProjects: IdNameDto[];
    subProjectsName: string[];
    subProjectIds: number[];
}

export class ProjectBillInfoDto{
    accountName: string;
    billRate: number;
    billRole: string;
    chargeType: number;
    currency: string;
    description: string;
    fullName: string;
    userFullName: string;
    workingTime: number;
    amout: number;
    roundAmount: number;
}


export class ProjectTimesheetDto {
    projectId: number;
    timesheetId: number;
    note: string;
    id?: number;
    createMode?: boolean;
    projectBillInfomation: string;
    timesheetFile:string;

}
export class UploadFileDto {
    TimesheetProjectId: number;
    File: any;

}
export class ClientInvoiceDto {
    clientId: number;
    clientName: string;
    totalProject: number
}
export class FileExportInvoiceDto {
    fileName: string;
    message: string;
    html: string
}

export class TimesheetProjectBill {
    userId: number
    userName: string
    projectId: number
    projectName: string
    billAccountName: string
    accountName: string
    billRole: string
    billRate: number
    startTime: string
    endTime: string
    note: string
    shadowNote: string
    isActive: boolean
    emailAddress: string
    fullName: string
    avatarPath: string
    avatarFullPath: string
    userType: number
    branch: number
    workingTime: number
    id: number
    createMode?: boolean
    timesheetId: number
    currency: string
    userList:any[]
    searchText:string
    chargeType?: number
    chargeTypeName: string
    isEditing: boolean
    isDisable: boolean
}
export class TotalAmountByCurrencyDto{
    CurrencyName : string;
    Amount : string;
    RoundAmount : string;
}
export class TimesheetInfoDto {
    timesheetId: number;
    timesheetName: string;
    date: string;
    currencies: CurrencyDto[];
}

export class CurrencyDto {
    currencyName: string;
    exchangeRate: number
}

