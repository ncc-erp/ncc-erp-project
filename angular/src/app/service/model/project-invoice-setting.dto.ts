export interface ProjectInvoiceSettingDto {
    currencyName: string,
    invoiceNumber: number,
    discount: number,
    isMainProjectInvoice: boolean,
    mainProjectId?: number,
    subProject: IdNameDto[],
    subProjectNames: string[],
    subProjectIds: number[]
}

export interface IdNameDto {
    id: number,
    name: string
}
