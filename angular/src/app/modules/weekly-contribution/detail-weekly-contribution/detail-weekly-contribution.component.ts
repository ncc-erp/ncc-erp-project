import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { catchError, finalize } from "rxjs/operators";
import { PMReportProjectContributionService } from "@app/service/api/pmreport-project-contribution.service";
import { UserGroupContributionDto } from "@app/service/model/weekly-contribution.dto";
import { ListProjectService } from "./../../../service/api/list-project.service";
import { UserService } from "./../../../service/api/user.service";
import { BranchService } from "@app/service/api/branch.service";

@Component({
  selector: "app-detail-weekly-contribution",
  templateUrl: "./detail-weekly-contribution.component.html",
  styleUrls: ["./detail-weekly-contribution.component.css"],
})
export class DetailWeeklyContributionComponent
  extends PagedListingComponentBase<UserGroupContributionDto>
  implements OnInit
{
  userGroupContributions: UserGroupContributionDto[] = [];
  public pmList: any[] = [];
  public tempPMList: any[] = [];
  public pmId = -1;
  public searchPM: string = "";
  public selectedBranchIds: number[] = [];
  public selectedBranchIdsCr: number[] = [];
  public selectedBranchIdsOld: number[] = [];
  public searchBranch: string = "";

  public sortColumn: string = "";
  public sortDirection: number = -1;
  public iconSort: string = "";
  public totalContributionSum: number = 0;
  public pmReportName: string = "";

  constructor(
    injector: Injector,
    private pmReportProjectContributionService: PMReportProjectContributionService,
    private route: ActivatedRoute,
    public listProjectService: ListProjectService,
    public userService: UserService,
    public branchService: BranchService,
  ) {
    super(injector);
    this.pageSize = 100;
    this.pageSizeType = 100;
  }

  @ViewChild("selectBranch") selectBranch;

  ngOnInit(): void {
    this.pmReportId = this.route.snapshot.queryParamMap.get("pmReportId");
    this.pmReportName = this.route.snapshot.queryParamMap.get("pmReportName");
    this.refresh();
    this.getAllBranchs();
  }

  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    if (!this.pmReportId) {
      finishedCallback();
      return;
    }

    const inputRequest = {
      ...request,
      searchText: this.searchText,
      branchIds: this.selectedBranchIds,
      sort: this.sortColumn,
      sortDirection: this.sortDirection === 1 ? 1 : 0,
    };

    this.pmReportProjectContributionService
      .getAllPagingContributions(this.pmReportId, inputRequest)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe((data) => {
        this.userGroupContributions = data.result.items;
        this.showPaging(data.result, pageNumber);
        this.getTotalContributionFromAllPages();
      });
  }

  public getAvatar(member) {
    if (member.avatarFullPath) {
      return member.avatarFullPath;
    }
    if (member.fullAvatarPath) {
      return member.fullAvatarPath;
    }
    return "/assets/img/user.png";
  }

  public searchProject() {
    this.getDataPage(1);
  }

  public openedChange(opened: boolean) {
    if (!opened) {
      // this.selectedBranchIds = [...this.selectedBranchIdsOld];
      // this.selectedBranchIdsCr = [...this.selectedBranchIdsOld];
      this.selectedBranchIdsOld = [...this.selectedBranchIds];
      this.selectedBranchIdsCr = [...this.selectedBranchIds];
      this.searchBranch = "";
      this.refresh();
    }
  }

  public actionSelect(typeSelect: any) {
    this.selectedBranchIds = typeSelect.data;
    this.selectedBranchIdsCr = typeSelect.data;
  }

  public selectDone() {
    this.selectedBranchIdsOld = this.selectedBranchIds;
    this.selectBranch.close();
    this.refresh();
  }

  public getTotalContributionFromAllPages() {
    if (!this.pmReportId) {
      return;
    }
    const inputRequest = {
      searchText: this.searchText,
      branchIds: this.selectedBranchIds,
    };
    this.pmReportProjectContributionService
      .getTotalContribution(this.pmReportId, inputRequest)
      .subscribe((data) => {
        this.totalContributionSum = data.result || 0;
      });
  }

  public getAllBranchs() {
    this.branchService.getAllNotPagging().subscribe((data) => {
      this.listBranchs = data.result;
      this.listBranchsId = data.result.map((item) => item.id);
      this.selectedBranchIds = data.result.map((item) => item.id);
      this.selectedBranchIdsOld = [...this.selectedBranchIds];
      this.selectedBranchIdsCr = this.selectedBranchIds;
    });
  }

  public sortData(property: string) {
    if (this.sortColumn === property) {
      this.sortDirection = -this.sortDirection;
    } else {
      this.sortColumn = property;
      this.sortDirection = -1;
    }
    this.iconSort =
      this.sortDirection === 1
        ? "fas fa-sort-amount-down-alt"
        : "fas fa-sort-amount-down";
    // Use backend sorting instead of client-side sorting
    this.refresh();
  }
  protected delete(entity: any): void {}
}
