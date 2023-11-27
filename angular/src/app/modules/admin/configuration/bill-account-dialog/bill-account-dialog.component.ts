import { Component, Inject, Injector, OnInit } from '@angular/core';
import { catchError } from "rxjs/operators";
import { AppComponentBase } from "@shared/app-component-base";
import { ProjectUserBillService } from "@app/service/api/project-user-bill.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { forkJoin } from "rxjs";

@Component({
  selector: 'app-bill-account-dialog',
  templateUrl: './bill-account-dialog.component.html',
  styleUrls: ['./bill-account-dialog.component.css']
})
// map: select, filter : where
export class BillAccountDialogComponent extends AppComponentBase implements OnInit {

  public resourceSelected : any = [];
  public resourceUnSelected : any = [];
  public listResourceSelect : any= [];
  public listResourceSelectCurrent : any = [];
  public listAllResource: any = [];
  public searchUser='';

  constructor(injector: Injector,
    public dialogRef: MatDialogRef<BillAccountDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private projectUserBillService: ProjectUserBillService) {
      super(injector);
     }

   ngOnInit() {
    this.listResourceSelect = this.data.selectedIds;
    this.listResourceSelectCurrent = [...this.listResourceSelect];
     this.projectUserBillService.getAllBillAccount().subscribe((data) => {
      this.listAllResource = data.result;
      this.orderListResource();
    });
  }

  openedChange(event){
    if(!event){
      this.searchUser = ''
    }
  }

  selectAll(){
    this.listResourceSelect = this.listAllResource.map(item => item.userId)
    this.listResourceSelectCurrent = [... this.listResourceSelect]
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
  }

  clear(){
    this.listResourceSelect = [];
    this.listResourceSelectCurrent = [];
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
  }

  onSelectChange(id){
    if(this.listResourceSelectCurrent.includes(id)){
      this.listResourceSelectCurrent = this.listResourceSelectCurrent.filter(res => res != id)
      this.listResourceSelect = [...this.listResourceSelectCurrent]
    }
    else{
      this.listResourceSelectCurrent.push(id)
      this.listResourceSelect = [...this.listResourceSelectCurrent]
    }
    this.orderListResource()
  }

  orderListResource(){
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
    this.resourceUnSelected  = this.listAllResource.filter(item =>  !this.listResourceSelect.includes(item.userId))
    this.listAllResource = [...this.resourceSelected, ...this.resourceUnSelected]
  }

  save(){
      abp.notify.success("Add bill account successfully")
      this.dialogRef.close({
        updateBill: this.listResourceSelect,
        isSave:true
      })
  }
}
