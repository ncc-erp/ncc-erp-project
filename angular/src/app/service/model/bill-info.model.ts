export class ParentInvoice {
    projectId: number;
    parentId?: number;
    isMainInvoice: boolean;
    projectName?: string;
    subInvoices: SubInvoice[];
}
export class SubInvoice {
    projectId: number;
    projectName: string;
    parentId?: number;
    parentName?: string
}