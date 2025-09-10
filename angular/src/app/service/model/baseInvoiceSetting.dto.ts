export interface BaseInvoiceSettingDto {
    invoiceNumber: number,
    discount: number,
    isMainProjectInvoice: boolean,
    mainProjectId: number;
    subProjectIds: number[]
    otTypes?: { id?: number, otTypeName: string, multiplier: number }[]
}
