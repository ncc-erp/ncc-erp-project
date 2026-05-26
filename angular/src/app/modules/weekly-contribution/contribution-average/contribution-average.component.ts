import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { BranchService } from "@app/service/api/branch.service";
import { PMReportProjectContributionService } from "@app/service/api/pmreport-project-contribution.service";
import { ResourceManagerService } from "@app/service/api/resource-manager.service";
import { BranchDto } from "@app/service/model/branch.dto";
import { ContributionAverageUserDto, ContributionAverageResultDto, UserTypeDto } from "@app/service/model/weekly-contribution.dto";
import { catchError, finalize } from "rxjs/operators";

@Component({
  selector: "app-contribution-average",
  templateUrl: "./contribution-average.component.html",
  styleUrls: ["./contribution-average.component.css"],
})
export class ContributionAverageComponent
  extends PagedListingComponentBase<ContributionAverageUserDto>
  implements OnInit
{
  public contributionSummaries: ContributionAverageUserDto[] = [];
  public listBranchs: BranchDto[] = [];
  public listBranchsId: number[] = [];
  public selectedBranchIds: number[] = [];
  public searchBranch: string = "";
  public listUserTypes: UserTypeDto[] = [];
  public selectedUserTypes: number[] = [];
  public searchUserType: string = "";
  public listProject: any[] = [];
  public projectSearchText: string = "";
  public selectedProjectId: number | null = null;
  public fromDate: Date;
  public toDate: Date;
  public weeklyReportInRange: number = 0;
  public isAverageLoaded: boolean = false;
  public sortColumn: string = "averageContribution";
  public sortDirection: number = 1;
  public iconSort: string = "fas fa-sort-amount-down-alt";

  @ViewChild("selectBranch") selectBranch;

  constructor(
    injector: Injector,
    private branchService: BranchService,
    private pmReportProjectContributionService: PMReportProjectContributionService,
    private resourceManagerService: ResourceManagerService,
  ) {
    super(injector);
    this.pageSize = 100;
    this.pageSizeType = 100;
  }

  ngOnInit(): void {
    this.setDefaultDateRange();
    this.getAllBranchs();
    this.getAllUserTypes();
    this.getProjectsForAllResource();
    this.refresh();
  }

  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    if (!this.isValidDateRange()) {
      finishedCallback();
      return;
    }

    const inputRequest = {
      ...request,
      searchText: this.searchText,
      branchIds: this.selectedBranchIds,
      projectId: this.selectedProjectId ?? null,
      userTypes: this.selectedUserTypes,
      fromDate: this.fromDate,
      toDate: this.toDate,
      sort: this.sortColumn,
      sortDirection: this.sortDirection,
    };

    this.pmReportProjectContributionService
      .getContributionAverage(inputRequest)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
        catchError(this.pmReportProjectContributionService.handleError),
      )
      .subscribe((data) => {
        const result: ContributionAverageResultDto = data.result;
        this.contributionSummaries = result.items;
        this.weeklyReportInRange = result.weeklyReportInRange;
        this.isAverageLoaded = true;
        this.showPaging({ items: result.items, totalCount: result.totalCount }, pageNumber);
      });
  }

  public getAvatar(user: ContributionAverageUserDto): string {
    if (user.avatarPath) {
      return user.avatarPath;
    }
    return "/assets/img/user.png";
  }

  public filterAverage(): void {
    this.getDataPage(1);
  }

  public sortData(property: string): void {
    if (this.sortColumn === property) {
      this.sortDirection = this.sortDirection === 1 ? 0 : 1;
    } else {
      this.sortColumn = property;
      this.sortDirection = property === "averageContribution" ? 1 : 0;
    }

    this.iconSort =
      this.sortDirection === 1
        ? "fas fa-sort-amount-down-alt"
        : "fas fa-sort-amount-down";

    this.refresh();
  }

  public getAllBranchs(): void {
    this.branchService
      .getAllNotPagging()
      .pipe(catchError(this.branchService.handleError))
      .subscribe((data) => {
        this.listBranchs = data.result;
        this.listBranchsId = data.result.map((item) => item.id);
      });
  }

  public getAllUserTypes(): void {
    this.listUserTypes = Object.entries(this.APP_ENUM.UserType)
      .filter(([displayName]) => displayName !== "FakeUser")
      .map(([displayName, value]) => ({
        displayName,
        value: Number(value),
      }));
  }

  public getProjectsForAllResource(): void {
    this.resourceManagerService.getProjectsForAllResource().subscribe((data) => {
      this.listProject = data.result;
    });
  }

  private setDefaultDateRange(): void {
    this.toDate = new Date();
    this.fromDate = new Date();
    this.fromDate.setDate(this.toDate.getDate() - 30);
  }

  private isValidDateRange(): boolean {
    if (!this.fromDate || !this.toDate) {
      abp.notify.error("Date range is required.");
      return false;
    }

    if (this.fromDate > this.toDate) {
      abp.notify.error("From date must be less than or equal to To date.");
      return false;
    }

    return true;
  }

  protected delete(entity: ContributionAverageUserDto): void {}
}