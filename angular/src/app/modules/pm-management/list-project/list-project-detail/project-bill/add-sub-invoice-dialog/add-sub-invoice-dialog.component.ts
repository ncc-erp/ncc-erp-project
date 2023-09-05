import { ProjectUserBillService } from './../../../../../../service/api/project-user-bill.service';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SubInvoice } from '@app/service/model/bill-info.model';

@Component({
  selector: 'app-add-sub-invoice-dialog',
  templateUrl: './add-sub-invoice-dialog.component.html',
  styleUrls: ['./add-sub-invoice-dialog.component.css']
})
export class AddSubInvoiceDialogComponent extends AppComponentBase implements OnInit {
  projectId: number;
  subInvoices: SubInvoice[];
  isSaving: boolean = false;
  subInvoiceCanAdd: SubInvoice[] = [];
  selectedSubInvoiceId: number;

  constructor(injector: Injector, public bsModalRef: BsModalRef, private projectUserBill: ProjectUserBillService) {
    super(injector);
  }

  ngOnInit(): void {
    this.setSubInvoiceCanAdd();
  }

  addSubInvoice(): void{
    console.log(this.selectedSubInvoiceId);
    this.projectUserBill.addSubInvoice(
      {parentInvoiceId: this.projectId, subInvoiceId: this.selectedSubInvoiceId} as AddSubInvoiceModel
    )
    .subscribe(response => {
      if(!response.success) return;
      abp.notify.success(response.result);
      this.setInvoices();
      this.setSubInvoiceCanAdd();
    });
  }

  private setInvoices(): void{
    this.projectUserBill.getSubInvoiceByProjectId(this.projectId)
    .subscribe(response => {
      if(!response.success) return;
      this.subInvoices = response.result;
    });
  }

  private setSubInvoiceCanAdd(): void{
    this.projectUserBill.getAllProjectCanUsing(this.projectId)
    .subscribe(response => {
      if(!response.success) return;
      this.subInvoiceCanAdd = response.result;
    })
  }
}
export class AddSubInvoiceModel{
  parentInvoiceId: number;
  subInvoiceId: number;
}
