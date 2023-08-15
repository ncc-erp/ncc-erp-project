import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { pmReportProjectHealthDto} from './../service/model/pmReport.dto';
import { ProjectInfoDto } from './../service/model/project.dto';
import { Component, OnInit, Injector, ViewChild, HostListener } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, ActivationEnd, NavigationEnd, NavigationStart, Router, Event as NavigationEvent } from '@angular/router';
import { AddReportNoteComponent } from '@app/modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/add-report-note/add-report-note.component';
import { PmReportService } from '@app/service/api/pm-report.service';
import { pmReportDto } from '@app/service/model/pmReport.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { LayoutStoreService } from '@shared/layout/layout-store.service';
import { catchError, filter } from 'rxjs/operators';
import { PMReportProjectService } from '@app/service/api/pmreport-project.service';

@Component({
  selector: 'header-left-navbar',
  templateUrl: './header-left-navbar.component.html',
  styleUrls: ['./header-left-navbar.component.css']

  // changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderLeftNavbarComponent extends AppComponentBase implements OnInit {
  @ViewChild("timmer") timmerCount;
  WeeklyReport_ReportDetail_UpdateProjectHealth = PERMISSIONS_CONSTANT.WeeklyReport_ReportDetail_UpdateProjectHealth;
  sidebarExpanded: boolean;
  isShowReportBar: boolean = false;
  currentUrl: string = "";
  pmReportList: pmReportDto[] = [];
  pmReport = {} as pmReportDto;
  reportId: any;
  select:any;
  projectInfo = {} as ProjectInfoDto
  projectName: string
  projectCode: string
  public projectHealthList: string[] =  Object.keys(this.APP_ENUM.ProjectHealth);
  projectHealth;
  projectId;
  isTimmerCounting: boolean = false
  isStopCounting: boolean = false
  isRefresh: boolean = false
  isStart: boolean = false
  pmReportProjectId:string
  public searchPmReport: string ="";
  public selectedNumberStatus: number;

  projectType = this.reportService.projectType.getValue();

  projectTypeList = [
    "ALL",
    "OUTSOURCING",
    "TRAINING",
    "PRODUCT"
  ];

  filterProjectHealth = this.reportService.filterProjectHealth.getValue();
  public projectHeathList = [
    {displayName: "ALL", value: "ALL"},
    {displayName: "Green", value: 1},
    {displayName: "Yellow", value: 2},
    {displayName: "Red", value: 3},
  ];

  public problemIssueList: string[] = Object.keys(this.APP_ENUM.ProjectHealth);

  filterSort = this.reportService.filterSort.getValue();
  filterSortList = [
    "No_Order",
    "Draft_Green_Yellow_Red",
    "Draft_Red_Yellow_Green",
    "Latest_Review_Last"
  ]

  filterReviewNeed = this.reportService.filterReviewNeed.getValue();
  filterReviewNeedList = [
    "All",
    "Nothing",
    "Need_Review",
    "Reviewed"
  ]

  private filterSortStorageKey = 'filterSort';
  private filterReviewNeedStorageKey = 'filterReviewNeed';

  constructor(public _layoutStore: LayoutStoreService, private router: Router, injector: Injector,
    private dialog: MatDialog, private route: ActivatedRoute, public reportService: PmReportService,
    private pmReportProjectService: PMReportProjectService) {
    super(injector);

  }

  ngOnInit(): void {
    this.projectInfo.projectName = this.route.snapshot.queryParamMap.get("name")
    this.projectInfo.clientName = this.route.snapshot.queryParamMap.get("client")
    this.projectInfo.clientCode = this.route.snapshot.queryParamMap.get("clientCode")
    this.projectInfo.pmName = this.route.snapshot.queryParamMap.get("pmName")
    this.projectHealth = Number(this.route.snapshot.queryParamMap.get("projectHealth"))

    this.projectName = this.route.snapshot.queryParamMap.get("projectName")
    this.projectCode = this.route.snapshot.queryParamMap.get("projectCode")

    this._layoutStore.sidebarExpanded.subscribe((value) => {
      this.sidebarExpanded = value;
    });
    this.currentUrl = this.router.url
    if (this.currentUrl.includes("weeklyReportTabDetail")) {
      this.reportId = this.route.snapshot.queryParamMap.get("id")
      this.isShowReportBar = true
      this.getPmReportList();
      this._layoutStore.setSidebarExpanded(true);
    }
    else {
      this.isShowReportBar = false;
    }
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(
        (event: NavigationEvent) => {
          this.currentUrl = this.router.url
          this.reportId = this.route.snapshot.queryParamMap.get("id")
          if (this.currentUrl.includes("weeklyReportTabDetail")) {
            this.isShowReportBar = true;
            this.getPmReportList();
            this._layoutStore.setSidebarExpanded(true);

          }
          else {
            this.isShowReportBar = false;
            // this.reportService.changeProjectType("OUTSOURCING")
          }
          if(this.isShowReportBar){
            this.projectHealth = this.pmReportProjectService.projectHealth
            this.pmReportProjectId = this.route.snapshot.queryParamMap.get("pmReportProjectId")
            this.projectInfo.projectName = this.route.snapshot.queryParamMap.get("name")
            this.projectInfo.clientName = this.route.snapshot.queryParamMap.get("client")
            this.projectInfo.clientCode = this.route.snapshot.queryParamMap.get("clientCode")
            this.projectInfo.pmName = this.route.snapshot.queryParamMap.get("pmName")
          }

          this.projectName = this.route.snapshot.queryParamMap.get("projectName")
          this.projectCode = this.route.snapshot.queryParamMap.get("projectCode")

        }
      )
  }
  getPmReportList() {
    this.reportService.getAll().pipe(catchError(this.reportService.handleError)).subscribe(data => {
      this.pmReportList = data.result
      this.pmReport = this.pmReportList.filter(report => report.id == Number(this.reportId))[0]
    })
  }

  toggleSidebar(): void {
    this._layoutStore.setSidebarExpanded(!this.sidebarExpanded);
  }
  showDialog() {
    let reportId = this.route.snapshot.queryParamMap.get("id")
    const show = this.dialog.open(AddReportNoteComponent, {
      data: {
        reportId: reportId
      },
      width: "700px",
    })
  }
  routingReportDetail() {
    this.router.navigateByUrl('', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/app/weeklyReportTabDetail'], {
        relativeTo: this.route, queryParams: {
          id: this.pmReport.id,
          isActive: this.pmReport.isActive,
        },
      });
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
  changeProjectType(type){
    this.reportService.changeProjectType(type)
  }


  changeProjectHealth(pmReportProjectId,projectHealth) {
    let data = {pmReportProjectId,projectHealth} as pmReportProjectHealthDto;
    this.reportService.changeProjectHealth(data)
  }

  onChangeFilter(){
    this.reportService.changeFilter({filterSort:this.filterSort, reviewNeed:this.filterReviewNeed, filterProjectHealth:this.filterProjectHealth, projectType : this.projectType});
  }

  updateHealth(projectHealth) {
    this.pmReportProjectService.updateHealth(this.pmReportProjectId, projectHealth).pipe(catchError(this.pmReportProjectService.handleError))
      .subscribe((data) => {
        this.pmReportProjectService.projectHealth = projectHealth
        this.changeProjectHealth( this.pmReportProjectId,projectHealth)
        abp.notify.success("update successful")
      })
  }
}


