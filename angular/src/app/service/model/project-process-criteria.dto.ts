export class GetAllProjectProcessCriteriaDto {
    clientName: string;
    id: number;
    listProcessCriteriaIds: number[];
    pmName: string;
    processCriteriaId: number;
    projectCode: string;
    projectId: number;
    projectName: string;
    projectType: string;
}
export class GetAllPagingProjectProcessCriteriaDto{
    clientCode: string
    clientName: string
    countCriteria: number
    pmName: string
    projectCode: string
    projectId: number
    projectName: string
    projectStatus: number
    projectType: number
    selected: boolean;
}
export class CreateProjectProcessCriteriaDto{
    projectIds: number[];
}
