export class GetAllWillPoolResourceDto {
    resource: GetUserInfo;
    resourceNote: string;
    accounts: AccountDto[];
    projectNames: string[];
    totalContribute: number;
    constructor(resource: GetUserInfo, accounts: AccountDto[], resourceNote: string = '', projectNames: string[] = []) {
        this.resource = resource;
        this.accounts = accounts;
        this.resourceNote = resourceNote;
        this.projectNames = projectNames;
        this.totalContribute = accounts.reduce((sum, account) => sum + account.contribute, 0);
    }
}
export class AccountDto {
    id: number;
    projectName: string;
    chargeName: string;
    headCount: number;
    endChargeDate: string;
    contribute: number;
    constructor(projectName:string, chargeName: string, headCount: number, endChargeDate: string, contribute: number) {
        this.projectName = projectName;
        this.chargeName = chargeName;
        this.headCount = headCount;
        this.endChargeDate = endChargeDate;
        this.contribute = contribute;
    }
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
    constructor(id: number, emailAddress: string, avatarPath: string, avatarFullPath: string, userType: number, userLevel: number, isActive: boolean, fullName: string, userName: string, branchColor: string, branchDisplayName: string, positionId: number | null, positionColor: string, positionName: string, contribute: number) {
        this.id = id;
        this.emailAddress = emailAddress;
        this.avatarPath = avatarPath;
        this.avatarFullPath = avatarFullPath;
        this.userType = userType;
        this.userLevel = userLevel;
        this.isActive = isActive;
        this.fullName = fullName;
        this.userName = userName;
        this.branchColor = branchColor;
        this.branchDisplayName = branchDisplayName;
        this.positionId = positionId;
        this.positionColor = positionColor;
        this.positionName = positionName;
        this.contribute = contribute;
    }
}

