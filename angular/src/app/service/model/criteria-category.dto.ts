export class CriteriaCategoryDto{
    name: string;
    id?:number;
}
export class CriteriaDto{
    name: string;
    weight: number;
    criteriaCatagoryName: string;
    note: string;
    id?: number
    criteriaCategoryId:number;
}
export class ProjectCriteriaDto{
    id?: number;
    name: string;
    guideline: string;
    isActive: boolean;
}
