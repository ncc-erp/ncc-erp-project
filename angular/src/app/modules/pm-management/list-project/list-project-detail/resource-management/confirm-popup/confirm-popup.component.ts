import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector } from '@angular/core';
import * as moment from 'moment';
import { PMReportProjectService } from '@app/service/api/pmreport-project.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';

@Component({
  selector: 'app-confirm-popup',
  templateUrl: './confirm-popup.component.html',
  styleUrls: ['./confirm-popup.component.css']
})
export class ConfirmPopupComponent extends AppComponentBase implements OnInit {

  Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject 
  = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject
  Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject 
  = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject
  Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject 
  = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject


  Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther
  
  WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther
  
  Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther

  Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther 
  = PERMISSIONS_CONSTANT.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther

  
  public startDate
  public user
  public title: string = ""
  public confirmMessage: string = ""
  public allowConfirm:boolean  =true
  public workingProject: any[] = []
 
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector:Injector,
    private projectUserService: ProjectUserService, private pmReportProService:PMReportProjectService,
     private dialogRef: MatDialogRef<ConfirmPopupComponent>) {
       super(injector)
      }

  ngOnInit(): void {
    this.user = this.data.user
    console.log("user",this.user)
    this.startDate = moment(this.data.user.startTime).format("YYYY-MM-DD")
    if (this.data.type === 'confirmJoin') {
      this.title = `Confirm user: <strong>${this.user.fullName}</strong> <strong class ="text-success">join project</strong>`
    }
    else if (this.data.type === 'confirmOut') {
      this.title = `Confirm user: <strong>${this.user.fullName}</strong> <strong class="text-success">Out project</strong>`
    }
    else{
      this.title = `Add user: <strong>${this.user.fullName}</strong> to project`
    }
    this.workingProject = this.data.workingProject
    this.checkPermissionForEachPage()
  }
  confirm() {
    if (this.data.workingProject.length > 0) {
      if (this.data.page == ConfirmFromPage.weeklyReport) {
        this.pmReportProService.ConfirmJoinProject(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.pmReportProService.handleError)).subscribe(rs => {
          abp.notify.success(`Confirmed for user ${this.user.fullName} join project`)
          this.dialogRef.close(true)
        })
      }
      else if(this.data.type == 'confirmJoin'){
        this.confirmJoin()
      }
      else{
        this.dialogRef.close(true)
      }
    }
    else {
      abp.message.confirm(`Confirm user <strong>${this.user.fullName}</strong> <strong class="text-success">join</strong> Project`, "", rs => {
        if (rs) {

          if (this.data.page == ConfirmFromPage.weeklyReport) {
            this.pmReportProService.ConfirmJoinProject(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.pmReportProService.handleError)).subscribe(rs => {
              this.dialogRef.close(true)
              abp.notify.success(`Confirmed for user ${this.user.fullName} join project`)
            })
          }
          else {
            this.confirmJoin()
          }
        }
      }, {isHtml:true})
    }
  }
  
  confirmJoinSuccessResult(){
    abp.notify.success(`Confirmed for user ${this.user.fullName} join project`)
    this.dialogRef.close(true)
  }
  confirmJoin(){
    if(this.data.page == ConfirmFromPage.outsource_workingResource 
      || this.data.page == ConfirmFromPage.outsource_PlannedResource
      || this.data.page == ConfirmFromPage.outsource_weekly ){
      this.projectUserService.ConfirmJoinProjectOutsourcing(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.projectUserService.handleError)).subscribe(rs => {
        this.confirmJoinSuccessResult()
      })
    }
    if(this.data.page == ConfirmFromPage.product_workingResource
      || this.data.page == ConfirmFromPage.product_PlannedResource
      || this.data.page == ConfirmFromPage.product_weekly){
      this.projectUserService.ConfirmJoinProjectProduct(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.projectUserService.handleError)).subscribe(rs => {
        this.confirmJoinSuccessResult()
      })
    }
    if(this.data.page == ConfirmFromPage.training_workingResource
      || this.data.page == ConfirmFromPage.training_PlannedResource
      || this.data.page == ConfirmFromPage.training_weekly){
      this.projectUserService.ConfirmJoinProjectTraining(this.user.id, moment(this.startDate).format("YYYY-MM-DD")).pipe(catchError(this.projectUserService.handleError)).subscribe(rs => {
        this.confirmJoinSuccessResult()
      })
    }
  }

  checkPermissionForEachPage(){
    if(this.data.page == ConfirmFromPage.outsource_workingResource){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject))
    }
    if(this.data.page == ConfirmFromPage.product_workingResource){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject))
    }
    if(this.data.page == ConfirmFromPage.training_workingResource){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject))
    }


    if(this.data.page == ConfirmFromPage.outsource_PlannedResource){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
    if(this.data.page == ConfirmFromPage.outsource_weekly){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
    if(this.data.page == ConfirmFromPage.product_PlannedResource){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
    if(this.data.page == ConfirmFromPage.product_weekly){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
    if(this.data.page == ConfirmFromPage.training_PlannedResource){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
    if(this.data.page == ConfirmFromPage.training_weekly){
      this.checkConfirmPermission(this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
    if(this.data.page == ConfirmFromPage.weeklyReport){
      this.checkConfirmPermission(this.permission.isGranted(this.WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther))
    }
   
  }

  checkConfirmPermission(confirmMovePermisson) {
    console.log("haaaaaa",confirmMovePermisson)
    console.log("haaaaaa",this.data.workingProject)
    if (!confirmMovePermisson) {
      this.data.workingProject.forEach(pu => {
        if (pu.isPool == false) {
          this.allowConfirm = false
          return 
        }
      }
      )
    }
  }
}
export const ConfirmFromPage = {
  outsource_workingResource: 1,
  outsource_PlannedResource: 2,
  outsource_weekly: 3,

  product_workingResource: 4,
  product_PlannedResource: 5,
  product_weekly: 6,

  training_workingResource: 7,
  training_PlannedResource: 8,
  training_weekly: 9,

  weeklyReport: 10,
  poolResource: 11,
  allResource:12,
  vendor:13
}
