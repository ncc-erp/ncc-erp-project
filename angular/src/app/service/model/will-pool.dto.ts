export class GetAllWillPoolResourceDto {
    resource: GetUserInfo;
    resourceNote: string;
    accounts: AccountDto[];
    projects: ShortInfoProjectDto[];
    totalContribute: number;
}
export class AccountDto {
    id: number;
    project: ShortInfoProjectDto;
    chargeName: string;
    headCount: number;
    endChargeDate: string;
    contribute: number;

}
export class GetUserInfo {
    id: number;
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
}

export class ShortInfoProjectDto {
    id: number;
    projectName: string;
    projectCode: string;
    projectType: number;
}

export class InputGetAllWillPoolResourceDto {
    userName: string;
    branchIds: number[];
    userTypes: number[];
    endChargeDateFrom: Date;
    endChargeDateTo: Date;
}

