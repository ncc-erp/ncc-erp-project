export interface ProjectDto {
    name: string;
    code: string;
    projectType: number;
    startTime: string;
    endTime: string;
    status: number;
    clientId: number;
    clientName?: string;
    isCharge: boolean;
    chargeType?: number;
    pmId: number;
    pmName?: string;
    id: number;
    currencyId: string;
    requireTimesheetFile?: boolean;
    isRequiredWeeklyReport: boolean;
}

export interface ClientDto {
    name: string;
    code: string;
    displayName: string;
    id: number;
    address: string;
    invoiceDateSetting: number;
    paymentDueBy: number;
    transferFee: number;
}

export interface SkillDto {
    name: string;
    id: number;
}

export interface AccountTypeDto {
    name: string;
    id: number;
}

export interface CreatorDto {
    name: string;
    id: number;
}

export interface ProjectResourceDto {
    id: number;
    name: string;
    note?: string;
    parentId?: number;
    childrens?: ProjectResourceDto[];
}

export class projectForDM {
    projectName: string;
    pmName: string;
    listUsers: [];
    problemsOfTheWeek: []
}
