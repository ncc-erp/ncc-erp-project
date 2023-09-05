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
