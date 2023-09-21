import { catchError } from "rxjs/operators";
import { AppComponentBase } from "@shared/app-component-base";
import { Component, OnInit, Injector, Inject } from "@angular/core";
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
      billAccountId: this.data.userId,
      projectId: this.data.projectId,
      userIds: this.listResourceSelect.filter(item => !this.listResourceSelectCurrent.includes(item))
    }

    const reqDelete = {
      billAccountId: this.data.userId,
      projectId: this.data.projectId,
      userIds: this.listResourceSelectCurrent.filter(item => !this.listResourceSelect.includes(item))
    }

      forkJoin(this.projectUserBillService.LinkUserToBillAccount(reqAdd),this.projectUserBillService.RemoveUserFromBillAccount(reqDelete))
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