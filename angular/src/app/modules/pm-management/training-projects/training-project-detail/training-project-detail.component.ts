import { AppComponentBase } from '@shared/app-component-base';
import { PERMISSIONS_CONSTANT } from './../../../../constant/permission.constant';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-training-project-detail',
  templateUrl: './training-project-detail.component.html',
  styleUrls: ['./training-project-detail.component.css']
})
export class TrainingProjectDetailComponent extends AppComponentBase implements OnInit {
  Projects_TrainingProjects_ProjectDetail_TabGeneral=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabGeneral;
  Projects_TrainingProjects_ProjectDetail_TabResourceManagement=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabResourceManagement;
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport;
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_TrainingProjects_ProjectDetail_TabBillInfo=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabBillInfo;
  Projects_TrainingProjects_ProjectDetail_TabProjectDescription=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabProjectDescription;
  Projects_TrainingProjects_ProjectDetail_TabProjectFile=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabProjectFile;
  Projects_TrainingProjects_ProjectDetail_TabTimesheet=PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabTimesheet;
  currentUrl: string = "";
  requestId: string = "";
  projectName:string;
  projectCode:string;
  constructor(public router : Router,
    public injector : Injector,
    private route: ActivatedRoute) { super(injector)}

  ngOnInit(): void {
    this.currentUrl =this.router.url
    this.router.events.subscribe(res => this.currentUrl = this.router.url)
    this.requestId = this.route.snapshot.queryParamMap.get("id");
    this.projectName = this.route.snapshot.queryParamMap.get("projectName");
    this.projectCode = this.route.snapshot.queryParamMap.get("projectCode");
  }
  public routingGeneralTab(){
    this.router.navigate(['training-project-general'],{
      relativeTo:this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
    })
  }

  public routingResourceTab() {
    this.router.navigate(['training-resource-management'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
    
  }

 
  public routingMilestoneTab() {
    this.router.navigate(['training-milestone'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingWeeklyReportTab(){
    this.router.navigate(['training-weekly-report'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingProjectChecklistTab(){
    this.router.navigate(['training-project-checklist'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingTimesheetTab(){
    this.router.navigate(['training-timesheet-tab'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  
  public routingDescriptionTab(){
    this.router.navigate(['training-description-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }
  public routingFileTab(){
    this.router.navigate(['training-project-file-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }

}
