export interface ProjectInvoiceSettingDto {
    currencyName: string,
    invoiceNumber: number,
    discount: number,
    isMainProjectInvoice: boolean,
    mainProjectId?: number,
    subProject: IdNameDto[],
    subProjectNames: string[],
    subProjectIds: number[],
    otTypes?: { id?: number, otTypeName: string, multiplier: number, isNormalInvoice: boolean }[],
}

export interface IdNameDto {
    id: number,
    name: string
}
