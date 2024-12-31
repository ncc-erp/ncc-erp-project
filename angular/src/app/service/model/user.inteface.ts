export interface IUser {
  userName: string;
  name: string;
  surname: string;
  emailAddress: string;
  isActive: true;
  fullName: string;
  lastLoginTime: string;
  creationTime: string;
  roleNames: [string];
  id: number;
  fullNameNormal: string;
  branch: number;
  avatarPath: string;
  avatarFullPath: string
  userType: number;
  userLevel: number;
  userSkills: any[] | undefined;
  userCode: string;
  poolNote: string;
  userProjectHistory?:IUSerProjectHistory[];
}
export interface IUSerProjectHistory {
  ProjectName: string;
  ProjectRole: string;
  StartTime: string;
  allowcatePercentage: number;
  Status: number;
}

export interface IGetUserInfo {
  emailAddress: string;
  avatarPath: string;
  avatarFullPath: string;
  userType: number;
  userLevel: number;
  isActive: boolean;
  fullName: string;
  userName: string;
  branchId: number;
  branchColor: string;
  branchDisplayName: string;
  positionId: number;
  positionColor: string;
  positionName: string;
  contribute: number;
  id: number;
}