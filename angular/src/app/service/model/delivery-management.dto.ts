export class RequestResourceDto {
  constructor() {
    this.level = 100;
    this.quantity = 1;
    this.priority = 1;
    this.timeNeed = new Date()
  }
  name: string;
  projectId: number;
  projectName?: string;
  timeNeed?: any;
  status?: number;
  statusName?: string;
  timeDone: string;
  pmNote?: string;
  dmNote?: string;
  plannedNumberOfPersonnel?: number;
  id?: number;
  skillIds?: any;
  level?: any;
  priority?: any;
  quantity: number;
  planUserInfo: any
}

export class TrainingRequestDto {
  constructor() {
    this.level = 100;
    this.quantity = 1;
    this.priority = 1;
    this.timeNeed = new Date()
  }
  name: string;
  projectId: number;
  projectName?: string;
  timeNeed?: any;
  status?: number;
  statusName?: string;
  timeDone: string;
  pmNote?: string;
  dmNote?: string;
  plannedNumberOfPersonnel?: number;
  id?: number;
  skillIds?: any;
  level?: any;
  priority?: any;
  quantity: number;
  planUserInfo: any;
}
export class ResourceRequestDetailDto {
  userId: number;
  FullName: string;
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
  id?: number;
}
export class userAvailableDto {
  userId: number;
  userName: string;
  undisposed: number;
  startDate?: any;
}
export class userToRequestDto {
  id?: number;
  userId?: number;
  allocatePercentage: number;
  startTime: any;
  resourceRequestId?: number;
  projectId?: number;
  status: number;
}
export class availableResourceDto {
  userId: number;
  userName: string;
  emailAddress: string;
  projects: [];
  used: number;
  fullName: string;
  listSkills: any[];
  totalFreeDay: any;
  starRate: number;
  avgPoint:number
}
export class planUserDto {
  projectId: number;
  userId: number;
  percentUsage: number;
  projectRole: number;
  startTime: string;
  isExpense?: true;
  fullName: string;
  isPool: boolean;
  allocatePercentage: number;
  disabled: boolean;
}
export class editFutureResourceDto {
  fullName: string;
  userId: number;
  projectId: number;
  allocatePercentage: number;
  startTime: string;
  id?: number;
  status: number;
}
export class futureResourceDto {
  fullName: string;
  userId: number;
  userName: string;
  projectid: number;
  projectName: string;
  startDate: string;
  use: number;
  status: number;
  id?: number;
}

export class ReviewInternRetroHisotyDto{
  email: string
  pointHistories: ReviewRetroInfoDto[]
  averagePoint: number
}
export class ReviewRetroInfoDto{
  point: number
  isRetro: boolean
  startDate: string
  projectName: string
  note:string
}
