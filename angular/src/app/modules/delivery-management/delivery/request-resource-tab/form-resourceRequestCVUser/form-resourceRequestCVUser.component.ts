import { ChangeDetectorRef, Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';
import * as moment from 'moment';
import { NgModule } from '@angular/core';
import { FormPlanUserComponent } from '../form-plan-user/form-plan-user.component';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { ResourcePlanDto } from './../../../../../service/model/resource-plan.dto';
import { concat, forkJoin } from 'rxjs';
import { catchError, startWith } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-form-resourceRequestCVUser',
  templateUrl: './form-resourceRequestCVUser.component.html',
  styleUrls: ['./form-resourceRequestCVUser.component.css']
})
export class FormResourceRequestCVUserComponent extends AppComponentBase implements OnInit {

  public search: string = '';
  public listUsers: any[] = [];
  public billInfoPlan:any
  public cvName: string;
  public resourceRequestCV : any;
  constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public input: any,
    public dialogRef: MatDialogRef<FormResourceRequestCVUserComponent>,
    private resourceRequestService: DeliveryResourceRequestService,
    private ref: ChangeDetectorRef,
    private projectUserBillService: ProjectUserBillService
    )
  {
    super(injector);
  }

  ngOnInit(): void {
    this.billInfoPlan = {
                         resourceRequestId: this.input.resourceRequestId,
                         userId : this.input.billUserInfo ? this.input.resourceRequestCV.userId : undefined,
                         cvName: this.input.cvName,
                         };

    this.resourceRequestCV = this.input.resourceRequestCV;
    this.listUsers = this.input.listUsers;
    this.cvName = this.input.cvName;
  }

  ngAfterViewChecked(): void {
    //Called after every check of the component's view. Applies to components only.
    //Add 'implements AfterViewChecked' to the class.
    this.ref.detectChanges()
  }

  SaveAndClose(){
   
    this.billInfoPlan.cvName = this.cvName;
    const input = {...this.resourceRequestCV,cvName: this.cvName,userId : this.billInfoPlan.userId };
    this.resourceRequestService.updateResourceRequestCV(input).subscribe((res:any) => {
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
