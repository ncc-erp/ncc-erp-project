

export class GetProjectProcessCriteriaResultDto{
    projectProcessResultId: number;
    projectId: number;
    status: NCSStatus;
    score: number;
    note: string;
    processCriteria: GetProcessCriteriaDto;
    id: number;
}

export enum NCSStatus{
    NC = 1,
    OB = 2,
    RE = 3,
    EX = 4
}

export class GetProcessCriteriaDto{
    code: string;
    name: string;
    isApplicable: boolean;
    isActive: boolean;
    guidLine: string;
    qAExample: string;
    parentId?: number;
    level: number;
    isLeaf: boolean;
}
