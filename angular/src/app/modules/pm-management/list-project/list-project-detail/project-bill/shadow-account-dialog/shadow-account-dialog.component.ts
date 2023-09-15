import { startWith, map, catchError } from "rxjs/operators";
import { ShadowAccountService } from "./../../../../../../service/api/shadow-account.service";
import { AppComponentBase } from "@shared/app-component-base";
import { Component, OnInit, Injector, Inject } from "@angular/core";
import { ResourceManagerService } from "@app/service/api/resource-manager.service";
import { ProjectUserBillService } from "@app/service/api/project-user-bill.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { forkJoin } from "rxjs";

@Component({
  selector: "app-shadow-account-dialog",
  templateUrl: "./shadow-account-dialog.component.html",
  styleUrls: ["./shadow-account-dialog.component.css"],
})
export class ShadowAccountDialogComponent
  extends AppComponentBase
  implements OnInit
{
  listAllResource = []
  projectId: number;
  userId: number;
  listResourceSelect=[]
  listResourceSelectCurrent=[]
  resourceSelected = [];
  searchUser='';
  constructor(
    injector: Injector,
    public dialogRef: MatDialogRef<ShadowAccountDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private shadowAccountService: ShadowAccountService,
    private resourceManagerService:ResourceManagerService,
    private projectUserBillService: ProjectUserBillService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.listResourceSelect = this.data.listResource
    this.listResourceSelectCurrent = this.listResourceSelect
    this.projectUserBillService.GetAllResource().subscribe(res=> {
      this.listAllResource = res.result;
      this.resourceSelected = this.listAllResource.filter(item => this.listResourceSelect.includes(item.userId))
    })
  }
  onSelectChange(){
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
  }
  save(){
    const reqAdd = {
      userId: this.data.userId,
      projectId: this.data.projectId,
      userBillAccountIds: this.listResourceSelect.filter(item => !this.listResourceSelectCurrent.includes(item))
    }

    const reqDelete = {
      userId: this.data.userId,
      projectId: this.data.projectId,
      userBillAccountIds: this.listResourceSelectCurrent.filter(item => !this.listResourceSelect.includes(item))
    }

      forkJoin(this.projectUserBillService.UpdateResource(reqAdd),this.projectUserBillService.RemoveBillAccountsFromUser(reqDelete))
      .pipe(catchError(this.projectUserBillService.handleError))
      .subscribe(([rsAdd,rsRemove])=>{
        if(rsAdd.result && rsRemove.result){
          abp.notify.success("Update successfully")
          this.dialogRef.close(true)
        }
      })  

  }
  clear(){
    this.listResourceSelect = [];
    this.onSelectChange()
  }
  selectAll(){
    this.listResourceSelect = this.listAllResource.map(item => item.userId)
    this.onSelectChange()
  }
}
