export class WeeklyContributionDto {
  pmReportId?: number;
  pmReportName?: string;
  projectId?: number;
  projectName?: string;
}

export interface ProjectBillDetailDto {
  pmReportId?: number;
  reportName?: string;
  accountName: string;
  billRole: string;
  contribute: number;
  headCount: number;
}

export interface ProjectUserContributionDto {
  projectId: number;
  projectName: string;
  pmName: string;
  totalContribute: number;
  billDetails: ProjectBillDetailDto[];
}

export interface UserGroupContributionDto {
  userId: number;
  userName: string;
  userFullName: string;
  avatarPath: string;
  branchDisplayName: string;
  branchColor: string;
  positionName: string;
  positionColor: string;
  totalHeadCount: number;
  projects: ProjectUserContributionDto[];
}

export interface UserTypeDto {
  displayName: string;
  value: number;
}

export interface ContributionAverageUserDto {
  userId: number;
  userName: string;
  userFullName: string;
  avatarPath: string;
  branchDisplayName: string;
  branchColor: string;
  positionName: string;
  positionColor: string;
  averageContribution: number;
  projects: ProjectUserContributionDto[];
}

export interface ContributionAverageResultDto {
  items: ContributionAverageUserDto[];
  totalCount: number;
  weeklyReportInRange: number;
}