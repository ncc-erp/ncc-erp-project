import { Component, EventEmitter, Inject, OnInit, Output, Injector } from "@angular/core";
import { ProjectUserBillService } from "@app/service/api/project-user-bill.service";
import { UpdateInvoiceDto } from "@app/service/model/updateInvoice.dto";
import { BsModalRef } from "ngx-bootstrap/modal";
import { Subscription } from "rxjs";
import { finalize } from "rxjs/operators";
import { APP_ENUMS } from "@shared/AppEnums";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { DialogDataDto } from "@app/service/model/common-DTO";
import { DropDownDataDto } from "@shared/filter/filter.component";
import { ProjectDetailService } from "@app/service/api/project-detail.service";
import { PERMISSIONS_CONSTANT } from "@app/constant/permission.constant";
import { AppComponentBase } from "@shared/app-component-base";

@Component({
  selector: "app-invoice-setting-dialog",
  templateUrl: "./invoice-setting-dialog.component.html",
  styleUrls: ["./invoice-setting-dialog.component.css"],
})
export class InvoiceSettingDialogComponent extends AppComponentBase implements OnInit {
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
  
  Projects_OutsourcingProjects_ViewBillInfo_EditOTType = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ViewBillInfo_EditOTType;

  constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public data: DialogDataDto,
    public matDialogRef: MatDialogRef<InvoiceSettingDialogComponent>,
    private projectUserBillService: ProjectUserBillService,
    private projectDetailService: ProjectDetailService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    Object.assign(this, this.data.dialogData);
    this.updateInvoiceDto.otTypes = this.updateInvoiceDto.otTypes || [];
    this.getAvailableProjectForSettingInvoice();
  }

  canEditOTType(){
    return this.isGranted(PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ViewBillInfo_EditOTType)
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
    const invalidOt = this.updateInvoiceDto.otTypes?.find(ot => 
      !ot.otTypeName || ot.multiplier == null || ot.multiplier < 0
    );
    if (invalidOt) {
      abp.message.warn(
        "Each OT must have a valid type name and a coefficient that is greater than or equal to 0.",
        "Invalid Input"
      );
      return;
    }
    const roundedOtTypes = this.updateInvoiceDto.otTypes?.map((ot) => ({
      ...ot,
      multiplier: Math.round(ot.multiplier * 100) / 100 
    }));
    let payload: UpdateInvoiceDto = {
      projectId: this.updateInvoiceDto.projectId,
      discount: this.updateInvoiceDto.discount,
      invoiceNumber: this.updateInvoiceDto.invoiceNumber,
      isMainProjectInvoice: this.updateInvoiceDto.isMainProjectInvoice,
      mainProjectId: this.updateInvoiceDto.mainProjectId,
      subProjectIds: this.updateInvoiceDto.subProjectIds,
      otTypes: roundedOtTypes
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
          this.projectDetailService.getProjectSummary(this.projectId).subscribe(res => { 
            this.projectDetailService.setValueSummary(res.result)
          });
          this.matDialogRef.close();
          this.onSave.emit();
          const message = this.updateInvoiceDto.isMainProjectInvoice ? "Update main project": "Update sub project"
          abp.notify.success(message);
        })
    );
  }

  addOtType() {
    this.updateInvoiceDto.otTypes.push({ otTypeName: '', multiplier: null });
  }

  removeOtType(index: number) {
    this.updateInvoiceDto.otTypes.splice(index, 1);
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}