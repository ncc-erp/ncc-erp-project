import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { IEventObject, ITypeEnum } from '@app/service/model/common.interface';
import { BranchService } from '@app/service/api/branch.service';
import { BackupUserContributionService } from '@app/service/api/backup-user-contribution.service';
import { BranchDto } from '@app/service/model/branch.dto';
import { catchError } from '@node_modules/rxjs/operators';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Utils } from '@shared/Utils';
import { IInfoMonthlyUserContribution, IProjectContribute } from '@app/service/model/user-contribution.interface'; // Import the interface
import { Moment } from 'moment';
import * as moment from 'moment';
import { FilterMonthlyUserContributionDto } from '@app/service/model/user-contribution.dto';

@Component({
  selector: 'app-backup-contribution',
  templateUrl: './backup-contribution.component.html',
  styleUrls: ['./backup-contribution.component.css']
})

export class BackupContributionComponent extends PagedListingComponentBase<BackupContributionComponent> implements OnInit {
  // PERMISSIONS
  public Admin_Backup:string = PERMISSIONS_CONSTANT.Admin_Backup;
  public Projects_TrainingProjects_ProjectDetail_TabWeeklyReport:string = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport;
  public Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View:string = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View;
  public Projects_ProductProjects_ProjectDetail_TabWeeklyReport:string = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport;
  public Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View:string = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View;
  public Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport:string = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  public Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View:string = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;

  // branch filter
  public listBranchs: BranchDto[] = [];
  public selectedBranchIds: number[] = [];
  public selectedBranchIdsOld: number[] = [];
  public searchBranch: string = '';
  public selectedBranchIdsCr: number[] = [];
  public listBranchsId: number[] = [];

  // usertype filter
  public listUserTypes: ITypeEnum[] = [];
  public selectedUserTypes: number[] = [];
  public selectedUserTypesCr: number[] = [];
  public selectedUserTypesOld: number[] = [];
  public searchUserType: string = '';
  public listUserTypesId: number[] = [];

  // userlevel filter
  public listUserLevels: ITypeEnum[] = [];
  public selectedUserLevels: number[] = [];
  public selectedUserLevelsCr: number[] = [];
  public selectedUserLevelsOld: number[] = [];
  public searchUserLevel: string = '';
  public listUserLevelsId: number[] = [];

  public userContributions: IInfoMonthlyUserContribution[] = [];

  // expand collapse row
  public isExpands: { [id: number]: boolean } = {};
  public numberDataRow: number = 3;
  public showIconExpandCollapeAll: boolean;
  public isExpandAll: boolean = false;

  public monthYear: Moment = moment().startOf('month');

  protected list(_request: PagedRequestDto, pageNumber: number, _finishedCallback: Function): void {
    this.isLoading = true;
    const requestBody = new FilterMonthlyUserContributionDto(
      this.selectedBranchIds,
      this.selectedUserTypes,
      this.selectedUserLevels,
      this.monthYear.month() + 1,
      this.monthYear.year()
    );
    this.backupUserContributionService.GetAllBackupMonthlyUserContribution(requestBody)
      .pipe(catchError(this.backupUserContributionService.handleError))
      .subscribe(data => {
        this.userContributions = data.result.items;
        this.showPaging(data.result, pageNumber);
        const startIndex = (this.pageNumber - 1) * this.pageSizeType;
        const endIndex = Math.min(startIndex + this.pageSizeType - 1, this.totalItems);
        this.showIconExpandCollapeAll = this.userContributions.slice(startIndex, endIndex)
          .some(item =>
            (item.projectContributes?.length > this.numberDataRow)
          );
        this.isExpands = {};
        this.isExpandAll = false;
        this.isLoading = false;
      });
  }
  
  protected delete(_entity: BackupContributionComponent): void {
  }

  constructor(injector: Injector,
    private branchService: BranchService,
    private backupUserContributionService: BackupUserContributionService
  ) { super(injector) }

  @ViewChild("selectBranch") selectBranch: { close: () => void; };
  @ViewChild("selectUserType") selectUserType: { close: () => void; };
  @ViewChild("selectUserLevel") selectUserLevel: { close: () => void; };
  
  ngOnInit(): void {
    this.getAllBranchs();
    this.getAllUserTypes();
    this.getAllUserLevels();
    this.refresh();
  }

  getAllBranchs(): void {
    this.branchService.getAllNotPagging()
      .pipe(catchError(this.branchService.handleError))
      .subscribe(data => {
        this.listBranchs = data.result as BranchDto[];
        this.listBranchsId = this.listBranchs.map(item => item.id);
      });
  }

  getAllUserTypes(): void {
    this.listUserTypes = Utils.mapEnumToList(this.APP_ENUM.UserTypeTabAllResource);
    this.listUserTypesId = this.listUserTypes.map(item => item.id);
  }

  getAllUserLevels(): void {
    this.listUserLevels = Utils.mapEnumToList(this.APP_ENUM.UserLevel);
    this.listUserLevelsId = this.listUserLevels.map(item => item.id);
  }

  openedChange(isOpen: boolean, field: string): void {
    if (!isOpen) {
      switch (field) {
        case 'Branch':
          this.selectedBranchIds = this.selectedBranchIdsCr = this.selectedBranchIdsOld;
          this.searchBranch = '';
          break;
        case 'UserType':
          this.selectedUserTypes = this.selectedUserTypesCr = this.selectedUserTypesOld;
          this.searchUserType = '';
          break;
        case 'UserLevel':
          this.selectedUserLevels = this.selectedUserLevelsCr = this.selectedUserLevelsOld;
          this.searchUserLevel = '';
          break;
      }
    }
  }

  actionSelect(event: IEventObject): void {
    switch (event.type) {
      case 'Branch':
        this.selectedBranchIds = this.selectedBranchIdsCr = event.data;
        break;
      case 'UserType':
        this.selectedUserTypes = this.selectedUserTypesCr = event.data;
        break;
      case 'UserLevel':
        this.selectedUserLevels = this.selectedUserLevelsCr = event.data;
        break;
    }
  }

  selectDone(field: string): void {
    switch (field) {
      case 'Branch':
        this.selectedBranchIdsOld = this.selectedBranchIds;
        this.selectBranch.close();
        break;
      case 'UserType':
        this.selectedUserTypesOld = this.selectedUserTypes;
        this.selectUserType.close();
        break;
      case 'UserLevel':
        this.selectedUserLevelsOld = this.selectedUserLevels;
        this.selectUserLevel.close();
        break;
    }
    this.pageNumber = 1;
    this.refresh();
  }

  onSelectChange(listSelect: number[], id: number): number[] {
    if (listSelect.includes(id)) {
      return listSelect.filter(res => res != id);
    }
    else {
      listSelect.push(id);
      return listSelect;
    }
  }

  onSelectChangeBranch(id: number): void {
    this.selectedBranchIdsCr = this.selectedBranchIds = this.onSelectChange(this.selectedBranchIdsCr, id);
    this.listBranchs = this.orderList(this.listBranchs, this.selectedBranchIds);
  }

  onSelectChangeUserType(id: number): void {
    this.selectedUserTypesCr = this.selectedUserTypes = this.onSelectChange(this.selectedUserTypesCr, id);
    this.listUserTypes = this.orderList(this.listUserTypes, this.selectedUserTypes);
  }

  onSelectChangeUserLevel(id: number): void {
    this.selectedUserLevelsCr = this.selectedUserLevels = this.onSelectChange(this.selectedUserLevelsCr, id);
    this.listUserLevels = this.orderList(this.listUserLevels, this.selectedUserLevels);
  }

  orderList(listAll: any[], listIdSelect: number[]): any[] {
    const selectedSet = new Set(listIdSelect);
    const listSelect: any[] = [];
    const listUnSelect: any[] = [];
    for (const item of listAll) {
      if (selectedSet.has(item.id)) {
        listSelect.push(item);
      } else {
        listUnSelect.push(item);
      }
    }
    return [...listSelect, ...listUnSelect];
  }

  viewProjectDetail(project: IProjectContribute): void {
    let routingToUrl: string = '';
    let projectPermissionKey = '';
    switch (project.projectType) {
      case 5:
        projectPermissionKey = this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport;
        routingToUrl = this.getProjectDetailUrl(
          projectPermissionKey,
          this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View,
          "/app/training-project-detail/training-weekly-report",
          "/app/training-project-detail/training-project-general"
        );
        break;
      case 3:
        projectPermissionKey = this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport;
        routingToUrl = this.getProjectDetailUrl(
          projectPermissionKey,
          this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View,
          "/app/product-project-detail/product-weekly-report",
          "/app/product-project-detail/product-project-general"
        );
        break;
      default:
        projectPermissionKey = this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
        routingToUrl = this.getProjectDetailUrl(
          projectPermissionKey,
          this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View,
          "/app/list-project-detail/weeklyreport",
          "/app/list-project-detail/list-project-general"
        );
        break;
    }
    const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], {
      queryParams: {
        id: project.id,
        type: project.projectType,
        projectName: project.projectName,
        projectCode: project.projectCode
      }
    }));
    window.open(url, '_blank');
  }

  getProjectDetailUrl(permissionKey: string, viewPermissionKey: string, permissionUrl: string, defaultUrl: string): string {
    return (this.permission.isGranted(permissionKey) && this.permission.isGranted(viewPermissionKey))
      ? permissionUrl
      : defaultUrl;
  }

  expandCollapseAll(): void {
    this.isExpandAll = !this.isExpandAll;
    this.userContributions.forEach(item => {
      this.isExpands[item.employee.id] = this.isExpandAll;
    });
  }

  expandCollapseDataRow(id: number): void {
    this.isExpands[id] = !this.isExpands[id];
  }

  getRowSpan(userContribution: IInfoMonthlyUserContribution): number {
    const projectCount = userContribution.projectContributes?.length;
    if (projectCount <= this.numberDataRow) {
      return projectCount + 1;
    }
    return this.isExpands[userContribution.employee.id] ? projectCount + 1 : this.numberDataRow + 1;
  }

  onDateChange(newTime: Moment): void {
    this.monthYear = newTime;
    this.pageNumber = 1;
    this.refresh();
  }

  backupData(): void {
    this.isLoading = true;
    const time = this.formatDateYMD(this.monthYear);
    this.backupUserContributionService.BackupMonthlyUserContribution(time)
      .pipe(catchError(this.backupUserContributionService.handleError))
      .subscribe(() => {
        this.pageNumber = 1;
        this.refresh();
        this.isLoading = false;
        this.notify.success(`Data backup for ${this.monthYear.format('MM/YYYY')} was successful.`);
      },
      () => {
        this.isLoading = false;
      });
  }

  pageSizeChange(newPageSize: number) {
    this.pageSizeType = newPageSize;
    this.changePageSize();
  }
}
