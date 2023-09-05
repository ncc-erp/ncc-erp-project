import { ResourceManagerService } from './../../../../../../../service/api/resource-manager.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { catchError } from 'rxjs/operators';
import { DeliveryResourceRequestService } from './../../../../../../../service/api/delivery-request-resource.service';
import { AppComponentBase } from '@shared/app-component-base';
import { APP_ENUMS } from './../../../../../../../../shared/AppEnums';
import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ProjectUserService } from '@app/service/api/project-user.service';
import * as moment from 'moment';
import { ConfirmFromPage } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/confirm-popup/confirm-popup.component';

@Component({
  selector: 'app-confirm-plan-dialog',
  templateUrl: './confirm-plan-dialog.component.html',
  styleUrls: ['./confirm-plan-dialog.component.css']
})
export class ConfirmPlanDialogComponent extends AppComponentBase implements OnInit {

  Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther
  Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther
  Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther

  public allowConfirm: boolean = true
  public startDate
  public user
  public title: string = ""
  public confirmMessage: string = ""
  public projectRoleList = Object.keys(APP_ENUMS.ProjectUserRole);
  public workingProject: any[] = []
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private puService: ResourceManagerService,
    injector: Injector,
    private dialogRef: MatDialogRef<ConfirmPlanDialogComponent>) {
    super(injector)
  }

  ngOnInit(): void {
    this.user = this.data.user
    this.workingProject = this.data.workingProject
    this.startDate = moment(this.data.user.startTime).format("YYYY-MM-DD")
    if (this.data.user.allocatePercentage > 0) {
      this.title = `Confirm <strong>${this.user.fullName}</strong> <strong class ="text-success">join</strong> <strong>${this.user.projectName}</strong>`
      if(this.data.fromPage == ConfirmFromPage.poolResource){
        this.checkConfirmPermission(this.permission.isGranted(this.Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther))
      }
      if(this.data.fromPage == ConfirmFromPage.allResource){
        console.log("testttt", this.permission.isGranted(this.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
        this.checkConfirmPermission(this.permission.isGranted(this.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
      }
      if(this.data.fromPage == ConfirmFromPage.vendor){
        this.checkConfirmPermission(this.permission.isGranted(this.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther))
      }
    }
    else if (this.data.user.allocatePercentage <= 0) {
      this.title = `Confirm <strong>${this.user.fullName}</strong> <strong class="text-danger">Out</strong> <strong>${this.user.projectName}</strong>`
    }

  }
  confirm() {
    if (this.user.allocatePercentage > 0) {
      if(this.data.fromPage == ConfirmFromPage.poolResource){
        this.puService.ConfirmJoinProjectFromTabPool(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.puService.handleError)).subscribe(rs => {
          this.confirmJoinSuccessResult()
        })
      }

      if(this.data.fromPage == ConfirmFromPage.allResource){
        this.puService.ConfirmJoinProjectFromTabAllResource(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.puService.handleError)).subscribe(rs => {
          this.confirmJoinSuccessResult()
        })
      }

      if(this.data.fromPage == ConfirmFromPage.vendor){
        this.puService.ConfirmJoinProjectFromTabVendor(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.puService.handleError)).subscribe(rs => {
          this.confirmJoinSuccessResult()
        })
      }

      
    }
    else {
      let requestBody = {
        projectUserId: this.user.id,
        startTime: this.startDate
      }
      this.puService.ConfirmOutProject(requestBody).pipe(catchError(this.puService.handleError)).subscribe(rs => {
        abp.notify.success(`Confirmed for user ${this.user.fullName} out project`)
        this.dialogRef.close(true)
      })
    }
  }

  confirmJoinSuccessResult(){
    abp.notify.success(`Confirmed for user ${this.user.fullName} join project`)
    this.dialogRef.close(true)
  }

  checkConfirmPermission(hasMovePermission:boolean) {
    if (!hasMovePermission) {
      this.workingProject.forEach(pu => {
        if (!pu.isPool) {
          this.allowConfirm = false
          return
        }
      }
      )
    }
  }
}
