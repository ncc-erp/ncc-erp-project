export interface IBillInfo {
    userInfor: IUserInfor;
    projects: IProject[];
    totalHeadCount: number;
}

export interface IUserInfor {
    userId: number;
    avatarPath: string | null;
    fullName: string;
    branch: number;
    branchColor: string;
    branchDisplayName: string;
    positionId: number;
    positionName: string;
    positionColor: string;
    emailAddress: string;
    simplizeEmailAddress: string;
    userType: number;
    userLevel: number;
    userSkills: IUserSkill[];
    skillNote: string;
}

export interface IUserSkill {
    userId: number;
    skillId: number;
    skillName: string;
    skillRank: number;
    skillNote: string;
}

export interface IProject {
    billId: number;
    projectId: number;
    projectName: string;
    projectStatus: number;
    accountName: string;
    billRate: number;
    headCount: number;
    startTime: string; // ISO 8601 format
    endTime: string | null;
    note: string | null;
    isActive: boolean;
    isExpose: boolean;
    chargeType: string | null;
    currencyCode: string;
    projectCode: string | null;
    clientId: number;
    clientCode: string;
    clientName: string;
    rateDisplay: string;
    linkedResources: ILinkedResource[];
}

export interface ILinkedResource {
    emailAddress: string;
    avatarPath: string;
    avatarFullPath: string;
    userType: number;
    userLevel: number;
    isActive: boolean;
    fullName: string;
    userName: string;
    branchColor: string;
    branchDisplayName: string;
    positionId: number;
    positionColor: string;
    positionName: string;
    contribute: number;
    id: number;
}