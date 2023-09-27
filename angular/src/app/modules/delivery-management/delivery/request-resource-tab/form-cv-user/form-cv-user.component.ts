import { ChangeDetectorRef, Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';
import * as moment from 'moment';
import { FormPlanUserComponent } from '../form-plan-user/form-plan-user.component';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';

@Component({
  selector: 'app-form-cv-user',
  templateUrl: './form-cv-user.component.html',
  styleUrls: ['./form-cv-user.component.css']
})
export class FormCvUserComponent extends AppComponentBase implements OnInit {

  public search: string = '';
  public listUsers: any[] = [];
  public billInfoPlan:any
  public timeJoin: any;
  public typePlan: string = 'create';
  constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public input: any,
    public dialogRef: MatDialogRef<FormPlanUserComponent>,
    private resourceRequestService: DeliveryResourceRequestService,
    private ref: ChangeDetectorRef,
    private projectUserBillService: ProjectUserBillService
    )
  {
    super(injector);
  }

  ngOnInit(): void {
    console.log(this.input)
    this.billInfoPlan = {startTime: this.input.billUserInfo ? this.input.billUserInfo.plannedDate : '', resourceRequestId: this.input.id, userId : this.input.billUserInfo ? this.input.billUserInfo.employee.id : undefined}
    this.timeJoin = this.billInfoPlan.startTime;
    if(this.billInfoPlan.userId){
      this.typePlan = 'update'
    }
    this.getAllUser()
  }

  ngAfterViewChecked(): void {
    //Called after every check of the component's view. Applies to components only.
    //Add 'implements AfterViewChecked' to the class.
    this.ref.detectChanges()
  }


  getAllUser(){
    let unassigned = {
      id: -1,
      fullName: 'Unassigned',
      emailAddress: ''
    }
    this.projectUserBillService.GetAllUserActive(this.input.projectId, '', false, true).subscribe(res => {
      this.listUsers = res.result
      if(this.typePlan == 'update'){
        this.listUsers.unshift(unassigned)
      }
    })
  }

  SaveAndClose(){
    this.billInfoPlan.startTime = this.timeJoin
    this.billInfoPlan.startTime = moment(this.billInfoPlan.startTime).format('YYYY/MM/DD')
    if(this.typePlan == 'create'){
      this.resourceRequestService.UpdateBillInfoPlan(this.billInfoPlan).subscribe((res:any) => {
        if(res.success){
          abp.notify.success("Plan Success")
          this.dialogRef.close(true)
        }
        else{
          abp.notify.error(res.result)
        }
      })
    }
    else{
      if(this.billInfoPlan.userId == -1){
          let billInfoPlan = {
            startTime:this.billInfoPlan.startTime,
            userId:null,
            resourceRequestId:this.billInfoPlan.resourceRequestId
          }
        this.resourceRequestService.UpdateBillInfoPlan(billInfoPlan).subscribe((res:any) => {
          if(res.success){
            abp.notify.success("Plan successfully")
            this.dialogRef.close(true)
          }
          else{
            abp.notify.error(res.result)
          }
        })
      }
      else{
        this.resourceRequestService.UpdateBillInfoPlan(this.billInfoPlan).subscribe((res:any) => {
          if(res.success){
            abp.notify.success("Update successfully")
            this.dialogRef.close(true)
          }
          else{
            abp.notify.error(res.result)
          }
        })
      }
    }
  }

  cancel(){
    this.dialogRef.close()
  }

}
