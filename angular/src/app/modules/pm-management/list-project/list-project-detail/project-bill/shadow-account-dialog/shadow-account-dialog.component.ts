import { catchError } from "rxjs/operators";
import { AppComponentBase } from "@shared/app-component-base";
import { Component, OnInit, Injector, Inject, ViewChild } from "@angular/core";
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
  listResourceSelected=[]
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
  @ViewChild("select") select;
  ngOnInit() { 
    this.listResourceSelect = this.data.listResource
    this.listResourceSelected = [...this.data.listResource]
    this.listResourceSelectCurrent = [...this.listResourceSelect]
    this.projectUserBillService.GetAllResource().subscribe(res=> {
      this.listAllResource = res.result;
      this.resourceSelected = this.listAllResource.filter(item => this.listResourceSelect.includes(item.userId))
    })
  }
  openedChange(event){
    if(!event){
      this.searchUser = ''
    }
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
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
  }
  save(){
    const reqAdd = {
      billAccountId: this.data.userId,
      projectId: this.data.projectId,
      userIds: this.listResourceSelect.filter(item => !this.listResourceSelected.includes(item))
    }

    const reqDelete = {
      billAccountId: this.data.userId,
      projectId: this.data.projectId,
      userIds: this.listResourceSelected.filter(item => !this.listResourceSelect.includes(item))
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
    this.listResourceSelectCurrent = [];
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
  }
  selectAll(){
    this.listResourceSelect = this.listAllResource.map(item => item.userId)
    this.listResourceSelectCurrent = [... this.listResourceSelect]
    this.resourceSelected = this.listAllResource.filter(item =>  this.listResourceSelect.includes(item.userId))
  }
}