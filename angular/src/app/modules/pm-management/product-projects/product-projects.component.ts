import { MatMenuTrigger } from '@angular/material/menu';
import { AppSessionService } from './../../../../shared/session/app-session.service';
import { PERMISSIONS_CONSTANT } from './../../../constant/permission.constant';
import { UserService } from './../../../service/api/user.service';
import { InputFilterDto } from './../../../../shared/filter/filter.component';
import { ListProjectService } from './../../../service/api/list-project.service';
import { Router } from '@angular/router';
import { catchError, finalize } from 'rxjs/operators';
import { CreateEditProductProjectComponent } from './create-edit-product-project/create-edit-product-project.component';
import { MatDialog } from '@angular/material/dialog';
import { ProductProjectDto, ProjectDto } from './../../../service/model/project.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-product-projects',
  templateUrl: './product-projects.component.html',
  styleUrls: ['./product-projects.component.css']
})
export class ProductProjectsComponent extends PagedListingComponentBase<any> implements OnInit {
  Projects_ProductProjects_ViewAllProject = PERMISSIONS_CONSTANT.Projects_ProductProjects_ViewAllProject;
  Projects_ProductProjects_ViewMyProjectOnly = PERMISSIONS_CONSTANT.Projects_ProductProjects_ViewMyProjectOnly;
  Projects_ProductProjects_Create = PERMISSIONS_CONSTANT.Projects_ProductProjects_Create;
  Projects_ProductProjects_Edit = PERMISSIONS_CONSTANT.Projects_ProductProjects_Edit;
  Projects_ProductProjects_Delete = PERMISSIONS_CONSTANT.Projects_ProductProjects_Delete;
  Projects_ProductProjects_Close = PERMISSIONS_CONSTANT.Projects_ProductProjects_Close;
  Projects_ProductProjects_ProjectDetail = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
  Projects_ProductProjects_ViewRequireWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ViewRequireWeeklyReport
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Tên dự án", },
    { propertyName: 'dateSendReport', comparisions: [0, 1, 2, 3, 4], displayName: "Thời gian gửi report", filterType: 1 },
    // { propertyName: 'status', comparisions: [0], displayName: "Trạng thái", filterType: 3, dropdownData: this.statusFilterList },
    { propertyName: 'isSent', comparisions: [0], displayName: "Đã gửi weekly", filterType: 2 },
    { propertyName: 'startTime', comparisions: [0, 1, 2, 3, 4], displayName: "Thời gian bắt đầu", filterType: 1 },
    { propertyName: 'endTime', comparisions: [0, 1, 2, 3, 4], displayName: "Thời gian kết thúc", filterType: 1 },
  ];

  public sortWeeklyReport: number = 0;
  public pmId =  -1;
  public searchPM: string = "";
  statusFilterList = [{ displayName: "Not Closed", value: 3 },
  { displayName: "InProgress", value: 1 }, { displayName: "Potential", value: 0 },
  { displayName: "Closed", value: 2 },
  ]

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    let check = false
    let checkFilterPM = false;

    if(this.sortWeeklyReport) {
      request.sort = 'timeSendReport';
      request.sortDirection = this.sortWeeklyReport - 1;
    }

    request.filterItems.forEach(item => {
      if (item.propertyName == "status") {
        check = true
        item.value = this.projectStatus
      }
      if (item.propertyName == "pmId") {
        checkFilterPM = true;
        item.value = this.pmId;
      }
    })
    if (check == false) {
      request.filterItems = this.AddFilterItem(request, "status", this.projectStatus)
    }
    if (!checkFilterPM) {
      request.filterItems = this.AddFilterItem(request, "pmId", this.pmId)
    }

    if (this.projectStatus === -1) {
      request.filterItems = this.clearFilter(request, "status", "")
      check = true

    }
    if (this.pmId == -1) {
      request.filterItems = this.clearFilter(request, "pmId", "")
      checkFilterPM = true

    }
    if(this.permission.isGranted( this.Projects_ProductProjects_ViewMyProjectOnly) && !this.permission.isGranted(this.Projects_ProductProjects_ViewAllProject)){
      this.pmId = Number(this.sessionService.userId);
    }else{
      if(request.searchText){
        this.pmId = -1;
      }
    }

    this.projectService.GetAllProductPaging(request).pipe(finalize(() => {
      finishedCallback()
    })).subscribe(data => {
      this.listProductProjects = data.result.items;
      if (check == false) {
        request.filterItems = this.clearFilter(request, "status", "");
      }
      if (!checkFilterPM) {
        request.filterItems = this.clearFilter(request, "pmId", "");
      }
      this.showPaging(data.result, pageNumber);
    })
  }
  protected delete(project: any): void {
    abp.message.confirm(
      "Delete project: " + project.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.projectService.delete(project.id).pipe(catchError(this.projectService.handleError)).subscribe(() => {
            abp.notify.success("Deleted project: " + project.name);
            this.refresh()
          });
        }
      }
    );
  }
  public listProductProjects: ProductProjectDto[] = [];
  public projectStatus: any = 3;
  public pmList: any[] = [];
  @ViewChild(MatMenuTrigger)
  menu: MatMenuTrigger
  contextMenuPosition = { x: '0', y: '0' }
  constructor(public injector: Injector,
    public dialog: MatDialog,
    public router: Router,
    private userService: UserService,
    private projectService: ListProjectService,
    public sessionService: AppSessionService) {
    super(injector)
    this.pmId = this.sessionService.userId
  }

  ngOnInit(): void {
    if(!this.isEnablePMFilter()){
      this.pmId = Number(this.sessionService.userId);
    }
    this.refresh();
    this.getAllPM();
  }
  public searchInfoProject(){
    if (this.isEnablePMFilter() && this.searchText != ""){
      this.pmId = -1;
    }
    this.getDataPage(1);    
  }
  public isEnablePMFilter(){
    return this.permission.isGranted(this.Projects_ProductProjects_ViewAllProject)
  }

  public getAllPM(): void {
    this.projectService.GetProductPMs().pipe(catchError(this.userService.handleError))
      .subscribe(data => {
        this.pmList = data.result;
      })
  }
  createProject() {
    this.showDialogListProject('create');
  }
  editProject(project: ProjectDto) {
    this.showDialogListProject('edit', project);
  }
  showDialogListProject(command: string, item?: ProjectDto): void {
    let project = {} as ProjectDto
    if (command == 'edit') {
      project = {
        name: item.name,
        code: item.code,
        startTime: item.startTime,
        endTime: item.endTime,
        pmId: item.pmId,
        id: item.id,
        status: item.status,
        clientId: item.clientId,
        isRequiredWeeklyReport: item.isRequiredWeeklyReport
      }
    }
    const dialogRef = this.dialog.open(CreateEditProductProjectComponent, {
      data: {
        command: command,
        dialogData: project
      },
      width: '700px',
      maxHeight: '100vh',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }
  showActions(e) {
    e.preventDefault();
    this.contextMenuPosition.x = e.clientX + 'px';
    this.contextMenuPosition.y = e.clientY + 'px';
    this.menu.openMenu();
  }
  showDetail(id: any) {
    if (this.permission.isGranted(this.PmManager_Project_ViewDetail)) {
      this.router.navigate(['app/product-project-detail/product-project-general'], {
        queryParams: {
          id: id
        }
      })
    }

  }

  isReportLate(time: string | null) {
    if(!time) return false;
    const timeSendReport = moment(new Date(time))
    const penaltyTime = moment().day(2).hour(15).minute(0).second(0);
    return timeSendReport.isAfter(penaltyTime)
  }

  handleSortWeeklyReportClick () {
    this.sortWeeklyReport = (this.sortWeeklyReport + 1) % 3;
    this.refresh();
  }

  presentPUs(arr) {
    if(arr.length == 0){
      return "";
    }
    arr = arr.map((item) => {
      return `- User ${item.fullName}`;
    })
    return `<div style='text-align:left'> And release following resources: <br/> ${arr.join('<br/>')} </div>`
  }

  protected closeProject(project: any): void {
    let item = {
      id: project.id    
    }

    this.projectService.getAllWorkingUserFromProject(project.id).pipe(catchError(this.projectService.handleError)).subscribe((res) => {
      let message = this.presentPUs(res.result);
      abp.message.confirm(
        `${message}`,
        `Are your sure close project: ${project.name}?`,
        (result: boolean) => {
          if (result) {
            this.projectService.closeProject(item).pipe(catchError(this.projectService.handleError)).subscribe((res) => {
              abp.notify.success("Update status project: " + project.name);
              if(res.result == "update-only-project-tool"){
                abp.notify.success("Update status project: "+ project.name);
              }
              else if(res.result == null || res.result == ""){
                abp.message.success(`<p>Update status project name <b>${project.name}</b> in <b>PROJECT TOOL</b> successful!</p> 
                <p style='color:#28a745'>Update status project name <b>${project.name}</b> in <b>TIMESHEET TOOL</b> successful!</p>`, 
               'Update status project result',true);
              }
              else{
                abp.message.error(`<p>Update status project <b>${project.name}</b> in <b>PROJECT TOOL</b> successful!</p> 
                <p style='color:#dc3545'>${res.result}</p>`, 
                'Update status project result',true);
              }
              this.refresh()
            });
          }
        }, {isHtml:true}
      );
    });
  }

  viewProjectDetail(project){
    let routingToUrl:string = (this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport)
     && this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View))
    ? "/app/product-project-detail/product-weekly-report" : "/app/product-project-detail/product-project-general"
    const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], { queryParams: {
      id: project.id,
      type: project.projectType, 
      projectName: project.name, 
      projectCode: project.code} }));
      window.open(url, '_blank');
  }
  changeRequireWeeklyReport(item) {
    this.projectService.changeRequireWeeklyReport(item.id).subscribe((res) => {
      item.isRequiredWeeklyReport = res.result;
      abp.notify.success("Change require weekly report sucessful!")
    });
  }
}
