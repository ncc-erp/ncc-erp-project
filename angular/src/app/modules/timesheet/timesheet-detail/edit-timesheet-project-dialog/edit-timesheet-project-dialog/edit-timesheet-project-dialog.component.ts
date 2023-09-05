

import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { SubInvoice } from '@app/service/model/bill-info.model';
import { APP_ENUMS } from '@shared/AppEnums';
import { DropDownDataDto } from '@shared/filter/filter.component';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { UpdateTimesheetProjectDto } from '@app/service/model/updateTimesheetProject.dto'
@Component({
  selector: 'app-edit-timesheet-project-dialog',
  templateUrl: './edit-timesheet-project-dialog.component.html',
  styleUrls: ['./edit-timesheet-project-dialog.component.css']
})
export class EditTimesheetProjectDialogComponent implements OnInit {
  public APP_ENUMS = APP_ENUMS
  id: number;
  invoiceNumber: number;
  workingDay: number;
  transferFee: number;
  discount: number;
  projectId: number;
  projectName: string;
  saving = false;
  isMainProjectInvoice: boolean;
  subProjectIds: number[] = []
  mainProjectId: number
  listSelectProject: DropDownDataDto[] = []
  public invoiceSettingOptions = Object.entries(APP_ENUMS.InvoiceSetting).map((item) => ({
    key: item[0],
    value: item[1]
  }))
  @Output() onSave = new EventEmitter<null>();

  subscription: Subscription[] = [];
  constructor(
    public bsModalRef: BsModalRef, 
    private timesheetProjectService:TimesheetProjectService,
    private projectUserBillService: ProjectUserBillService
    ) {}

  ngOnInit(): void {
   this.getAvailableProject();
  }

  getAvailableProject(){
    this.projectUserBillService.getAvailableProjectsForSettingInvoice(this.projectId).subscribe(rs => {
      this.listSelectProject = rs.result.map(item => ({displayName: item.projectName, value: item.projectId}))
     })
  }

  SaveAndClose() {
    if(+this.invoiceNumber <= 0) {
      abp.message.error("Invoice Number must be bigger than 0!");
      return;
    }
    if(+this.workingDay <= 0) {
      abp.message.error("Working Day must be bigger than 0!");
      return;
    }
    if(+this.transferFee < 0) {
      abp.message.error("Transfer Fee must be bigger than or equal to 0!");
      return;
    }
    if(+this.discount < 0) {
      abp.message.error("Discount must be bigger than or equal to 0!");
      return;
    }
    if(this.mainProjectId == null && !this.isMainProjectInvoice){
      abp.message.error("Main project must not be null!")
      return;
    }
    let requestBody: UpdateTimesheetProjectDto = {
      id : this.id,
      invoiceNumber: this.invoiceNumber,
      workingDay: this.workingDay,
      transferFee: this.transferFee,
      discount: this.discount,
      isMainProjectInvoice: this.isMainProjectInvoice,
      mainProjectId: this.isMainProjectInvoice? null : this.mainProjectId,
      subProjectIds: this.isMainProjectInvoice? (this.subProjectIds ? this.subProjectIds : []) : [],
    }
    this.saving = true;
    this.subscription.push(
      this.timesheetProjectService
        .updateTimesheetProject(requestBody)
        .pipe(
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe(() => {
          this.bsModalRef.hide();
          this.onSave.emit();
          abp.notify.success("Updated Invoice Info")
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }

}
