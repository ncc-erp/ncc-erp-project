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
  public cvName: string;
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
    this.billInfoPlan = {startTime: this.input.billUserInfo ? this.input.billUserInfo.plannedDate : '',
                         resourceRequestId: this.input.resourceRequestId,
                         userId : this.input.billUserInfo ? this.input.billUserInfo.employee.id : undefined,
                         cvName: this.input.cvName,
                         };

    this.timeJoin = this.billInfoPlan.startTime;
    this.listUsers = this.input.listUsers;
    this.cvName = this.input.cvName;
  }

  ngAfterViewChecked(): void {
    //Called after every check of the component's view. Applies to components only.
    //Add 'implements AfterViewChecked' to the class.
    this.ref.detectChanges()
  }

  SaveAndClose(){
    this.billInfoPlan.startTime = this.timeJoin;
    if (this.billInfoPlan.startTime) {
      this.billInfoPlan.startTime = moment(this.billInfoPlan.startTime).format('YYYY/MM/DD');
      }
    this.billInfoPlan.cvName = this.cvName;
    this.resourceRequestService.UpdateBillInfoPlan(this.billInfoPlan).subscribe((res:any) => {
      if(res.success){
        abp.notify.success("Update successfully")
        this.dialogRef.close({ type: 'update', data: res.result})
      }
      else{
        abp.notify.error(res.result)
      }
    })
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
