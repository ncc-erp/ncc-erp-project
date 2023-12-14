import {
  Component,
  EventEmitter,
  Injector,
  Input,
  OnInit,
  Output,
} from "@angular/core";

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
  date = new FormControl(moment());
  dateYear = new FormControl(moment());
  planType = APP_ENUMS.PlanType.ALL;
  chargeStatus = APP_ENUMS.ChargeStatus.IsCharge;
  public filterFromDate: string;
  public filterToDate: string;
  public projectId;
  public billInfoList = [];
  public projectList = [];
  public selectedIsPlanned: number;
  projectType = [
    {
      label: "All",
      value: this.APP_ENUM.FilterProjectType.All,
    },
    {
      label: "Main",
      value: false,
    },
    {
      label: "Sub",
      value: true,
    },
  ];

  chargeStatusList = [
    { text: "All", value: APP_ENUMS.ChargeStatus.All },
    { text: "Charge", value: APP_ENUMS.ChargeStatus.IsCharge },
    { text: "NoCharge", value: APP_ENUMS.ChargeStatus.IsNotCharge },
  ];

  planStatusList = [
    { value: APP_ENUMS.PlanStatus.AllPlan, displayName: "Has plans" },
    { value: APP_ENUMS.PlanStatus.PlanningJoin, displayName: "Planning join" },
    { value: APP_ENUMS.PlanStatus.PlanningOut, displayName: "Planning out" },
    { value: APP_ENUMS.PlanStatus.NoPlan, displayName: "No plan" },
  ];

  constructor(
    injector: Injector,
    private planningBillInfoService: PlanningBillInfoService
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
    var date = new Date();
    this.filterFromDate = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    ).toDateString();
    this.filterToDate = new Date(
      date.getFullYear(),
      date.getMonth() + 1,
      0
    ).toDateString();
    this.birthdayFromDate = this.filterFromDate;
    this.birthdayToDate = this.filterToDate;
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
      //joinOutStatus:this.planType,
      planStatus: this.selectedIsPlanned || APP_ENUMS.PlanStatus.All,
      chargeStatus: this.chargeStatus,
      startDate: this.filterFromDate,
      endDate: this.filterToDate,
      girdParam: {
        skipCount: (this.pageNumber - 1) * this.pageSize,
        maxResultCount: this.pageSize,
      },
    };
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
  filerByChargeStatus() {
    this.getDataPage(1);
  }
  filerByProject() {
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

  applyPlanTypeFilter() {
    this.selectedIsPlanned = APP_ENUMS.PlanStatus.All;
    this.isFilterSelected = false;
    this.getDataPage(1);
  }
}
