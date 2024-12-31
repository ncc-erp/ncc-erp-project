export interface IUserInfo {
    emailAddress: string;
    avatarPath: string | null;
    avatarFullPath: string | null;
    userType: number;
    userLevel: number;
    fullName: string;
    userName: string;
    branchId: number;
    branchColor: string;
    branchDisplayName: string;
    id: number;
}

export interface IProjectContribute {
    id: number;
    projectType: number;
    projectName: string;
    projectCode: string;
    contribute: number;
}

export interface IInfoMonthlyUserContribution {
    employee: IUserInfo;
    projectContributes: IProjectContribute[];
    totalContribute: number;
}