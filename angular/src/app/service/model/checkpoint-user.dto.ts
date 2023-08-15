export class CheckpointUserDto{
    reviewerId: number;
    reviewerName: string;
    reviewerEmail?: string;
    reviewerAvatar?: string;
    userId: number;
    userName: string;
    userEmail?: string;
    userAvatar?: string;
    type: 0;
    status: 0;
    score?: number;
    note?: string;
    id?: number;
    phaseId?:string;
}