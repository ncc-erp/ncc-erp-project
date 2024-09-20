export class ProjectDto {
  name: string;
  code: string;
  projectType?: 0;
  startTime: string;
  endTime: string;
  status?: number;
  clientId?: number;
  isCharge?: boolean;
  pmId: number;
  id?: number;
  isRequiredWeeklyReport:boolean;
}

export interface ResponseWrapper<T> {
  result: T;
  error?: unknown;
  success?: boolean;
  targetUrl?: unknown;
}

export interface IProjectHistoryUser {
  userId: number;
  fullName: string;
  projectId: number;
  projectName: string;
  projectRole: string;
  allocatePercentage: number;
  startTime: string;
  status: string;
  isExpense: true;
  resourceRequestId: number;
  resourceRequestName: string;
  pmReportId: number;
  pmReportName: string;
  isFutureActive: true;
  emailAddress: string;
  userName: string;
  avatarPath: string;
  avatarFullPath: string
  userType: number;
  branch: number;
  note: string;
  id: number;
  pMName: string;
  workType: boolean;
  isPool?: boolean;
}

export class projectUserDto {
  userId: number;
  userName: string;
  fullName: string;
  projectId: number;
  projectName: string;
  projectRole: string;
  allocatePercentage: number;
  startTime: string;
  status: any;
  isExpense: boolean;
  resourceRequestId: number;
  resourceRequestName: string;
  pmReportId: number;
  pmReportName: string;
  isFutureActive: boolean;
  id: number;
  createMode?: boolean;
  viewMode?: boolean;
  workType:boolean;
  isPool?: boolean;
}

export class projectResourceRequestDto {
  name: string;
  projectId: number;
  timeNeed: string;
  status: string;
  timeDone: string;
  note: string;
  id: number;
  createMode?: boolean;
  planUserInfo:any
  pmNote?: string;
  dmNote?: string;
}

export class projectUserBillDto {
  userId: number;
  userName: string;
  emailAddress: string;
  projectId: number;
  projectName: string;
  billAccountName: string;
  accountName: string;
  billRole: string;
  billRate: number;
  headCount: number;
  startTime: string;
  endTime: string;
  currency: number;
  isActive: boolean;
  isExpose: boolean;
  createMode?: boolean;
  note: string;
  shadowNote: string;
  workingTime: string;
  id: number;
  timesheetId: number;
  userList?: any[];
  chargeType?: number;
  chargeTypeName: string;
  linkedResources: any[];
  createLinkResourceMode?: boolean;
  linkCV: string;
  totalHeadCount:number;
}

export class ProjectRateDto {
  currencyName: string;
  isCharge: boolean;
  chargeType: number;
}

export class MilestoneDto {
  projectId: number;
  name: string;
  description: string;
  flag: string;
  status: string;
  uatTimeStart: string;
  uatTimeEnd: string;
  note: string;
  id?: number;
  createMode?: boolean;
}

export class ProjectInfoDto {
  projectName: string;
  clientName: string;
  clientCode: string;
  pmName: string;
  totalBill: number;
  totalResource: number;
  projectCode:string;
  automationNote: string;
  pmNote: string;
  projectUserBills: any;
}

export class TrainingProjectDto {
  name: string;
  code: string;
  projectType?: number;
  startTime: string;
  endTime: string;
  status?: number;
  clientId?: number;
  clientName?: string;
  currencyId?: number;
  currencyName?: string;
  isCharge?: true;
  pmId: number;
  pmName?: string;
  pmFullName?: string;
  pmEmailAddress?: string;
  pmUserName?: string;
  pmAvatarPath?: string;
  pmAvatarFullPath?: string;
  pmUserType?: number;
  pmBranch?: number;
  isSent?: number;
  timeSendReport?: string;
  dateSendReport?: string;
  evaluation?: string;
  id: number;
  isRequiredWeeklyReport:boolean;
  resourceInfo?: any; 
}

export class ProductProjectDto {
  name: string;
  code: string;
  startTime: string;
  endTime: string;
  status: number;
  pmId: number;
  pmName: string;
  pmFullName: string;
  pmEmailAddress: string;
  pmUserName: string;
  pmAvatarPath: string;
  pmUserType: number;
  projectType: number;
  pmBranch: number;
  isSent: number;
  timeSendReport: string;
  dateSendReport: string;
  id: number;
  requireTimesheetFile?: boolean;
}