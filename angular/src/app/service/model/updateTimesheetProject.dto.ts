import { BaseInvoiceSettingDto } from './baseInvoiceSetting.dto'
export interface UpdateTimesheetProjectDto extends BaseInvoiceSettingDto {
    id: number;
    workingDay: number;
    transferFee: number;
}