import { Component, EventEmitter, Inject, OnInit, Output } from "@angular/core";
import { ProjectUserBillService } from "@app/service/api/project-user-bill.service";
import { UpdateInvoiceDto } from "@app/service/model/updateInvoice.dto";
import { BsModalRef } from "ngx-bootstrap/modal";
import { Subscription } from "rxjs";
import { finalize } from "rxjs/operators";
import { APP_ENUMS } from "@shared/AppEnums";
import { SubInvoice } from "@app/service/model/bill-info.model";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { DialogDataDto } from "@app/service/model/common-DTO";
import { DropDownDataDto } from "@shared/filter/filter.component";
@Component({
  selector: "app-invoice-setting-dialog",
  templateUrl: "./invoice-setting-dialog.component.html",
  styleUrls: ["./invoice-setting-dialog.component.css"],
})
export class InvoiceSettingDialogComponent implements OnInit {
  public APP_ENUMS = APP_ENUMS
  fullName: string;
  projectName: string;
  projectId: number;
  note: string;
  saving = false;
  public invoiceSettingOptions = Object.entries(APP_ENUMS.InvoiceSetting).map((item) => ({
    key: item[0],
    value: item[1]
  }))
  public updateInvoiceDto: UpdateInvoiceDto;
  public listSelectProject: DropDownDataDto[] = []
  @Output() onSave = new EventEmitter<null>();

  subscription: Subscription[] = [];
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogDataDto,

    public matDialogRef: MatDialogRef<InvoiceSettingDialogComponent>,
    private projectUserBillService: ProjectUserBillService,
  ) {}

  ngOnInit(): void {
    Object.assign(this, this.data.dialogData);
    this.getAvailableProjectForSettingInvoice();
  }

  getAvailableProjectForSettingInvoice(){
    this.subscription.push(this.projectUserBillService.getAvailableProjectsForSettingInvoice(this.projectId).subscribe(rs => {
      this.listSelectProject = rs.result.map(project => ({
        displayName: project.projectName,
        value: project.projectId
      }))
    }))
  }
  SaveAndClose() {
    let payload: UpdateInvoiceDto = {
      projectId: this.updateInvoiceDto.projectId,
      discount: this.updateInvoiceDto.discount,
      invoiceNumber: this.updateInvoiceDto.invoiceNumber,
      isMainProjectInvoice: this.updateInvoiceDto.isMainProjectInvoice,
      mainProjectId: this.updateInvoiceDto.mainProjectId,
      subProjectIds: this.updateInvoiceDto.subProjectIds
    };
    this.saving = true;
    this.subscription.push(
      this.projectUserBillService
        .updateInvoiceSetting(payload)
        .pipe(
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe(() => {
          this.matDialogRef.close();
          this.onSave.emit();
          const message = this.updateInvoiceDto.isMainProjectInvoice ? "Update main project": "Update sub project"
          abp.notify.success(message);
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
