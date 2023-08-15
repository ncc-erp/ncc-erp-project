export class PhaseDto{
    name:number;
    year: number;
    parentName?: string;
    type: string;
    status: number;
    isCriteria: boolean;
    index: number;
    id: number;
    parentId:number;
}
export class getAllPhaseDto{
    phaseId: number;
    phaseName:string;
    type:number;
    status:number;
}