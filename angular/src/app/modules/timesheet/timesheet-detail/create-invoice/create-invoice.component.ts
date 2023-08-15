import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { ClientInvoiceDto } from '@app/service/model/timesheet.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-create-invoice',
  templateUrl: './create-invoice.component.html',
  styleUrls: ['./create-invoice.component.css']
})
export class CreateInvoiceComponent extends AppComponentBase implements OnInit {
  clientDataList: ClientInvoiceDto[] = []
  listInvoice: any[] = []
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private timeSheetProjectService: TimesheetProjectService,
    public dialogRef: MatDialogRef<CreateInvoiceComponent>, injector: Injector) {
    super(injector)
  }

  ngOnInit(): void {
    this.getClientData();
  }

  getClientData() {
    this.timeSheetProjectService.getClient(this.data.timeSheetId)
      .pipe(catchError(this.timeSheetProjectService.handleError)).subscribe(data => {
        this.clientDataList = data.result
        this.listInvoice = this.clientDataList.map(item => { return { clientId: item.clientId, isMergeInvoice:false } })
      })
  }
  SaveAndClose() {
    this.isLoading = true
    this.timeSheetProjectService.createInvoice(this.data.timeSheetId, this.listInvoice)
    .pipe(catchError(this.timeSheetProjectService.handleError))
    .subscribe(rs => {
      this.dialogRef.close(this.listInvoice);
      abp.notify.success("Create invoice successfully");
    },
      () => { this.isLoading = false })
  }

  onMergeInvoice(clientId: number, event) {
    this.listInvoice.forEach(item => {
      if (item.clientId == clientId) {
        item.isMergeInvoice = event.checked
      }
    })
  }
}
export class MergeInvoiceDto {
  clientId: number;
  isMergeInvoice: boolean;
}