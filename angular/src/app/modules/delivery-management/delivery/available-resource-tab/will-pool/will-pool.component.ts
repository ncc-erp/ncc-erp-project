import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { BranchService } from '@app/service/api/branch.service';
import { BranchDto } from '@app/service/model/branch.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError } from 'rxjs/operators';
import { GetAllWillPoolResourceDto, InputGetAllWillPoolResourceDto, ShortInfoProjectDto } from '@app/service/model/will-pool.dto';
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
  public selectedBranchIdsCr: number[] = [];
  public listBranchsId: number[] = [];
  // usertype filter
  public listUserTypes: IUserType[] = [];
  public selectedUserTypes: number[] = [];
  public selectedUserTypesCr: number[] = [];
  public selectedUserTypesOld: number[] = [];
  public searchUserType: string = '';
  public listUserTypesId: number[] = [];
  // end charge date filter
  public endChargeDateFromValue: Date;
  public endChargeDateToValue: Date;
  // list data will pool
  public listWillPool: GetAllWillPoolResourceDto[] = [];
  // sort filter
  private readonly sortProperties = {
    Contribute: 1,
    EndChargeDate: 2
  };
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
  // search username
  public searchText: string = "";
  public oldSearchText: string = "";

  protected list(_request: PagedRequestDto, pageNumber: number, _finishedCallback: Function): void {
    this.isLoading = true;
    const requestBody = new InputGetAllWillPoolResourceDto();
    requestBody.userName = this.searchText;
    requestBody.branchIds = this.selectedBranchIds;
    requestBody.userTypes = this.selectedUserTypes;
    requestBody.endChargeDateFrom = this.endChargeDateFromValue;
    requestBody.endChargeDateTo = this.endChargeDateToValue;
    this.resourceService.GetAllWillPoolResource(requestBody)
      .pipe(catchError(this.resourceService.handleError))
      .subscribe(data => {
        this.listWillPool = data.result.items;
        this.sortDataByTotalContribute(this.fieldSortDirection[this.sortProperties.Contribute]);
        this.sortDataByEndChargeDate(this.fieldSortDirection[this.sortProperties.EndChargeDate]);
        this.showPaging(data.result, pageNumber);
        this.showIconExpandCollapeAll = this.listWillPool.some(item => 
          (item.projects?.length > this.numberDataRow || 
           item.accounts?.length > this.numberDataRow)
        );
        this.isLoading = false;
      });
  }

  protected delete(_entity: WillPoolComponent): void {
  }

  constructor(public injector: Injector,
    private branchService: BranchService,
    private resourceService: ResourceManagerService
  ) {
    super(injector);
  }

  @ViewChild("selectBranch") selectBranch: { close: () => void; };
  @ViewChild("selectUserType") selectUserType: { close: () => void; };

  async ngOnInit(): Promise<void> {
    await this.getAllBranchs();
    this.initialEndChargeDate();
    this.getAllUserTypes();
    this.refresh();
  }

  getAllBranchs(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.branchService.getAllNotPagging()
        .pipe(catchError(this.branchService.handleError))
        .subscribe(data => {
          this.listBranchs = data.result as BranchDto[];
          const listBranchIds = this.listBranchs.map(item => item.id);
          this.selectedBranchIds = this.selectedBranchIdsCr = this.selectedBranchIdsOld = this.listBranchsId = listBranchIds;
          resolve();
        }, error => {
          reject(error);
        });
    });
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
    }
    this.pageNumber = 1;
    this.refresh();
  }

  onSelectChangeBranch(id: number): void {
    this.selectedBranchIdsCr = this.selectedBranchIds = this.onSelectChange(this.selectedBranchIdsCr, id);
    this.listBranchs = this.orderList(this.listBranchs, this.selectedBranchIds);
  }

  getAllUserTypes(): void {
    this.listUserTypes = Object.entries(this.APP_ENUM.UserTypeTabAllResource).map((item) => {
      return {
        displayName: item[0],
        value: item[1],
      };
    });
    const listUserTypeValues = this.listUserTypes.map(item => item.value);
    this.selectedUserTypes = this.selectedUserTypesOld = this.selectedUserTypesCr = this.listUserTypesId = listUserTypeValues;
  }

  onSelectChangeUserType(id: number): void {
    this.selectedUserTypesCr = this.selectedUserTypes = this.onSelectChange(this.selectedUserTypesCr, id);
    this.listUserTypes = this.orderList(this.listUserTypes, this.selectedUserTypes);
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

  onClickSortDirection(field: number): void {
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

  sortDataByTotalContribute(direction: boolean): void {
    this.listWillPool = this.listWillPool.sort((a, b) => {
      return direction
        ? a.totalContribute - b.totalContribute
        : b.totalContribute - a.totalContribute;
    });
  }

  sortDataByEndChargeDate(direction: boolean): void {
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

  expandCollapseDataRow(id: number): void {
    this.isExpands[id] = !this.isExpands[id];
  }

  expandCollapseAll(): void {
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

  onEditNote(willPool: GetAllWillPoolResourceDto): void {
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

  updateNoteResource(willPool: GetAllWillPoolResourceDto): void {
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

  viewProjectDetail(project: ShortInfoProjectDto): void {
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

  filterEndChargeDate(): void {
    this.endChargeDateFromValue = this.formatDateToYYYYMMdd(this.endChargeDateFromValue);
    this.endChargeDateToValue = this.formatDateToYYYYMMdd(this.endChargeDateToValue);
    if(this.endChargeDateFromValue > this.endChargeDateToValue) {
      abp.notify.error("End Charge Date From must be less than the End Charge Date To");
      this.initialEndChargeDate();
      return;
    }
    this.pageNumber = 1;
    this.refresh();
  }

  filterUsername(): void {
    if(this.searchText != this.oldSearchText) {
      this.oldSearchText = this.searchText;
      this.pageNumber = 1;
      this.refresh();
    }
  }

  initialEndChargeDate(): void {
    this.endChargeDateFromValue = new Date();
    this.endChargeDateToValue = new Date(this.endChargeDateFromValue);
    this.endChargeDateToValue.setMonth(this.endChargeDateFromValue.getMonth() + 1);
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
