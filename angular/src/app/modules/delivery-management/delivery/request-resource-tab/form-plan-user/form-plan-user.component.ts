import { ResourcePlanDto } from './../../../../../service/model/resource-plan.dto';
import { result } from 'lodash-es';
import { ChangeDetectorRef, Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';
import { RequestResourceDto } from '@app/service/model/delivery-management.dto';
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from 'moment';
import { UserService } from '@app/service/api/user.service';

@Component({
  selector: 'app-form-plan-user',
  templateUrl: './form-plan-user.component.html',
  styleUrls: ['./form-plan-user.component.css']
})
export class FormPlanUserComponent extends AppComponentBase implements OnInit {
  public search: string = '';
  public listUsers: any[] = [];
  public listProjectUserRoles: any[] = []
  public resourcePlan = {} as ResourcePlanDto
  public timeJoin: any;
  public typePlan: string = 'create';
  constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public input: any,
    public dialogRef: MatDialogRef<FormPlanUserComponent>,
    private resourceRequestService: DeliveryResourceRequestService,
    private ref: ChangeDetectorRef,
    private _userService: UserService
  ) 
  {
    super(injector);
  }

  ngOnInit(): void {
    this.resourcePlan = this.input;
    this.timeJoin = this.resourcePlan.startTime
    this.listProjectUserRoles = this.input.projectUserRoles
    this.listUsers = this.input.listActiveUsers
    if(this.resourcePlan.userId){
      this.typePlan = 'update';
    }
  }

  ngAfterViewChecked(): void {
    //Called after every check of the component's view. Applies to components only.
    //Add 'implements AfterViewChecked' to the class.
    this.ref.detectChanges()
  }

  SaveAndClose(){
    this.resourcePlan.startTime = this.timeJoin
    this.resourcePlan.startTime = moment(this.resourcePlan.startTime).format('YYYY/MM/DD')
    let data = {
      resourceRequestId: this.resourcePlan.resourceRequestId,
      result: null
    }
    if(this.typePlan == 'create'){
      this.resourceRequestService.createPlanUser(this.resourcePlan).subscribe(res => {
        if(res.success){
          abp.notify.success("Plan Success")
          data.result = res.result
          this.dialogRef.close({ type: 'create', data})
        }
        else{
          abp.notify.error(res.result)
        }
      })
    }
    else{
      this.resourceRequestService.updatePlanUser(this.resourcePlan).subscribe(res => {
        if(res.success){
          abp.notify.success("Update successfully")
          data.result = res.result
          this.dialogRef.close({ type: 'update', data})
        }
        else{
          abp.notify.error(res.result)
        }
      })
    }
  }

  cancel(){
    this.dialogRef.close()
  }

  getStyleStatusUser(isActive: boolean){
    return isActive?"badge badge-pill badge-success":"badge badge-pill badge-danger"
  }

  getValueStatusUser(isActive: boolean){
    return isActive?"Active":"InActive"
  }
}
