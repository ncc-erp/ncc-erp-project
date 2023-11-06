import { ChangeDetectorRef, Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';
import * as moment from 'moment';
import { FormPlanUserComponent } from '../form-plan-user/form-plan-user.component';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { ResourcePlanDto } from './../../../../../service/model/resource-plan.dto';
import { concat, forkJoin } from 'rxjs';
import { catchError, startWith } from 'rxjs/operators';

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
  public resourcePlan = {} as ResourcePlanDto
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
    this.billInfoPlan = {startTime: this.input.item.billUserInfo ? this.input.item.billUserInfo.plannedDate : '',
                         resourceRequestId: this.input.item.id,
                         userId : this.input.item.billUserInfo ? this.input.item.billUserInfo.employee.id : undefined,
                         isHasResource: this.input.isHasResource,
                         };
    this.resourcePlan = this.input.planUser;

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
    this.projectUserBillService.GetAllUserActive(this.input.item.projectId, '', false, true).subscribe(res => {
      this.listUsers = res.result
      if(this.typePlan == 'update'){
        this.listUsers.unshift(unassigned)
      }
    })
  }

  SaveAndClose(){
    this.billInfoPlan.startTime = this.timeJoin;
    if (this.billInfoPlan.startTime) {
      this.billInfoPlan.startTime = moment(this.billInfoPlan.startTime).format('YYYY/MM/DD');
    }

    const currentDate = moment().format('YYYY/MM/DD');
    this.resourcePlan.userId = this.billInfoPlan.userId;
    this.resourcePlan.projectUserId = this.billInfoPlan.projectUserId;
    this.resourcePlan.projectRole = 1; //Set default role for Resource as a Dev

    if (this.billInfoPlan.startTime == null  || !moment(this.billInfoPlan.startTime).isValid()){
      this.resourcePlan.startTime = currentDate;
    } else {
      this.resourcePlan.startTime = this.billInfoPlan.startTime;
    }

    if (this.typePlan === 'create') {
      const updateBillInfoPlan = this.resourceRequestService.UpdateBillInfoPlan(this.billInfoPlan);
      const createPlanUser = this.resourceRequestService.createPlanUser(this.resourcePlan);
      const actions = this.billInfoPlan.isHasResource ? [updateBillInfoPlan] : [updateBillInfoPlan, createPlanUser];

      concat(...actions)
        .pipe(catchError(this.resourceRequestService.handleError))
        .subscribe(() => {
          if (this.billInfoPlan.isHasResource) {
            abp.notify.success('Create Bill Account successfully!');
          } else {
            abp.notify.success('Create Bill Account and Plan User successfully!');
          }
          this.dialogRef.close(true);
        });
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
