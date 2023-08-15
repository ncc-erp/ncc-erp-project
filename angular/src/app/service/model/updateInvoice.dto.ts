import { BaseInvoiceSettingDto } from "./baseInvoiceSetting.dto";

export interface UpdateInvoiceDto extends BaseInvoiceSettingDto {
    projectId: number
}
