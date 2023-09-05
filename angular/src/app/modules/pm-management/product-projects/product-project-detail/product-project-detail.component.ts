import { AppComponentBase } from '@shared/app-component-base';
import { PERMISSIONS_CONSTANT } from './../../../../constant/permission.constant';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-product-project-detail',
  templateUrl: './product-project-detail.component.html',
  styleUrls: ['./product-project-detail.component.css']
})
export class ProductProjectDetailComponent extends AppComponentBase implements OnInit {

  Projects_ProductProjects_ProjectDetail_TabGeneral=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabGeneral;
  Projects_ProductProjects_ProjectDetail_TabResourceManagement=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabResourceManagement;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View;

  Projects_ProductProjects_ProjectDetail_TabBillInfo=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabBillInfo;
  Projects_ProductProjects_ProjectDetail_TabProjectDescription=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectDescription;
  Projects_ProductProjects_ProjectDetail_TabProjectFile=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectFile;
  Projects_ProductProjects_ProjectDetail_TabTimesheet=PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabTimesheet;

  public currentUrl: string= '';
  requestId: string = "";
  projectName:string;
  projectCode:string;
  constructor(public router : Router,
    public injector : Injector,
    private route: ActivatedRoute) {super(injector) }

  ngOnInit(): void {
    this.currentUrl =this.router.url
    this.router.events.subscribe(res => this.currentUrl = this.router.url)
    this.requestId = this.route.snapshot.queryParamMap.get("id");
    this.projectName = this.route.snapshot.queryParamMap.get("projectName");
    this.projectCode = this.route.snapshot.queryParamMap.get("projectCode");
 
  }
  public routingGeneralTab(){
    this.router.navigate(['product-project-general'],{
      relativeTo:this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
    })
  }

  public routingResourceTab() {
    this.router.navigate(['product-resource-management'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
    
  }

 
  public routingMilestoneTab() {
    this.router.navigate(['product-milestone'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingWeeklyReportTab(){
    this.router.navigate(['product-weekly-report'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingProjectChecklistTab(){
    this.router.navigate(['product-project-checklist'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  public routingTimesheetTab(){
    this.router.navigate(['product-timesheet-tab'], {
      relativeTo: this.route, queryParams: {
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      },
      // replaceUrl: true
    })
  }
  
  public routingDescriptionTab(){
    this.router.navigate(['product-description-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }
  public routingFileTab(){
    this.router.navigate(['product-project-file-tab'],{
      relativeTo: this.route, queryParams:{
        id:this.requestId,
        projectName: this.projectName,
        projectCode:this.projectCode
      }
    })
  }


}
