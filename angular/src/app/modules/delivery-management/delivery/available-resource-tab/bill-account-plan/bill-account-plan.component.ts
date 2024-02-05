import { BillAccountDialogNoteComponent } from './bill-account-dialog-note/bill-account-dialog-note.component';
import {ChangeDetectorRef, Component, EventEmitter, Injector, OnInit, Output } from "@angular/core";
import * as _moment from "moment";
import { APP_ENUMS } from "@shared/AppEnums";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { catchError } from "rxjs/operators";
import { PlanningBillInfoService } from "../../../../../service/api/bill-account-plan.service";
import { THeadTable } from "../../request-resource-tab/request-resource-tab.component";
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
  public searchProject: string = "";
  public searchClient: string = "";
  public projectStatus;
  public isCharge: boolean = true;
  public isShowLevel: boolean = false;
  public isShowBillRate: boolean = false;
  public filterFromDate: string;
  public filterToDate: string;
  public projectId;
  public clientId;
  public billInfoList = [];
  public projectList = [];
  public clientList = [];

  public theadTable: THeadTable[] = [
    { name: "#", width: "30px" },
    { name: "Bill Account"},
    { name: "Client", width: "140px"},
    { name: "Projects"},
    { name: "Is Charge", width: "70px", padding : "12px 10px", whiteSpace: "nowrap" },
    { name: "Bill Date", width: "100px" },
    { name: "Note" },
  ];

  public projectStatusList = [
    { text: "Potential", value: APP_ENUMS.ProjectStatus.Potential },
    { text: "InProgress", value: APP_ENUMS.ProjectStatus.InProgress },
    { text: "Closed", value: APP_ENUMS.ProjectStatus.Closed },
  ];

  public isChargeList = [
    { text: "Charged", value: true },
    { text: "UnCharged", value: false },
  ];

  constructor(
    injector: Injector,
    private planningBillInfoService: PlanningBillInfoService,
    private dialog: MatDialog,
    ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getProjectUserBill();
    this.getProjectClientBill();
    this.refresh();
  }

  getProjectUserBill(): void {
    this.planningBillInfoService
        .GetAllProjectUserBill()
        .pipe(catchError(this.planningBillInfoService.handleError))
        .subscribe((data) => {
            this.projectList = data.result;
        });
  }

  getProjectClientBill(): void {
    this.planningBillInfoService
        .GetAllProjectClientBill()
        .pipe(catchError(this.planningBillInfoService.handleError))
        .subscribe((data) => {
            this.clientList = data.result;
        });
  } 

  protected list(
    request: PagedRequestDto,
    pageNumber: number,
  ): void {
    const requestBody :any = {...request,
      searchText: this.searchText,
      projectId: this.projectId,
      clientId: this.clientId,
      projectStatus: this.projectStatus,
      isCharge: this.isCharge,
      sortParams: this.sortResource};

    this.planningBillInfoService
      .GetAllBillInfo(requestBody)
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe(
        (data) => {
          this.billInfoList = data.result.items;
          this.showPaging(data.result, pageNumber);
          this.isLoading = false;
        },
        () => {}
      );
  }

  protected delete(entity: BillAccountPlanComponent): void {}

  filerByProjectStatus() {
    this.getDataPage(1);
  }

  onSelectedChargeFilter() {
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

  applyProjectFilter() {
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

  clearChargeFilter() {
    this.isCharge = null;
    this.getDataPage(1);
  }

  isShowBtnClearChargeFilter() {
    return this.isCharge !== null;
  }

  styleThead(item: any) {
    return {
      width: item.width,
      height: item.height,
    };
  }

  viewProjectDetail(project) {
    let routingToUrl: string = this.getRoutingUrl(project);
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

  getRoutingUrl(project) {
    if (project.projectType == 5) {
      return this.permission.isGranted(
        this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport
      ) &&
      this.permission.isGranted(
        this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View
      )
        ? "/app/training-project-detail/training-weekly-report"
        : "/app/training-project-detail/training-project-general";
    } else if (project.projectType == 3) {
      return this.permission.isGranted(
        this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
      ) &&
      this.permission.isGranted(
        this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
      )
        ? "/app/product-project-detail/product-weekly-report"
        : "/app/product-project-detail/product-project-general";
    } else {
      return "/app/list-project-detail/project-bill-tab";
    }
  }

  showLevel(checked: boolean) {
    this.isShowLevel = checked;
  }

  showBillRate(checked: boolean) {
    this.isShowBillRate = checked;
  }

  UpdateBillNote(userInfor, projectId, note, projectName) {
    const addOrEditNoteDialog = this.dialog.open(BillAccountDialogNoteComponent, {
      width: "580px",
      data: {
        userInfor: userInfor,
        projectId: projectId,
        note: note,
        projectName: projectName
      },
    });
    addOrEditNoteDialog.afterClosed().subscribe(() => {
      this.refresh();
    });
  }
}