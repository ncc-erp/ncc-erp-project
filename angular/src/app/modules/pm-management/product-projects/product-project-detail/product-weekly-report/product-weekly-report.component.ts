import { PERMISSIONS_CONSTANT } from './../../../../../constant/permission.constant';
import { ProductApprovedDialogComponent } from './product-approved-dialog/product-approved-dialog.component';
import { ProjectInfoDto, projectUserDto } from './../../../../../service/model/project.dto';
import { catchError } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { PmReportIssueService } from './../../../../../service/api/pm-report-issue.service';
import { ProjectResourceRequestService } from './../../../../../service/api/project-resource-request.service';
import { ListProjectService } from './../../../../../service/api/list-project.service';
import { PmReportService } from './../../../../../service/api/pm-report.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from './../../../../../service/api/user.service';
import { PMReportProjectService } from './../../../../../service/api/pmreport-project.service';
import { ProjectUserService } from './../../../../../service/api/project-user.service';
import { UserDto } from './../../../../../../shared/service-proxies/service-proxies';
import { projectReportDto, projectProblemDto } from './../../../../../service/model/projectReport.dto';
import { pmReportDto, pmReportProjectDto } from './../../../../../service/model/pmReport.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { AddFutureResourceDialogComponent } from '@app/modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/add-future-resource-dialog/add-future-resource-dialog.component';
import { ConfirmFromPage, ConfirmPopupComponent } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/confirm-popup/confirm-popup.component';
import { ReleaseUserDialogComponent } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/release-user-dialog/release-user-dialog.component';
import { GetTimesheetWorkingComponent, WorkingTimeDto } from '@app/modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/get-timesheet-working/get-timesheet-working.component';
import { ApproveDialogComponent } from '@app/modules/pm-management/list-project/list-project-detail/weekly-report/approve-dialog/approve-dialog.component';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { LayoutStoreService } from '@shared/layout/layout-store.service';
import { MatMenuTrigger } from '@angular/material/menu';
import { RadioDropdownComponent } from '@shared/components/radio-dropdown/radio-dropdown.component';
import { WeeklyReportComponent } from '@app/modules/pm-management/list-project/list-project-detail/weekly-report/weekly-report.component';
import { PagedRequestDto } from '@shared/paged-listing-component-base';
import * as echarts from 'echarts';
import { forkJoin } from 'rxjs';
import { CriteriaService } from '@app/service/api/criteria.service';
import { ProjectCriteriaResultService } from '@app/service/api/project-criteria-result.service';
import { ProjectCriteriaDto } from '@app/service/model/criteria-category.dto';
import { ProjectCriteriaResultDto } from '@app/service/model/project-criteria-result.dto';
import { APP_ENUMS } from '@shared/AppEnums';
import { GuideLineDialogComponent } from '@app/modules/pm-management/list-project/list-project-detail/weekly-report/guide-line-dialog/guide-line-dialog/guide-line-dialog.component';

@Component({
  selector: 'app-product-weekly-report',
  templateUrl: './product-weekly-report.component.html',
  styleUrls: ['./product-weekly-report.component.css']
})
export class ProductWeeklyReportComponent extends AppComponentBase implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    // this.pmReportProjectService.GetAllByPmReport(this.pmReportId, request).pipe(finalize(()=>{
    //   finishedCallback();
    // }),catchError(this.pmReportProjectService.handleError)).subscribe((data)=>{
    //   this.pmReportProjectList=data.result.items;
    //   this.showPaging(data.result,pageNumber);
    // })
  }
  protected delete(entity: WeeklyReportComponent): void {
    throw new Error('Method not implemented.');
  }
  @ViewChild(RadioDropdownComponent) child: RadioDropdownComponent;
  @ViewChild("timmer") timmerCount;
  @ViewChild(MatMenuTrigger)
  menuRelease: MatMenuTrigger;
  contextMenuPosition = { x: '0px', y: '0px' };

  public itemPerPage: number = 20;
  public weeklyCurrentPage: number = 1;
  public futureCurrentPage: number = 1;
  public problemCurrentPage: number = 1;
  public searchText = "";
  public pmReportProjectList: pmReportProjectDto[] = [];
  public tempPmReportProjectList: pmReportProjectDto[] = [];
  public show: boolean = false;
  public pmReportProject = {} as pmReportProjectDto;
  public pmReportId: any;
  public isActive: boolean = true;
  public projectType = "";
  public weeklyReportList: projectReportDto[] = [];
  public futureReportList: projectReportDto[] = [];
  public problemList: projectProblemDto[] = [];
  public problemIssueList: string[] = Object.keys(this.APP_ENUM.ProjectHealth);
  public projectRoleList: string[] = Object.keys(this.APP_ENUM.ProjectUserRole);
  public issueStatusList: string[] = Object.keys(this.APP_ENUM.PMReportProjectIssueStatus)
  public projectHealthList: string[] =  Object.keys(this.APP_ENUM.ProjectHealth);
  pmReportList: pmReportDto[] = [];
  public activeReportId: number;
  public pmReportProjectId: number;
  public isEditWeeklyReport: boolean = false;
  public isEditFutureReport: boolean = false;
  public isEditProblem: boolean = false;
  public processFuture: boolean = false;
  public processProblem: boolean = false
  public processWeekly: boolean = false;
  public createdDate = new Date();
  public projectId: number;
  public projectIdReport: number;
  public isEditingNote: boolean = false;
  public isEditingAutomationNote:boolean = false
  public isShowProblemList: boolean = false;
  public isShowWeeklyList: boolean = false;
  public isShowFutureList: boolean = false;
  public projectInfo = {} as ProjectInfoDto
  public projectCurrentResource: any = []
  public mondayOf5weeksAgo: any
  public lastWeekSunday: any
  public tempResourceList: any[] = []
  public officalResourceList: any[] = []
  public selectedReport = {} as pmReportDto;
  public userList = [];
  totalNormalWorkingTime: number = 0;
  totalOverTime: number = 0;
  sidebarExpanded: boolean;
  isShowCurrentResource: boolean = true;
  searchUser: string = ""
  isTimmerCounting: boolean = false
  isStopCounting: boolean = false
  isRefresh: boolean = false
  isStart: boolean = false

  totalNormalWorkingTimeOfWeekly: number = 0;
  totalNormalWorkingTime1: number = 0;
  totalOverTime1: number = 0;
  totalNormalWorkingTimeOfWeekly1: number = 0;
    overTimeNoCharge1: number = 0;
    totalNormalWorkingTimeStandard: number = 0;
    totalNormalWorkingTimeStandard1: number = 0;
  public projectCurrentSupportUser: any = []

  public listCriteria: ProjectCriteriaDto[] = []
  public listCriteriaResult: ProjectCriteriaResultDto[] = []
  public bgFlag: string = ''
  public status: string = ''
  public processCriteria: boolean = false;
  public isShowActionPM: boolean;
  public isValidCriteria: boolean;

  public isSentReport: boolean;
  public searchPmReport: string = "";
  public projectHealth: any;
  allowSendReport: boolean = true;
  overTimeNoCharge:number = 0;
  public defaultStatus = this.APP_ENUM.PMReportProjectIssueStatus[this.issueStatusList[0]];
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport;

  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_AddNewIssue = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_Edit = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_Delete = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_SetDone = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone;

  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release;

  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan;

  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View;

  ProjectHealthCriteria_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View;
  ProjectHealthCriteria_ChangeStatus = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus;
  ProjectHealthCriteria_Edit = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit;

  constructor(public pmReportProjectService: PMReportProjectService,
    private tsProjectService: TimesheetProjectService,
    private reportIssueService: PmReportIssueService, private pmReportService: PmReportService,
    public route: ActivatedRoute,
    private router:Router,
    injector: Injector,
    private projectUserService: ProjectUserService,
    private userService: UserService,
    private dialog: MatDialog,
    private requestservice: ProjectResourceRequestService,
    private _layoutStore: LayoutStoreService,
    private reportService: PMReportProjectService,
    private pjCriteriaService: CriteriaService,
    private pjCriteriaResultService: ProjectCriteriaResultService,
  ) {
    super(injector);
    this.projectId = Number(route.snapshot.queryParamMap.get("id"));
    this.projectType = route.snapshot.queryParamMap.get("type");

      this.isShowActionPM = this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_Edit) ||
      this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_Delete) ||
      this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_SetDone) ||
      this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectIssue_AddNewIssue);
  }
  ngOnInit(): void {
    this.getAllPmReport();
    let currentDate = new Date()
    currentDate.setDate(currentDate.getDate() - (currentDate.getDay() + 6) % 7);
    currentDate.setDate(currentDate.getDate() - 7);


    this.mondayOf5weeksAgo = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
    this.mondayOf5weeksAgo = moment(this.mondayOf5weeksAgo.setDate(this.mondayOf5weeksAgo.getDate() - 28)).format("YYYY-MM-DD")
    this.lastWeekSunday = moment(new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 6)).format("YYYY-MM-DD");
      this.getUser();
      this._layoutStore.sidebarExpanded.subscribe((value) => {
        this.sidebarExpanded = value;
      });

  }
  public startTimmer() {
    if ((!this.isTimmerCounting && !this.isStopCounting) || this.isRefresh) {
      this.timmerCount.start()
      this.isTimmerCounting = true
      this.isStopCounting = false
      this.isRefresh = false
      this.isStart = true
    }
  }
  public stopTimmer() {
    this.timmerCount.stop()
    this.isTimmerCounting = false
    this.isStopCounting = true
    this.isRefresh = false

  }
  public refreshTimmer() {
    this.timmerCount.reset()
    this.isTimmerCounting = false
    // this.isStopCounting =true
    this.isRefresh = true
    this.isStart = false

  }
  public resumeTimmer() {
    this.timmerCount.resume()
    this.isTimmerCounting = true
    this.isStopCounting = false
    this.isRefresh = false



  }

  public getWeeklyReport() {
    this.pmReportProjectService.getChangesDuringWeek(this.projectId, this.selectedReport.reportId).pipe(catchError(this.pmReportProjectService.handleError)).subscribe(data => {
      this.weeklyReportList = data.result;
    })
  }

  public getAllPmReport() {


    this.pmReportProjectService.GetAllByProject(this.projectId).pipe(catchError(this.pmReportProjectService.handleError)).subscribe(data => {
      this.pmReportList = data.result;
      this.selectedReport = this.pmReportList.filter(item => item.isActive == true)[0];
      this.isSentReport = this.selectedReport.status == 'Draft' ? true : false
      this.allowSendReport = this.selectedReport.note == null || this.selectedReport.note == '' ? false : true;
      this.projectHealth = this.APP_ENUM.ProjectHealth[this.selectedReport.projectHealth]
      this.getAllCriteria();
      this.getProjectInfo();
      this.getWeeklyReport();
      this.getFuturereport();
      this.getProjectProblem();
      this.getChangedResource();
    })
  }

  public handleBGStatus(priority: number) {
    if (priority === APP_ENUMS.ProjectHealth.Red) {
      this.bgFlag = 'bg-danger';
      this.status = 'Red';
    }
    else if (priority === APP_ENUMS.ProjectHealth.Yellow) {
      this.bgFlag = 'bg-warning';
      this.status = 'Yellow';
    }
    else if (priority === APP_ENUMS.ProjectHealth.Green) {
      this.bgFlag = 'bg-success';
      this.status = 'Green';
    }
  }

  public getAllCriteria() {
    forkJoin([this.pjCriteriaService.getAll(), this.pjCriteriaResultService.getAllCriteriaResult(this.projectId, this.selectedReport.reportId)])
      .subscribe(([resCriteria, resCriteriaResult]) => {
        this.bgFlag = '';
        this.status = '';
        this.listCriteriaResult = []
        const listTmpCriteria = resCriteria.result as ProjectCriteriaDto[];
        const listTmpCriteriaResult = resCriteriaResult.result as ProjectCriteriaResultDto[];
        for (let i = 0; i < listTmpCriteria.length; i++) {
          const criteria = listTmpCriteria[i];
          const check = listTmpCriteriaResult.find(item => item.projectCriteriaId === listTmpCriteria[i].id);
          const itemCriteriaResult = {
            criteriaName: criteria.name,
            note: check?.note || '',
            status: check?.status ,
            editMode: false,
            projectCriteriaId: criteria.id,
            projectId: this.projectId,
            pmReportId: this.selectedReport?.reportId,
            id: check?.id,
            isActive: criteria.isActive,
            guideline: criteria.guideline
          } as ProjectCriteriaResultDto
          if (this.selectedReport.isActive == true) {
            if (itemCriteriaResult.id) {
              this.listCriteriaResult.push(itemCriteriaResult);
            }
            else if (itemCriteriaResult.isActive) {
              this.listCriteriaResult.push(itemCriteriaResult);
            }
          }
          else {
            if (itemCriteriaResult.id) {
              this.listCriteriaResult.push(itemCriteriaResult);
            }
          }
        }
    this.setTotalHealth();
    })
  }

  setTotalHealth() {
    let priority: number = 0;
    for (let i = 0; i < this.listCriteriaResult.length; i++) {
      let tmpPriority = 1;
      if (!this.listCriteriaResult[i].status) {
        tmpPriority = 100;
      }
      else if (this.listCriteriaResult[i].status === APP_ENUMS.ProjectHealth.Red) {
        tmpPriority = 3;
      }
      else if (this.listCriteriaResult[i].status === APP_ENUMS.ProjectHealth.Yellow) {
        tmpPriority = 2;
      }

      if (tmpPriority > priority) {
        priority = tmpPriority;
      }
    }
    const empty = this.listCriteriaResult.filter(x => x.note == '' || !x.status || x.editMode);
    empty.length > 0 ? this.isValidCriteria = false : this.isValidCriteria = true;
    this.handleBGStatus(priority);
  }

  public handleEditCriteriaResult(index: number) {
    this.listCriteriaResult[index].editMode = true;
    this.processCriteria = true;
  }

  public cancelEditCriteriaResult(index: number) {
    this.listCriteriaResult[index].editMode = false;
    this.processCriteria = false;
  }

  public handleChangeStatus(item: ProjectCriteriaResultDto) {
    if (!item.editMode) {
      item.pmReportId = this.selectedReport.reportId;
      if (item.id) {
        this.pjCriteriaResultService.update(item).subscribe(res => {
          abp.notify.success(`Change status ${item.criteriaName} successfully`);
          this.processCriteria = false;
          if (res.success === true) {
            item.status = res.result.status;
            item.note = res.result.note;
            item.editMode = false;
            this.setTotalHealth();
          }
        });
      }
      else {
        this.pjCriteriaResultService.create(item).subscribe(res => {
          abp.notify.success(`Change status ${item.criteriaName} successfully`);
          this.processCriteria = false;
          if (res.success === true) {
            item.id = res.result.id;
            item.note = res.result.note;
            item.status = res.result.status;
            item.editMode = false;
            this.setTotalHealth()
          }
        })
      }
    }
  }

  public saveCriteriaResult(item: ProjectCriteriaResultDto) {
    item.pmReportId = this.selectedReport.reportId;
    if (item.id) {
      this.pjCriteriaResultService.update(item).subscribe(res => {
        abp.notify.success(`Update ${item.criteriaName} successfully`);
        item.editMode = false;
        this.processCriteria = false;
        if (res.success === true) {
          item.status = res.result.status;
          item.note = res.result.note;
          item.editMode = false;
          this.setTotalHealth()
        }
      });
    }
    else {
      this.pjCriteriaResultService.create(item).subscribe(res => {
        abp.notify.success(`Update ${item.criteriaName} successfully`);
        item.editMode = false;
        this.processCriteria = false;
        if (res.success === true) {
          item.id = res.result.id;
          item.status = res.result.status;
          item.note = res.result.note;
          item.editMode = false;
          this.setTotalHealth()
        }
      })
    }
  }

  public sendWeeklyreport() {
    abp.message.confirm(
      `send report ${this.selectedReport.pmReportName}? `,
      "",
      (result: boolean) => {
        if (result) {
          this.reportService.sendReport(this.projectId, this.selectedReport.reportId,this.status).pipe(catchError(this.reportService.handleError)).subscribe(data => {
            abp.notify.success("Send report successful");
            this.getAllPmReport();
          })
        }
      }
    );
    }

  getProjectInfo() {
    this.isLoading = true;
    if (this.selectedReport.pmReportProjectId) {
      this.pmReportProjectService.GetInfoProject(this.selectedReport.pmReportProjectId).pipe(catchError(this.pmReportProjectService.handleError)).subscribe(data => {
        this.projectInfo = data.result

        this.isLoading = false;
        this.getDataForBillChart(this.projectInfo.projectCode)

        this.getCurrentResourceOfProject(this.projectInfo.projectCode);
        this.router.navigate(
          [],
          {
            relativeTo: this.route,
            queryParams: {
              name: this.projectInfo.projectName,
              client: this.projectInfo.clientName,
              pmName: this.projectInfo.pmName,
              pmReportProjectId: this.selectedReport.pmReportProjectId,
              projectHealth: this.projectHealth
            },
            queryParamsHandling: 'merge', // remove to replace all query params by provided
          });
      },
        () => { this.isLoading = false })
    }
  }

  public getChangedResource() {
    if (this.projectId) {
      this.pmReportProjectService.getChangesDuringWeek(this.projectId, this.selectedReport.reportId).pipe(catchError(this.pmReportProjectService.handleError)).subscribe(data => {
        this.weeklyReportList = data.result;
        this.isShowWeeklyList = this.weeklyReportList.length == 0 ? false : true;
      })
    }
  }
  public getFuturereport() {
    if (this.projectId) {
      this.pmReportProjectService.getChangesInFuture(this.projectId, this.selectedReport.reportId).pipe(catchError(this.pmReportProjectService.handleError)).subscribe(data => {
        this.futureReportList = data.result;
        this.isShowFutureList = this.futureReportList.length == 0 ? false : true;
      })
    }

  }
  public getProjectProblem() {
    if (this.projectId) {
      this.pmReportProjectService.problemsOfTheWeekForReport(this.projectId, this.selectedReport.reportId).pipe(catchError(this.reportIssueService.handleError)).subscribe(data => {
        if (data.result) {
          this.problemList = [];
          for (let i = 0; i < data.result.result.length; i++) {
              this.problemList.push(data.result.result[i]);
          }

          this.projectHealth = data.result.projectHealth;
          this.pmReportProjectService.projectHealth = this.projectHealth

        } else {
          this.problemList = [];

        }
        this.isShowProblemList = this.problemList.length == 0 ? false : true;
      })
    }
  }


  public markRead(project) {
    this.pmReportProjectService.reverseDelete(project.id, {}).subscribe((res) => {

      if (project.seen == false) {
        abp.notify.success("Mark Read!");
      } else {
        abp.notify.success("Mark Unread!");
      }
      project.seen = !project.seen

    })

  }

  //weekly
  public addWeekReport() {
    let newReport = {} as projectUserDto
    newReport.createMode = true;
    this.weeklyReportList.unshift(newReport)
    this.processWeekly = true;
  }
  public saveWeekReport(report: projectReportDto) {
    report.projectId = this.projectId
    report.isExpense = true;
    report.status = "0";
    report.startTime = moment(report.startTime).format("YYYY-MM-DD");
    delete report["createMode"]
    if (this.isEditWeeklyReport == true) {
      this.projectUserService.update(report).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
        report.startTime = moment(report.startTime).format("YYYY-MM-DD")
        this.projectUserService.update(report).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
          abp.notify.success(`updated user: ${report.userName}`);
          this.getChangedResource();
          this.getCurrentResourceOfProject(this.projectInfo.projectCode);
          this.isEditWeeklyReport = false;
          this.processWeekly = false;
          this.searchUser = ""
        })
      },
        () => {
          report.createMode = true
        })
    }
    else {
      this.projectUserService.create(report).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
        abp.notify.success("created new weekly report");
        this.processWeekly = false;
        report.createMode = false;
        this.getChangedResource();
        this.getCurrentResourceOfProject(this.projectInfo.projectCode);
        this.searchUser = ""

      },
        () => {
          report.createMode = true
        })
    }

  }
  public cancelWeekReport() {
    this.processWeekly = false;
    this.isEditWeeklyReport = false;
    this.getChangedResource();
    this.searchUser = ""
  }
  updateWeekReport(report) {
    this.processWeekly = true
    this.isEditWeeklyReport = true;
    report.createMode = true;
    report.projectRole = this.APP_ENUM.ProjectUserRole[report.projectRole]
  }
  deleteWeekReport(report) {

    abp.message.confirm(
      "Delete Issue? ",
      "",
      (result: boolean) => {
        if (result) {
          this.projectUserService.removeProjectUser(report.id).pipe(catchError(this.projectUserService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Report");
            this.getChangedResource();
          });
        }
      }
    );
  }


  //Future
  public getUser(): void {
    this.userService.GetAllUserActive(false).pipe(catchError(this.userService.handleError)).subscribe(data => {
      this.userList = data.result;
    })
  }

  public saveFutureReport(report: projectReportDto) {
    delete report["createMode"]
    if (this.isEditFutureReport) {
      this.projectUserService.update(report).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
        report.startTime = moment(report.startTime).format("YYYY-MM-DD")
        this.projectUserService.update(report).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
          abp.notify.success(`updated user: ${report.userName}`);
          this.getFuturereport();
          this.getCurrentResourceOfProject(this.projectInfo.projectCode);
          this.isEditFutureReport = false;
          this.processFuture = false
          this.searchUser = ""
        })
      },
        () => {
          report.createMode = true
        })
    }
    else {
      // report.isFutureActive = false
      report.projectId = this.projectId
      report.isExpense = true;
      report.status = "2";
      report.startTime = moment(report.startTime).format("YYYY-MM-DD");

      this.projectUserService.create(report).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
        abp.notify.success("created new future report");
        this.processFuture = false;
        report.createMode = false;
        this.getFuturereport();
        this.getCurrentResourceOfProject(this.projectInfo.projectCode);
        this.searchUser = ""
      },
        () => {
          report.createMode = true
        })
    }

  }
  public cancelFutureReport() {
    this.processFuture = false;
    this.isEditFutureReport = false;
    this.getFuturereport();
  }
  public approveRequest(resource: projectUserDto): void {
    this.showDialog(resource);

  }
  showDialog(resource: any): void {
    let dialogData = {}
    dialogData = {
      id: resource.id,
      userId: resource.userId,
      projectRole: resource.projectRole,
      startTime: resource.startTime,
      allocatePercentage: resource.allocatePercentage,
      fullName: resource.fullName
    }
    const dialogRef = this.dialog.open(ApproveDialogComponent, {
      data: {
        dialogData: dialogData,
      },
      width: "700px",
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getFuturereport();
        this.getChangedResource();
        this.getCurrentResourceOfProject(this.projectInfo.projectCode);
      }
    });

  }

  public rejectRequest(report): void {
    this.requestservice.rejectRequest(report.id).pipe(catchError(this.requestservice.handleError)).subscribe(data => {
      abp.notify.success("Rejected request")
      this.getFuturereport();
    })
  }
  public updateRequest(report): void {
    this.processFuture = true
    this.isEditFutureReport = true;
    report.createMode = true;
    report.projectRole = this.APP_ENUM.ProjectUserRole[report.projectRole]
  }
  //Problem
  public addIssueReport() {
    let newIssue = {} as projectProblemDto
    newIssue.createMode = true;
    newIssue.status = this.defaultStatus;
    this.problemList.unshift(newIssue)
    this.processProblem = true;
  }


  public saveProblemReport(problem: projectProblemDto) {
    problem.createdAt = moment(this.createdDate).format("YYYY-MM-DD");
    delete problem["createMode"]
    if (!this.isEditProblem) {
      this.reportIssueService.createReportIssue(this.projectId, problem).pipe(catchError(this.reportIssueService.handleError)).subscribe(data => {
        abp.notify.success("created new Issue");
        this.processProblem = false;
        problem.createMode = false;
        this.getProjectProblem();
      },
        () => {
          problem.createMode = true
        })
    }
    else {
      this.reportIssueService.update(problem).pipe(catchError(this.reportIssueService.handleError)).subscribe(data => {
        abp.notify.success("edited Issue");
        this.processProblem = false;
        problem.createMode = false;
        this.getProjectProblem();
        this.isEditProblem = false;
      },
        () => {
          problem.createMode = true
        })
    }

  }
  public cancelProblemReport() {
    this.processProblem = false;
    this.isEditProblem = false;
    this.getProjectProblem();
  }
  public editProblemReport(user: projectUserDto) {
    this.isEditProblem = true;
    user.createMode = true
    user.status = this.APP_ENUM.ProjectUserStatus[user.status]
    user.projectRole = this.APP_ENUM.ProjectUserRole[user.projectRole]
    // this.projectUserProcess = true
  }

  public deleteProblem(problem) {
    abp.message.confirm(
      "Delete Issue? ",
      "",
      (result: boolean) => {
        if (result) {
          this.reportIssueService.deleteReportIssue(problem.id).pipe(catchError(this.reportIssueService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Issue ");
            this.getProjectProblem();
          });
        }
      }
    );
  }
  public updateReportIssue(Issue): void {
    this.processProblem = true
    this.isEditProblem = true;
    Issue.createMode = true
    Issue.status = this.APP_ENUM.PMReportProjectIssueStatus[Issue.status]

  }

  public updateNote() {
    this.reportService.updateNote(this.projectInfo.pmNote, this.selectedReport.pmReportProjectId).pipe(catchError(this.reportService.handleError)).subscribe(rs => {
      abp.notify.success("Update successful!")
      this.isEditingNote = false;
      this.allowSendReport = this.projectInfo.pmNote ? true : false
    })
  }
  public cancelUpdateNote() {
    this.isEditingNote = false;
    this.allowSendReport = this.projectInfo.pmNote ? true : false
    this.getProjectInfo();
  }


  public updateAutoNote() {
    this.pmReportProjectService.updateAutomationNote(this.projectInfo.automationNote, this.selectedReport.pmReportProjectId).pipe(catchError(this.pmReportProjectService.handleError)).subscribe(rs => {
      abp.notify.success("Update successful!")
      this.isEditingAutomationNote = false;
    })
  }
  cancelUpdateAutoNote() {
    this.isEditingAutomationNote = false;
    this.getProjectInfo();
  }



  getPercentage(report, data) {
    report.allocatePercentage = data
  }
  charts = []
  getCurrentResourceOfProject(projectCode) {
    if (this.projectId) {
      this.tempResourceList = []
      this.officalResourceList = []
      var d = new Date();
      d.setDate(d.getDate() - (d.getDay() + 6) % 7);
      d.setDate(d.getDate() - 7);
      let lastWeekMonday = moment(new Date(d.getFullYear(), d.getMonth(), d.getDate())).format("YYYY-MM-DD")
      let usersEmail = []
      this.pmReportProjectService.GetCurrentResourceOfProject(this.projectId)
        .pipe(catchError(this.pmReportProjectService.handleError)).subscribe(data => {
          this.totalNormalWorkingTime = 0
          this.totalOverTime = 0
          this.projectCurrentResource = data.result
          this.projectCurrentResource.forEach(user => {
            if (user.isPool) {
              this.tempResourceList.push(user.emailAddress)
            }
            else {
              this.officalResourceList.push(user.emailAddress)
            }
            usersEmail.push(user.emailAddress)
            this.GetTimesheetWeeklyChartOfUserInProject(projectCode, user, this.mondayOf5weeksAgo, this.lastWeekSunday)
            this.GetTimesheetOfUserInProjectNew(projectCode, user, lastWeekMonday, this.lastWeekSunday)
          })
          this.getUserFromTimesheet(projectCode, usersEmail, lastWeekMonday)
          this.getDataForWeeklyChart(projectCode, this.mondayOf5weeksAgo, this.lastWeekSunday)
        })
    }
  }
  getUserFromTimesheet(projectCode, usersEmail, lastWeekMonday)
  {
    this.pmReportProjectService.GetUserInProjectFromTimesheet(projectCode, usersEmail, this.mondayOf5weeksAgo, this.lastWeekSunday).subscribe(rs => {
      this.projectCurrentSupportUser = rs.result
      this.projectCurrentSupportUser.forEach(user => {
        this.GetTimesheetOfSupportUserInProject(projectCode, user, lastWeekMonday, this.lastWeekSunday)
        this.GetTimesheetWeeklyChartOfUserInProject(projectCode, user, this.mondayOf5weeksAgo, this.lastWeekSunday)
      })
    })
  }
  getTimesheetWorking() {
    const dialogRef = this.dialog.open(GetTimesheetWorkingComponent, {
      data: {
        dialogData: this.selectedReport.pmReportProjectId,
      },
      width: "500px",
      disableClose: true,
    });
    dialogRef.afterClosed().subscribe((result: WorkingTimeDto) => {
      if (result) {
        // this.totalNormalWorkingTime = result.normalWorkingTime
        // this.totalOverTime = result.overTime
      }
    });
  }

  public onReportchange() {
    this.getWeeklyReport();
    this.getFuturereport();
    this.getProjectProblem();
    this.getProjectInfo();
    this.getAllCriteria();
    this.isEditingNote = false;
    this.projectHealth = this.APP_ENUM.ProjectHealth[this.selectedReport.projectHealth]
  }





  buildProjectTSChart(normalAndOTchartData, officalChartData, TempChartData) {
    setTimeout(() => {
      var chartDom = document.getElementById('timesheet-chart')!;
      var myChart = echarts.init(chartDom);
      var option: echarts.EChartsOption;

      let hasOtValue = normalAndOTchartData?.overTimeHours.some(item => item > 0)
      let hasOfficalDataNormal = officalChartData?.normalWoringHours.some(item => item > 0)
      let hasOfficalDataOT = officalChartData?.overTimeHours.some(item => item > 0)
      let hasTempDataNormal = TempChartData?.normalWoringHours.some(item => item > 0)
      let hasTempDataOT = TempChartData?.overTimeHours.some(item => item > 0)
      let hasOtNoCharge = normalAndOTchartData?.otNoChargeHours.some(item => item > 0)
      if (JSON.stringify(normalAndOTchartData?.normalWoringHours) == JSON.stringify(officalChartData?.normalWoringHours)) {
        hasOfficalDataNormal = false
      }
      if (JSON.stringify(normalAndOTchartData?.overTimeHours) == JSON.stringify(officalChartData?.overTimeHours)) {
        hasOfficalDataOT = false
      }

      option = {
        width:'90%',
        title: {
          text: 'Timesheet'
        },
        tooltip: {
          trigger: 'axis'
        },
        legend: {
          left:'25%',
          width:'80%',
          data: ['Total normal', `${hasOtValue ? 'Total OT' : ''}`, `${hasOfficalDataNormal ? 'Normal Offical' : ''}`
          , `${hasOfficalDataOT ? 'OT Offical' : ''}`, `${hasTempDataNormal ? 'Normal Temp' : ''}`,
          `${hasTempDataOT ? 'OT Temp' : ''}`,`${hasOtNoCharge ? 'OT NoCharge' : ''}`],
        },
        color: ['#211f1f', 'red', 'blue', 'orange', '#787a7a', 'purple', 'green'],
        grid: {
          left: '3%',
          right: '4%',
          bottom: '3%',
          containLabel: true
        },

        xAxis: {
          axisLabel: {
            padding: [4, 0, 0, 0]
          },
          type: 'category',
          boundaryGap: false,
          data: normalAndOTchartData.labels
        },
        yAxis: {
          type: 'value'
        },
        series: [
          {
            lineStyle: { color: '#211f1f' },
            name: 'Total normal',
            type: 'line',
            data: normalAndOTchartData?.normalWoringHours
          },
          {
            lineStyle: { color: 'red' },
            name: 'Total OT',
            type: 'line',
            data: hasOtValue ? normalAndOTchartData?.overTimeHours : []
          },
          {
            lineStyle: { color: 'blue' },
            name: 'Normal Offical',
            type: 'line',
            data: hasOfficalDataNormal ? officalChartData?.normalWoringHours : []
          }, {
            lineStyle: { color: 'orange' },
            name: 'OT Offical',
            type: 'line',
            data: hasOfficalDataOT ? officalChartData?.overTimeHours : []
          }, {
            lineStyle: { color: '#787a7a' },
            name: 'Normal Temp',
            type: 'line',
            data: hasTempDataNormal ? TempChartData?.normalWoringHours : []
          }, {
            lineStyle: { color: 'purple' },
            name: 'OT Temp',
            type: 'line',
            data: hasTempDataOT ? TempChartData?.overTimeHours : []
          },{
            lineStyle: { color: 'green' },
            name: 'OT NoCharge',
            type: 'line',
            data: hasOtNoCharge ? TempChartData?.otNoChargeHours : []
          },
        ]
      };
      option && myChart.setOption(option);
    }, 1)

  }


  public genarateUserChart(user, chartData) {
    let hasOtValue = chartData.overTimeHours.some(item => item > 0)
    let hasOtNocharge = chartData.otNoChargeHours.some(item => item > 0)

    // var chartDom = document.getElementById(user.userId.toString());
    // var myChart = echarts.init(chartDom);

    setTimeout(() => {

      let chartDom = document.getElementById('user' + user.userId);

      let myChart = echarts.init(chartDom);
      let option: echarts.EChartsOption;
      option = {
        tooltip: {
          trigger: 'axis'
        },


        grid: {
          top: "6%",
          left: '3%',
          right: '4%',
          bottom: '2%',
          containLabel: true
        },
        xAxis: {
          data: chartData.labels,
          show: false
        },
        yAxis: {
          type: 'value',
          min: 0,
          max: 60,
          show: false
        },
        series: [

          {
            // showSymbol: false,
            symbolSize: 2,
            data: chartData.normalWoringHours,
            type: 'line',
            name: 'Normal',
          },
          {
            // showSymbol: false,
            color: ['#dc3545'],
            symbolSize: 2,
            data: hasOtValue ? chartData.overTimeHours : [],
            type: 'line',
            name: 'OT',
            lineStyle: {color: '#dc3545'}
          },
          {
            // showSymbol: false,
            color:['orange'],
            symbolSize: 2,
            data: hasOtNocharge ? chartData.otNoChargeHours : [],
            type: 'line',
            name: 'OT no charge',
            lineStyle: { color: 'orange' }
          }
        ]
      };
      option && myChart.setOption(option);


    }, 1)






    // option && myChart.setOption(option);

  }



  getDataForWeeklyChart(projectCode, startTime, endTime) {
    let totalNormalAndOT = this.pmReportProjectService.GetTimesheetWeeklyChartOfProject(projectCode, startTime, endTime)
    let temp = null
    let offical = null
    let forkJoinRequest = [totalNormalAndOT]
    let apiPayload = {
      projectCode: projectCode,
      emails: this.officalResourceList,
      startDate: startTime,
      endDate: endTime
    }
    if (this.officalResourceList.length > 0) {
      apiPayload.emails = this.officalResourceList
      offical = this.pmReportProjectService.GetTimesheetWeeklyChartOfUserGroupInProject(apiPayload)
      forkJoinRequest.push(offical)
    }

    if (this.tempResourceList.length > 0) {
      apiPayload.emails = this.tempResourceList
      temp = this.pmReportProjectService.GetTimesheetWeeklyChartOfUserGroupInProject(apiPayload)
      forkJoinRequest.push(temp)
    }

    forkJoin(forkJoinRequest).subscribe((data: any) => {
      totalNormalAndOT = data[0].result
      offical = data[1]?.result
      temp = data[2]?.result
      this.buildProjectTSChart(totalNormalAndOT, offical, temp)
    });
  }


  getDataForBillChart(projectCode: string) {
    var now = new Date();
    var currentDate = this.formatDateYMD(new Date())
    let fiveMonthAgo: any =  new Date(now.setMonth(now.getMonth() - 5))
    fiveMonthAgo =  this.formatDateYMD(this.getFirstDayOfMonth(fiveMonthAgo.getFullYear(), fiveMonthAgo.getMonth()))

    let apiPayload = {
      projectCode: projectCode,
      startDate: fiveMonthAgo,
      endDate: currentDate
    }

    let effort = this.pmReportProjectService.GetEffortMonthlyChartProject(apiPayload)
    let bill = this.tsProjectService.GetBillInfoChart(this.projectId, fiveMonthAgo, currentDate)

    forkJoin([effort, bill]).subscribe(data => {
      effort = data[0].result
      bill = data[1].result
      this.buildBillChart(bill, effort)
    });

  }

  getFirstDayOfMonth(year, month) {
    return new Date(year, month, 1);
  }






  GetTimesheetWeeklyChartOfUserInProject(projectCode, user, startTime, endTime) {
    this.pmReportProjectService.GetTimesheetWeeklyChartOfUserInProject(projectCode, user.emailAddress, startTime, endTime).subscribe(rs => {
      this.genarateUserChart(user, rs.result)
    })
  }
  GetTimesheetOfUserInProject(projectCode, user, startTime, endTime) {
    this.pmReportProjectService.GetTimesheetOfUserInProject(projectCode, user.emailAddress, startTime, endTime).subscribe(rs => {
      user.normalWorkingTime = rs.result ? rs.result.normalWorkingTime : 0
      user.overTime = rs.result ? rs.result.overTime : 0
      user.overTimeNoCharge = rs.result ? rs.result.overTimeNoCharge : 0
      this.totalNormalWorkingTime += user.normalWorkingTime
      this.totalOverTime += user.overTime
      this.overTimeNoCharge += user.overTimeNoCharge
    })
    }

    GetTimesheetOfUserInProjectNew(projectCode, user, startTime, endTime) {
        this.pmReportProjectService.GetTimesheetOfUserInProjectNew(projectCode, user.emailAddress, startTime, endTime).subscribe(rs => {
            user.normalWorkingTime = rs.result ? rs.result.normalWorkingTime : 0
            user.overTime = rs.result ? rs.result.overTime : 0
            user.overTimeNoCharge = rs.result ? rs.result.overTimeNoCharge : 0
            user.normalWorkingTimeAll = rs.result ? rs.result.normalWorkingTimeAll : 0
            user.normalWorkingTimeStandard = rs.result ? rs.result.normalWorkingTimeStandard : 0
            this.totalNormalWorkingTime += user.normalWorkingTime
            this.totalOverTime += user.overTime
            this.overTimeNoCharge += user.overTimeNoCharge
            this.totalNormalWorkingTimeOfWeekly += user.normalWorkingTimeAll
            this.totalNormalWorkingTimeStandard += user.normalWorkingTimeStandard
        })
    }
    GetTimesheetOfSupportUserInProject(projectCode, user, startTime, endTime) {
        this.pmReportProjectService.GetTimesheetOfUserInProjectNew(projectCode, user.emailAddress, startTime, endTime).subscribe(rs => {
            user.normalWorkingTime = rs.result ? rs.result.normalWorkingTime : 0
            user.overTime = rs.result ? rs.result.overTime : 0
            user.overTimeNoCharge = rs.result ? rs.result.overTimeNoCharge : 0
            user.normalWorkingTimeAll = rs.result ? rs.result.normalWorkingTimeAll : 0
            user.normalWorkingTimeStandard = rs.result ? rs.result.normalWorkingTimeStandard : 0
            this.totalNormalWorkingTime1 += user.normalWorkingTime
            this.totalOverTime1 += user.overTime
            this.overTimeNoCharge1 += user.overTimeNoCharge
            this.totalNormalWorkingTimeOfWeekly1 += user.normalWorkingTimeAll
            this.totalNormalWorkingTimeStandard1 += user.normalWorkingTimeStandard
        })
    }

  GetTimesheetWeeklyChartOfUserGroupInProject(emailList) {
    // monday at 5 weeks ago =  last week mondy - 5 week (35 days)

    let requestBody = {
      projectCode: this.projectInfo.projectCode,
      emails: emailList,
      startDate: this.mondayOf5weeksAgo,
      endDate: this.lastWeekSunday
    }
    this.pmReportProjectService.GetTimesheetWeeklyChartOfUserGroupInProject(requestBody).subscribe(r => {

    })
  }
  showActions(e) {
    e.preventDefault();
    this.contextMenuPosition.x = e.clientX + 'px';
    this.contextMenuPosition.y = e.clientY + 'px';
    this.menuRelease.openMenu();


  }

  // Current project action

  releaseUser(user) {
    let ref = this.dialog.open(ReleaseUserDialogComponent, {
      width: "700px",
      data: {
        user: user
      }
    })
    ref.afterClosed().subscribe(rs => {
      if (rs) {
       this.getCurrentResourceOfProject(this.projectInfo.projectCode)
       this.getChangedResource()
      }
    })
  }


  //  planned resource action

  confirm(user) {
    if (user.allocatePercentage <= 0) {
      let ref = this.dialog.open(ReleaseUserDialogComponent, {
        width: "700px",
        data: {
          user: user,
          type: "confirmOut"
        },
      })
      ref.afterClosed().subscribe(rs => {
        if (rs) {
          this.getChangedResource()
          this.getFuturereport()
          this.getCurrentResourceOfProject(this.projectInfo.projectCode)
        }
      })
    }
    else if (user.allocatePercentage > 0) {
      let workingProject = [];
      this.projectUserService.GetAllWorkingProjectByUserId(user.userId).subscribe(data => {
        workingProject = data.result
        let ref = this.dialog.open(ConfirmPopupComponent, {
          width: '700px',
          data: {
            workingProject: workingProject,
            user: user,
            type: "confirmJoin",
            page: ConfirmFromPage.product_weekly
          }
        })

        ref.afterClosed().subscribe(rs => {
          if (rs) {
            this.getChangedResource()
            this.getFuturereport()
            this.getCurrentResourceOfProject(this.projectInfo.projectCode)
          }
        })
      })
    }
  }

  cancelResourcePlan(user) {
    abp.message.confirm(
      `Cancel plan for user <strong>${user.fullName}</strong> <strong class = "${user.allocatePercentage > 0 ? 'text-success' : 'text-danger'}">
      ${user.allocatePercentage > 0 ? 'Join project' : 'Out project'}</strong>?`,
      "",
      (result: boolean) => {
        if (result) {
          this.projectUserService.CancelResourcePlan(user.id).subscribe(rs => {
            abp.notify.success(`Cancel plan for user ${user.fullName}`)
            this.getFuturereport()
          })
        }
      }, {isHtml:true}
    )
  }

  updateHealth(projectHealth) {
    this.pmReportProjectService.updateHealth(this.selectedReport.pmReportProjectId, projectHealth).subscribe((data) => {
      abp.notify.success("Update project health to " + this.getByEnum(projectHealth, this.APP_ENUM.ProjectHealth))
    })

  }

  setDone(issue){
    this.pmReportProjectService.SetDoneIssue(issue.id).subscribe((res)=>{
      if(res){
        abp.notify.success("Update Successfully!")
      }
      this.getProjectProblem();
    })
  }

    public buildBillChart(billData,EffortData) {

      // var chartDom = document.getElementById(user.userId.toString());
      // var myChart = echarts.init(chartDom);

      setTimeout(() => {

        let chartDom = document.getElementById('bill-chart');

        let myChart = echarts.init(chartDom);
        let option: echarts.EChartsOption;
        option = {
          tooltip: {
            trigger: 'axis',
          },
          title: {
            text: 'Bill info'
          },
          grid: {
            left: '3%',
            right: '4%',
            bottom: '1%',
            containLabel: true
          },
          legend: {
            data: ['Bill.ManMonth', 'Bill.ManDay', `${EffortData?.manDays? 'Effort.ManDay' : ''}`]
          },
          xAxis: [
            {
              axisLabel: {
                padding: [4, 0, 0, 0]
              },
              type: 'category',
              data: billData.labels,
              boundaryGap: false,
            }
          ],
          yAxis: [
            {
              axisLabel: {
                padding: [0, 13, 0, 13]
              },
              type: 'value',
              name: 'ManMonth',

            },
            {
              axisLabel: {
                padding: [0, 13, 0, 13]
              },
              type: 'value',
              name: 'ManDay',

            }
          ],
          series: [

            {
              barWidth: 30,
              name: 'Bill.ManMonth',
              type: 'bar',
              data: billData.manMonths
            },
            {
              name: 'Bill.ManDay',
              type: 'line',
              yAxisIndex: 1,
              data: billData.manDays
            },
            {
              name: 'Effort.ManDay',
              type: 'line',
              yAxisIndex: 1,
              data: EffortData?.manDays
            }
          ]
        };
        option && myChart.setOption(option);
      }, 1)
    }


    addPlanResource() {
      let ref = this.dialog.open(AddFutureResourceDialogComponent, {
        width: "700px",
        data: {
          projectId: this.projectId,
          projectName: this.projectInfo.projectName
        }
      })
      ref.afterClosed().subscribe(rs => {
        if (rs) {
          this.getFuturereport()
        }
      })
    }
    editResourcePlan(resource) {
      let item = {
        projectUserId:resource.id,
        fullName: resource.fullName,
        projectId: this.projectId,
        projectRole: resource.projectRole,
        userId: resource.userId,
        startDate: resource.startDate,
        isPool: resource.isPool,
        allocatePercentage: resource.allocatePercentage,
        startTime: resource.startTime
      }
      let ref = this.dialog.open(AddFutureResourceDialogComponent, {
        width: "700px",
        data: {
          command: "edit",
          item: item
        }
      })
      ref.afterClosed().subscribe(rs => {
        if (rs) {
          this.getFuturereport()
        }
      })
    }
    isShowChangeDoneText(issue){
      return this.APP_ENUM.PMReportProjectIssueStatus[issue.status] == this.APP_ENUM.PMReportProjectIssueStatus.InProgress
    }
    isShowChangeInProgressText(issue){
      return this.APP_ENUM.PMReportProjectIssueStatus[issue.status] == this.APP_ENUM.PMReportProjectIssueStatus.Done
    }
    showGuideLine(item) {
      const show = this.dialog.open(GuideLineDialogComponent,{
        width: "60%",
        data:item
        })
      }
}

