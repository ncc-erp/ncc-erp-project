export class CheckpointResultDto{
    phaseId?: number;
      userId?: number;
      userName?: string;
      reviewerId?: number;
      reviewerName?: string;
      userNote?: string;
      pmNote?: string;
      finalNote: string;
      currentLevel?: number;
      expectedLevel?: number;
      nowLevel: number;
      pmScore?: number;
      teamScore?: number;
      clientScore?: number;
      examScore?: number;
      status?: number;
      id: number;
      tag:[]
}
export class CheckpointUserEditDto{
  checkPointUserResultId: number;
  finalNote: string;
  now: number;
  tagIds:any;
}