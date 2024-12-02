import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { BranchService } from '@app/service/api/branch.service';
import { BranchDto } from '@app/service/model/branch.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError } from 'rxjs/operators';
import { GetAllWillPoolResourceDto, ShortInfoProjectDto } from '@app/service/model/will-pool.dto';
import { LISTWILLPOOL } from './data';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';

@Component({
  selector: 'app-will-pool',
  templateUrl: './will-pool.component.html',
  styleUrls: ['./will-pool.component.css']
})
export class WillPoolComponent extends PagedListingComponentBase<any> implements OnInit {
  // PERMISSIONS
  Resource_TabWillPool = PERMISSIONS_CONSTANT.Resource_TabWillPool;
  Resource_TabWillPool_EditNote = PERMISSIONS_CONSTANT.Resource_TabAllResource_EditNote;
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport;
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;
  // branch filter
  public listBranchs: BranchDto[] = [];
  public selectedBranchIds: number[] = [];
  public selectedBranchIdsOld: number[] = [];
  public searchBranch: string = '';
  public listBranchsId: number[] = [];
  public selectedBranchIdsCr: number[] = [];
  // usertype filter
  public listUserTypes: IUserType[] = [];
  public selectedUserTypes: number[] = [];
  public selectedUserTypesCr: number[] = [];
  public selectedUserTypesOld: number[] = [];
  public searchUserType: string = '';
  // end charge date filter
  public endChargeDateFromValue: Date = new Date();
  public endChargeDateToValue: Date = new Date(this.endChargeDateFromValue);
  // list data will pool
  public listWillPool: GetAllWillPoolResourceDto[] = LISTWILLPOOL;
  public listWillPoolBeforeSort: GetAllWillPoolResourceDto[] = LISTWILLPOOL;
  // sort filter
  private readonly sortProperties = {
    Contribute: 1,
    EndChargeDate: 2
  }
  public fieldSortDirection: { [key: number]: boolean } = {
    [this.sortProperties.Contribute]: false,
    [this.sortProperties.EndChargeDate]: false
  };
  // expand collapse row
  public isExpands: { [id: number]: boolean } = {};
  public numberDataRow: number = 3;
  public showIconExpandCollapeAll: boolean;
  public isExpandAll: boolean = false;
  // edit note
  public isEditNote: { [id: number]: boolean } = {};
  public originalNoteValue: string = "";

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
  }
  protected delete(entity: WillPoolComponent): void {
  }

  constructor(public injector: Injector,
    private branchService: BranchService,
    private resourceService: ResourceManagerService
  ) {
    super(injector);
  }

  @ViewChild("selectBranch") selectBranch: { close: () => void; };
  @ViewChild("selectUserType") selectUserType: { close: () => void; };

  ngOnInit(): void {
    this.endChargeDateToValue.setMonth(this.endChargeDateFromValue.getMonth() + 1);
    this.getAllBranchs();
    this.getAllUserTypes();
    this.sortDataByEndChargeDate(this.fieldSortDirection[this.sortProperties.Contribute]);
    this.sortDataByTotalContribute(this.fieldSortDirection[this.sortProperties.EndChargeDate]);
    this.showIconExpandCollapeAll = this.listWillPool.some(item => 
      (item.projects?.length > this.numberDataRow || 
       item.accounts?.length > this.numberDataRow)
    );
    this.totalItems = 6;
    this.refresh();
  }

  getAllBranchs() {
    this.branchService.getAllNotPagging()
      .pipe(catchError(this.branchService.handleError))
      .subscribe((data) => {
        this.listBranchs = data.result as BranchDto[];
        this.listBranchsId = this.listBranchs.map(branch => branch.id)
        this.selectedBranchIds = this.listBranchs.map(item => item.id)
        this.selectedBranchIdsOld = [...this.selectedBranchIds]
        this.selectedBranchIdsCr = this.selectedBranchIds
      });
  }

  openedChange(isOpen: boolean, field: string) {
    if (!isOpen) {
      switch (field) {
        case 'Branch':
          this.selectedBranchIds = [...this.selectedBranchIdsOld]
          this.selectedBranchIdsCr = [...this.selectedBranchIdsOld]
          this.searchBranch = '';
          break;
        case 'UserType':
          this.selectedUserTypes = [...this.selectedUserTypesOld]
          this.selectedUserTypesCr = [...this.selectedUserTypesOld]
          this.searchUserType = '';
          break;
      }
    }
  }

  actionSelect(event: IEventObject) {
    switch (event.type) {
      case 'Branch':
        this.selectedBranchIds = event.data
        this.selectedBranchIdsCr = event.data
        break;
      case 'UserType':
        this.selectedUserTypes = event.data
        this.selectedUserTypesCr = event.data
        break;
    }
  }

  selectDone(field: string) {
    switch (field) {
      case 'Branch':
        this.selectedBranchIdsOld = this.selectedBranchIds
        this.selectBranch.close()
        break;
      case 'UserType':
        this.selectedUserTypesOld = this.selectedUserTypes
        this.selectUserType.close()
        break;
    }
  }

  onSelectChangeBranch(id: number) {
    const branch = this.onSelectChange(this.selectedBranchIdsCr, id)
    this.selectedBranchIdsCr = branch
    this.selectedBranchIds = [...branch]
    this.listBranchs = this.orderList(this.listBranchs, this.selectedBranchIds)
  }

  getAllUserTypes() {
    this.listUserTypes = Object.entries(this.APP_ENUM.UserTypeTabAllResource).map((item) => {
      return {
        displayName: item[0],
        value: item[1],
      };
    });
    this.listUserTypesId = this.listUserTypes.map(item => item.value);
    this.selectedUserTypes = this.listUserTypes.map(item => item.value);
    this.selectedUserTypesOld = [...this.selectedUserTypes];
    this.selectedUserTypesCr = this.selectedUserTypes;
  }

  onSelectChangeUserType(id: number) {
    const userType = this.onSelectChange(this.selectedUserTypesCr, id)
    this.selectedUserTypesCr = userType
    this.selectedUserTypes = [...userType]
    this.listUserTypes = this.orderList(this.listUserTypes, this.selectedUserTypes)
  }

  onClickSortDirection(field: number) {
    this.fieldSortDirection[field] = !this.fieldSortDirection[field];
    switch(field) {
      case this.sortProperties.Contribute:
        this.sortDataByTotalContribute(this.fieldSortDirection[this.sortProperties.Contribute]);
        break;
      case this.sortProperties.EndChargeDate:
        this.sortDataByEndChargeDate(this.fieldSortDirection[this.sortProperties.EndChargeDate]);
        break;
    }
  }

  sortDataByTotalContribute(direction: boolean) {
    this.listWillPool = this.listWillPool.sort((a, b) => {
      return direction
        ? a.totalContribute - b.totalContribute
        : b.totalContribute - a.totalContribute;
    });
  }

  sortDataByEndChargeDate(direction: boolean) {
    const defaultGetTime = 1;
    this.listWillPool.forEach(item => {
      item.accounts.sort((a, b) => {
        const getValidTimestamp = (dateString: string): number => {
          const date = new Date(dateString);
          return isNaN(date.getTime()) ? defaultGetTime : date.getTime();
        };
        const dateA = getValidTimestamp(a.endChargeDate);
        const dateB = getValidTimestamp(b.endChargeDate);
        return direction 
          ? dateA - dateB
          : dateB - dateA;
      })
    })
  }

  expandCollapseDataRow(id: number) {
    this.isExpands[id] = !this.isExpands[id];
  }

  expandCollapseAll() {
    this.isExpandAll = !this.isExpandAll;
    this.listWillPool.forEach(item => {
      this.isExpands[item.resource.id] = this.isExpandAll;
    });
  }

  getCollapseLine(willPool: GetAllWillPoolResourceDto): number {
    const maxLengthClassNumber = 5;
    const defaultCollapseLine = 3;
    const maxLength = Math.max(willPool.projects?.length, willPool.accounts?.length);
    if (this.isExpands[willPool.resource.id]) {
      return maxLength > this.numberDataRow ? maxLength : this.numberDataRow + maxLengthClassNumber;
    }
    return defaultCollapseLine;
  }

  getRowSpanAccount(willPool: GetAllWillPoolResourceDto): number {
    const accountCount = willPool.accounts?.length;
    if (accountCount <= this.numberDataRow) {
      return accountCount + 1;
    }
    return this.isExpands[willPool.resource.id] ? accountCount + 1 : this.numberDataRow + 1;
  }

  onEditNote(willPool: GetAllWillPoolResourceDto) {
    const resourceId = willPool.resource.id;
    const currentNote = willPool.resourceNote;
    this.isEditNote[resourceId] = !this.isEditNote[resourceId];
    if (this.isEditNote[resourceId]) {
      this.originalNoteValue = currentNote;
    } else {
      willPool.resourceNote = this.originalNoteValue;
      this.originalNoteValue = "";
    }
  }

  updateNoteResource(willPool: GetAllWillPoolResourceDto) {
    const requestBody = { userId: willPool.resource.id, note: willPool.resourceNote };
    this.resourceService.updatePoolNote(requestBody).subscribe({
      next: () => {
        abp.notify.success("Update Note Successful");
        this.isEditNote[willPool.resource.id] = false;
        this.originalNoteValue = "";
      },
      error: (err: { message: string; }) => {
        abp.notify.error("Update Failed: " + (err.message || "An error occurred"));
      }
    });
  }

  viewProjectDetail(project: ShortInfoProjectDto) {
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

  getProjectDetailUrl(permissionKey: string, viewPermissionKey: string, permissionUrl: string, defaultUrl: string) {
    return (this.permission.isGranted(permissionKey) && this.permission.isGranted(viewPermissionKey))
      ? permissionUrl
      : defaultUrl;
  }
}

export interface IEventObject {
  type: string,
  data: number[]
}

export interface IUserType {
  displayName: string,
  value: number
}
