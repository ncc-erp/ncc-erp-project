import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppComponentBase } from 'shared/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-list-project-detail',
  templateUrl: './list-project-detail.component.html',
  styleUrls: ['./list-project-detail.component.css']
})
export class ListProjectDetailComponent extends AppComponentBase implements OnInit {
  requestId: any;
  projectType: any;
  projectName:string;
  projectCode:string
  currentUrl: string = "";
  Projects_OutsourcingProjects_ProjectDetail_TabGeneral=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabGeneral;
  Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo;
  Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription;
  Projects_OutsourcingProjects_ProjectDetail_TabProjectFile=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile;
  Projects_OutsourcingProjects_ProjectDetail_TabTimesheet=PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabTimesheet;
 

  constructor(private route: ActivatedRoute, private router: Router, injector:Injector) {
    super(injector)
  }

  ngOnInit(): void {
    this.currentUrl =this.router.url
    this.router.events.subscribe(res => this.currentUrl = this.router.url)
    this.requestId = this.route.snapshot.queryParamMap.get("id");
    this.projectType = this.route.snapshot.queryParamMap.get("type");
    this.projectName = this.route.snapshot.queryParamMap.get("projectName");
    this.projectCode = this.route.snapshot.queryParamMap.get("projectCode");
  }
  public routingGeneralTab(){
    this.router.navigate(['list-project-general'],{
      relativeTo:this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
    })
  }

  public routingResourceTab() {
    this.router.navigate(['resourcemanagement'], {
      relativeTo: this.route, queryParams: {
        id: this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
    
  }

 
  public routingMilestoneTab() {
    this.router.navigate(['milestone'], {
      relativeTo: this.route, queryParams: {
        id: this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingWeeklyReportTab(){
    this.router.navigate(['weeklyreport'], {
      relativeTo: this.route, queryParams: {
        id: this.requestId,
        type: this.projectType,
        projectName: this.projectName,
        projectCode:this.projectCode,
      },
      // replaceUrl: true
    })
  }
  public routingProjectChecklistTab(){
    this.router.navigate(['projectchecklist'], {
      relativeTo: this.route, queryParams: {
        id: this.requestId,
        type: this.projectType,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingTimesheetTab(){
    this.router.navigate(['timesheet-tab'], {
      relativeTo: this.route, queryParams: {
        id: this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  
  public routingDescriptionTab(){
    this.router.navigate(['description-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }
  public routingFileTab(){
    this.router.navigate(['project-file-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }
  public routingProjectBillTab(){
    this.router.navigate(['project-bill-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }

}
