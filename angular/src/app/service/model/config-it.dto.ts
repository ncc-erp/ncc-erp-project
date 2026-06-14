export interface ConfigItDto {
  id?: number;
  userId?: number;
  emailAddress?: string;
  avatarPath?: string;
  avatarFullPath?: string;
  userType?: number;
  userLevel?: number;
  branch?: number;
  fullName?: string;
  creationTime?: string;
  branchColor?: string;
  branchDisplayName?: string;
  positionId?: number;
  positionColor?: string;
  positionName?: string;
}

export interface ConfigItUserOptionDto {
  id: number;
  fullName: string;
  emailAddress: string;
  name: string;
  surname: string;
}
