export class PosistionTalentDto {
    id: number;
    position: string;
    items: SubPositionTalentDto[];
}

export class SubPositionTalentDto {
    id: number;
    subPosition: string;
}

export class BranchTalentDto {
    id: number;
    name: string;
    color: string;
}

export class SendRecuitmentDto {
    resourceRequestId: number;
    subPositionId: number;
    branchId: number;
    note: string;
}