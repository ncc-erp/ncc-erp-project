import { BillAccountDialogNoteComponent } from './bill-account-dialog-note/bill-account-dialog-note.component';
import {ChangeDetectorRef, Component, EventEmitter, Injector, OnInit, Output } from "@angular/core";
import * as _moment from "moment";
import { FormControl } from "@angular/forms";
import * as moment from "moment";
import { APP_ENUMS } from "@shared/AppEnums";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { catchError } from "rxjs/operators";
import { PlanningBillInfoService } from "../../../../../service/api/bill-account-plan.service";
import { THeadTable } from "../../request-resource-tab/request-resource-tab.component";
import { SortableModel } from "@shared/components/sortable/sortable.component";
import { MatSelectChange } from "@angular/material/select";
import { MatDialog } from "@angular/material/dialog";

@Component({
  selector: "app-bill-account-plan",
  templateUrl: "./bill-account-plan.component.html",
  styleUrls: ["./bill-account-plan.component.css"],
})
export class BillAccountPlanComponent
  extends PagedListingComponentBase<any>
  implements OnInit
{
  APP_ENUM = APP_ENUMS;
  @Output() onDateSelectorChange = new EventEmitter();

  searchProject: string = "";
  searchClient: string = "";
  date = new FormControl(moment());
  dateYear = new FormControl(moment());
  planType = APP_ENUMS.PlanType.ALL;
  public projectStatus;
  public isShowLevel: boolean = false;
  public isShowBillRate: boolean = false;

  public filterFromDate: string;
  public filterToDate: string;
  public projectId;
  public clientId;
  public selectedClientId: string = "";
  public billInfoList = [];
  public projectList = [];
  public clientList = [];
  public selectedIsPlanned: number;
  
  public theadTable: THeadTable[] = [
    { name: "#", width: "30px" },
    { name: "Bill Account"},
    { name: "Client", width: "140px"},
    { name: "Projects"},
    { name: "Is Charge", width: "70px", padding : "12px 10px", whiteSpace: "nowrap" },
    { name: "Bill Date", width: "100px" },
    { name: "Note" },
  ];

  public sortable = new SortableModel("", 0, "");
  public sortResource = {};

  projectStatusList = [
    { text: "Potential", value: APP_ENUMS.ProjectStatus.Potential },
    { text: "InProgress", value: APP_ENUMS.ProjectStatus.InProgress },
    { text: "Closed", value: APP_ENUMS.ProjectStatus.Closed },
  ];

  constructor(
    injector: Injector,
    private planningBillInfoService: PlanningBillInfoService,
    private ref: ChangeDetectorRef,
    private dialog: MatDialog,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getDateOptions();
    this.planningBillInfoService
      .GetAllProjectUserBill()
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe((data) => {
        this.projectList = data.result;
      });
      this.refresh();

    this.planningBillInfoService
      .GetAllProjectClientBill()
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe((data) => {
        this.clientList = data.result;
      });
    var date = new Date();
    this.refresh();

    this.planningBillInfoService
      .GetAllProjectClientBill()
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe((data) => {
        this.clientList = data.result;
      });
    var date = new Date();
    this.refresh();
  }

  public listDateOptions = [];
  public defaultValue = APP_ENUMS.DATE_TIME_OPTIONS.Month;
  public birthdayFromDate: string;
  public birthdayToDate: string;

  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
    skill?
  ): void {
    const requestBody: any = {
      searchText: this.searchText,
      projectId: this.projectId,
      clientId: this.clientId,
      //joinOutStatus:this.planType,
      planStatus: this.selectedIsPlanned || APP_ENUMS.PlanStatus.All,
      projectStatus: this.projectStatus,
      startDate: this.filterFromDate,
      endDate: this.filterToDate,
      girdParam: {
        skipCount: (this.pageNumber - 1) * this.pageSize,
        maxResultCount: this.pageSize,
      },
      sortParams: this.sortResource,
    };
    this.planningBillInfoService
      .GetAllBillInfo(requestBody)
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe(
        (data) => {
          console.log(data);
          this.billInfoList = data.result.items;
          this.showPaging(data.result, pageNumber);
          this.isLoading = false;
        },
        () => {}
      );
  }

  protected delete(entity: BillAccountPlanComponent): void {}
  getDateOptions() {
    this.listDateOptions = [
      APP_ENUMS.DATE_TIME_OPTIONS.Month,
      APP_ENUMS.DATE_TIME_OPTIONS.Quarter,
      APP_ENUMS.DATE_TIME_OPTIONS.Year,
      APP_ENUMS.DATE_TIME_OPTIONS.CustomTime,
    ];
  }
  public handleDateSelectorChange(data) {
    this.filterFromDate = data?.fromDate;
    this.filterToDate = data?.toDate;
    this.getDataPage(1);
  }

  filerByPlanType() {
    this.getDataPage(1);
  }

  filerByProjectStatus() {
    this.getDataPage(1);
  }

  filerByProject() {
    this.getDataPage(1);
  }

  filerByClient() {
    this.getDataPage(1);
  }

  checkTimeInFilterDate(time) {
    if (
      time >= this.filterFromDate &&
      Date.parse(time) <= Date.parse(this.filterToDate)
    ) {
      return true;
    } else {
      return false;
    }
  }

  applyPlanFilter() {
    this.projectId = "";
    this.searchProject = "";
    this.getDataPage(1);
  }

  applyClientFilter() {
    this.clientId = "";
    this.searchClient = "";
    this.getDataPage(1);
  }

  applyProjectStatusFilter() {
    this.projectStatus = "";
    this.getDataPage(1);
  }

  applyPlanTypeFilter() {
    this.selectedIsPlanned = APP_ENUMS.PlanStatus.All;
    this.isFilterSelected = false;
    this.getDataPage(1);
  }

  sortTable(event: any) {
    this.sortable = event;
    this.changeSortableByName(
      this.sortable.sort,
      this.sortable.typeSort,
      this.sortable.sortDirection
    );
    this.refresh();
  }

  changeSortableByName(sort: string, sortType: string, sortDirection?: number) {
    if (!sortType) {
      delete this.sortResource[sort];
    } else {
      this.sortResource[sort] = sortDirection;
    }
    this.ref.detectChanges();
  }

  styleThead(item: any) {
    return {
      width: item.width,
      height: item.height,
    };
  }

  viewProjectDetail(project) {
    let routingToUrl: string = "";
    if (project.projectType == 5) {
      routingToUrl =
        this.permission.isGranted(
          this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport
        ) &&
        this.permission.isGranted(
          this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View
        )
          ? "/app/training-project-detail/training-weekly-report"
          : "/app/training-project-detail/training-project-general";
    } else if (project.projectType == 3) {
      routingToUrl =
        this.permission.isGranted(
          this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
        ) &&
        this.permission.isGranted(
          this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
        )
          ? "/app/product-project-detail/product-weekly-report"
          : "/app/product-project-detail/product-project-general";
    } else {
      routingToUrl = "/app/list-project-detail/project-bill-tab";
    }
    const url = this.router.serializeUrl(
      this.router.createUrlTree([routingToUrl], {
        queryParams: {
          id: project.projectId,
          type: project.projectType,
          projectName: project.projectName,
          projectCode: project.projectCode,
        },
      })
    );
    return url;
  }
  showLevel(checked: boolean) {
    this.isShowLevel = checked;
  }

  showBillRate(checked: boolean) {
    this.isShowBillRate = checked;
  }

  UpdateBillNote(userId,projectId, note) {
    const addOrEditNoteDialog = this.dialog.open(BillAccountDialogNoteComponent, {
      width: "40%",
      height: "70%",
      data: {
        userId:userId,
        projectId: projectId,
        note: note,
      },
    });

    addOrEditNoteDialog.afterClosed().subscribe(() => {
      this.refresh();
    });
  }
}
