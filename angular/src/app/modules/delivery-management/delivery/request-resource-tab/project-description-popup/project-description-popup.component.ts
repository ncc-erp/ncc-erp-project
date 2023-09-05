import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ListProjectService } from '@app/service/api/list-project.service';
import { ProjectdetailDto } from '@app/service/model/projectDetail.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { Subscription } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-project-description-popup',
  templateUrl: './project-description-popup.component.html',
  styleUrls: ['./project-description-popup.component.css']
})
export class ProjectDescriptionPopupComponent extends AppComponentBase implements OnInit {
  projectId
  projectName:string='';
  projectDetail={} as ProjectdetailDto;
  subscription: Subscription[] = [];

  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View
  
  constructor( private router: Router,
    injector: Injector,
    private projectService:ListProjectService, 
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<ProjectDescriptionPopupComponent>,
  )
  {
    super(injector);
  }
  ngOnInit(): void {
    this.projectId=this.data.projectId
    this.projectName=this.data.projectName
    this.getProjectDetail()
  }

  getProjectDetail(){
    this.projectService.getProjectDetail(this.projectId).pipe(catchError(this.projectService.handleError)).subscribe(data=>{
      this.projectDetail =data.result
    })
  }
  viewProjectDetail(){
    let routingToUrl: string = ''

    if( this.data.projectType == 5 ){
      routingToUrl = (this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport)
      && this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View))
     ? "/app/training-project-detail/training-weekly-report" : "/app/training-project-detail/training-project-general"
    } 

    else if ( this.data.projectType == 3){
      routingToUrl= (this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport)
     && this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View))
    ? "/app/product-project-detail/product-weekly-report" : "/app/product-project-detail/product-project-general"
    }

    else {
      routingToUrl = (this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport)
      && this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View))
     ? "/app/list-project-detail/weeklyreport" : "/app/list-project-detail/list-project-general"
    }
    const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], {
      queryParams: {
        id: this.data.projectId,
        type: this.data.projectType,
        projectName: this.data.projectName,
        projectCode: this.data.projectCode
      }
    }));
    window.open(url, '_blank');
  }
  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
