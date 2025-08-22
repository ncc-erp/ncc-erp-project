
import { ActivatedRoute, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { UserService } from '../../../../../../app/service/api/user.service';
import { PERMISSIONS_CONSTANT } from '../../../../../../app/constant/permission.constant';
import { AppComponentBase } from '../../../../../../shared/app-component-base';
import { UserDto } from '../../../../../../shared/service-proxies/service-proxies';
import { projectUserBillDto, ProjectRateDto } from './../../../../../service/model/project.dto';
import { ProjectUserBillService } from './../../../../../service/api/project-user-bill.service';
import { Component, OnInit, Injector, ViewChildren, QueryList} from '@angular/core';
import * as moment from 'moment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditNoteDialogComponent } from '../../../list-project/list-project-detail/project-bill/add-note-dialog/edit-note-dialog.component';
import { DropDownDataDto } from '../../../../../../shared/filter/filter.component';
import { ProjectDto } from '../../../../../../app/service/model/list-project.dto';
import { ListProjectService } from '../../../../../../app/service/api/list-project.service';
import { MatDialog } from '@angular/material/dialog';
import { SortableModel } from '../../../../../../shared/components/sortable/sortable.component';
import { ChargeStatusFilter } from '../../../../../../app/service/model/project-process-criteria-result.dto';
import { MatSelect } from '@angular/material/select';
import { optionDto } from '../../../../../../shared/components/multiple-select/multiple-select.component';
import * as _ from 'lodash';
import { ResourceManagerService } from '../../../../../../app/service/api/resource-manager.service';
import { UpdateUserSkillDialogComponent } from '../../../../../../app/users/update-user-skill-dialog/update-user-skill-dialog.component';
import { AppConsts } from '../../../../../../shared/AppConsts';
import { UploadCvBillAccountComponent } from '../../../../../../shared/components/upload-cv-bill-account/upload-cv-bill-account.component';
import { GetCvBillAccountDto } from '../../../../../../app/service/model/upload-cv.dto';
import { FileHandlerService } from '../../../../../../app/service/utility/file-handler.service';
import { IGetUserInfo } from '../../../../../../app/service/model/user.inteface';

@Component({
  selector: 'app-project-bill',
  templateUrl: './product-account-info.component.html',
  styleUrls: ['./product-account-info.component.css']
})

export class ProductAccountInfoComponent extends AppComponentBase implements OnInit {
  public userBillList: projectUserBillDto[] = [];
  public filteredUserBillList: projectUserBillDto[] = [];
  public totalHeadCount:number;

  public filteredChargeRoles: any;
  public userForUserBill: UserDto[] = [];
  public userIdOld: number;
  sortColumn: string;
  sortDirect: number;
  iconSort: string;

  public isEditUserBill: boolean = false;
  public userBillProcess: boolean = false;
  public panelOpenState: boolean = false;
  public isShowUserBill: boolean = false;
  public showSearchAndFilter: boolean = true;
  public isAddingOrEditingUserBill: boolean = false;
  public isAddingResource: boolean = false;
  public searchUserBill: string = "";
  public searchResource: string = "";
  public searchText: string = "";
  public selectedResource: number | null = null;
  private projectId: number
  public projectUserBillId: number
  public userBillCurrentPage: number = 1
  public rateInfo = {} as ProjectRateDto;
  public isEditDiscount: boolean = false;
  public maxBillUserCurrentPage = 10;
  public totalBillList: number;
  public sortable = new SortableModel("", 0, "");
  @ViewChildren("sortThead") private elementRefSortable: QueryList<any>;
  public sortResource = {};
  public ChargeStatusFilter = ChargeStatusFilter;
  public selectedIsCharge: ChargeStatusFilter = ChargeStatusFilter.IsCharge;
  public chargeTypeList = [{ name: 'Daily', value: 0 }, { name: 'Monthly', value: 1 }, { name: 'Hourly', value: 2 }];

  public listSelectProject: DropDownDataDto[] = [];
  public currentProjectInfo: ProjectDto;

  public selectedChargeRole: string[] = [];
  public listSelectChargeRole: string[] = [];

  public selectedLinkedResources: number[] = [];
  public listSelectLinkedResources: optionDto[] = [];
  // public isHideRates:boolean = false;

  public listAllResource: UserDto[] = [];
  public listAvailableResource: UserDto[] = [];

  editingRows: { [key: number]: { [key: number]: { [key: string]: boolean } } } = {};
  originalContribute: { [key: number]: { [key: number]: { [key: string]: number } } } = {};

  private numberSkill: number = 3;
  private isViewAllUserSkill: { [userId: number] : boolean } = {};

  private oldUserBill: projectUserBillDto | null = null;

  Projects_ProductProjects_ProjectDetail_TabBillInfo_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabBillInfo_View;
  Projects_ProductProjects_ProjectDetail_TabBillInfo_Create = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabBillInfo_Create;
  Projects_ProductProjects_ProjectDetail_TabBillInfo_Edit = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabBillInfo_Edit;
  Projects_ProductProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount;
  Resource_TabAllResource_ViewUserStarSkill = PERMISSIONS_CONSTANT.Resource_TabAllResource_ViewUserStarSkill;
  Resource_TabAllResource_UpdateSkill = PERMISSIONS_CONSTANT.Resource_TabAllResource_UpdateSkill;

  constructor(private router: Router,
    private projectUserBillService: ProjectUserBillService,
    private route: ActivatedRoute,
    injector: Injector,
    private userService: UserService,
    private _modalService: BsModalService,
    private dialog: MatDialog,
    private projectService: ListProjectService,
    private resourceManagerService: ResourceManagerService,
    private fileHandlerService: FileHandlerService) {
    super(injector)
    this.projectId = Number(this.route.snapshot.queryParamMap.get("id"));
  }

  ngOnInit(): void {
    this.getUserBill();
    this.GetChargeRoleData();
    this.GetLinkedResourcesData();
    this.getCurrentProjectInfo();
    this.getListUserAndResources();
  }

  getRate() {
    this.projectUserBillService.getRate(this.projectId).subscribe(data => {
      this.rateInfo = data.result;
    })
  }

  private getListUserAndResources() {
    this.resourceManagerService.GetListAllUserShortInfo().pipe(catchError(this.userService.handleError)).subscribe(data => {
      this.userForUserBill = data.result;
      this.listAllResource = this.userForUserBill.filter(item => item.isActive)
    })
  }
  public removeLinkResource(userId, id){
    const req = {
      projectUserBillId: id,
      userIds: [userId]
    }
    abp.message.confirm(
      "Remove linked resource?",
      "",
      (result: boolean) => {
        if (result) {
          this.isLoading = true;
          this.projectUserBillService.RemoveLinkedResource(req).pipe(catchError(this.projectUserBillService.handleError)).subscribe(data => {
            abp.notify.success(`Linked Resource Removed Successfully!`)
            this.filteredUserBillList = this.filteredUserBillList.map(userBill => {
              if (userBill.id === id) {
                  userBill.linkedResources = userBill.linkedResources.filter(linkedResource => linkedResource.id !== userId);
              }
              return userBill;
            });
            this.isLoading = false;
          }, () => {
            this.isLoading = false
          })
        }
      }
    )
  }
  public addUserBill(): void {
    let newUserBill = {} as projectUserBillDto
    newUserBill.createMode = true;
    newUserBill.isActive = true;
    this.userBillProcess = true;
    this.filteredUserBillList.unshift(newUserBill);
    this.showSearchAndFilter = false;
    this.isAddingOrEditingUserBill = true;

  }
  public saveUserBill(userBill: projectUserBillDto): void {
    this.showSearchAndFilter = true;
    this.isAddingOrEditingUserBill = false;
    userBill.startTime = moment(userBill.startTime).format("YYYY-MM-DD");
    if (userBill.endTime) {
      userBill.endTime = moment(userBill.endTime).format("YYYY-MM-DD");
    }
    const existingUserBill = this.userBillList.find(item => item.userId === userBill.userId);
    if (!this.isEditUserBill) {
      if (existingUserBill) {
        abp.message.confirm(
          "This user bill already exists. Do you want to continue?",
          "",
          (result: boolean) => {
            if (result) {
              this.createUserBill(userBill);
            } else {
              this.userBillProcess = true;
              this.showSearchAndFilter = false;
              this.isAddingOrEditingUserBill = true;
              userBill.initialIsExpose = userBill.isExpose;
            }
          }
        );
      } else {
        this.createUserBill(userBill);
      }
    }
    else {
      if(this.userIdOld == userBill.userId){
        this.isLoading = true
        const userBillToUpdate = {
          projectId : userBill.projectId,
          userId: userBill.userId,
          billRole: userBill.billRole,
          billRate: userBill.billRate || 0,
          headCount: userBill.headCount || 0,
          startTime: userBill.startTime,
          endTime: userBill.endTime,
          note: userBill.note,
          shadowNote: userBill.shadowNote,
          isActive: userBill.isActive,
          isExpose: userBill.isExpose,
          accountName: userBill.accountName,
          chargeType: userBill.chargeType,
          linkedResources: userBill.linkedResources,
          id: userBill.id
        }
        this.projectUserBillService.update(userBillToUpdate).pipe(catchError(this.projectUserBillService.handleError)).subscribe(()=>{
          abp.notify.success("Update successfully")
          this.getUpdatedProjectUserBill(userBill.id);
          this.userBillProcess = false;
          this.isEditUserBill = false;
          this.searchUserBill = "";
          delete userBill["createMode"];
      },
        () => {
          userBill.createMode = true;
          this.isLoading = false;
        }
        )
      }
      else {
        if (existingUserBill) {
          abp.message.confirm(
            "This user bill already exists. Do you want to continue?",
            "",
            (result: boolean) => {
              if (result) {
                this.updateUserBill(userBill);
              } else {
                this.userBillProcess = true;
                this.isEditUserBill = true;
                this.showSearchAndFilter = false;
                this.isAddingOrEditingUserBill = true;
              }
            }
          );
        } else {
          this.updateUserBill(userBill);
        }
      }
    }
  }

  createUserBill(userBill: projectUserBillDto): void {
    this.isLoading = true;
    userBill.projectId = this.projectId;
    this.projectUserBillService.create(userBill).pipe(
      catchError(error => {
        this.userBillProcess = true;
        this.showSearchAndFilter = false;
        this.isAddingOrEditingUserBill = true;
        return this.projectUserBillService.handleError(error);
      })
    ).subscribe(
        () => {
            abp.notify.success(`Created new user bill`);
            this.getUserBill();
            this.userBillProcess = false;
            this.searchUserBill = "";
            delete userBill["createMode"];
        },
        () => {
            userBill.createMode = true;
            this.isLoading = false;
        }
    );
  }

  updateUserBill(userBill: projectUserBillDto): void {
    this.isLoading = true
    this.projectUserBillService.update(userBill).pipe(
        catchError(error => {
        this.userBillProcess = true;
        this.isEditUserBill = true;
        this.showSearchAndFilter = false;
        this.isAddingOrEditingUserBill = true;
        return this.projectUserBillService.handleError(error);
      })
    ).subscribe(() => {
        abp.notify.success("Update successfully")
        this.getUserBill()
        this.userBillProcess = false;
        this.isEditUserBill = false;
        this.searchUserBill = "";
        delete userBill["createMode"];
    },
      () => {
        userBill.createMode = true;
        this.isLoading = false
      })
  }

  public cancelUserBill(userBill): void {
    if (!this.isEditUserBill) {
      let index = this.filteredUserBillList.indexOf(userBill);
      if (index !== -1) {
          this.filteredUserBillList.splice(index, 1);
      }
    }
    Object.assign(userBill, this.oldUserBill);
    userBill.isExpose = userBill.initialIsExpose;
    this.oldUserBill = null;
    userBill.createMode = false;
    this.userBillProcess = false;
    this.isEditUserBill = false;
    this.searchUserBill = "";
    this.showSearchAndFilter = true;
    this.isAddingOrEditingUserBill = false;
  }
  public editUserBill(userBill: projectUserBillDto): void {
    this.userIdOld = userBill.userId
    userBill.createMode = true;
    this.userBillProcess = true;
    this.isEditUserBill = true;
    this.showSearchAndFilter = false;
    this.isAddingOrEditingUserBill = true;
    userBill.isExpose = userBill.initialIsExpose;
    this.oldUserBill = { ...userBill };
  }
  private getUserBill(id?: number, status?: boolean, userIdNew?: number): void {
    this.isLoading = true;
    const body = {
        projectId: this.projectId,
        linkedResourcesFilter: this.selectedLinkedResources,
        searchText: this.searchText,
        isAccountInfoTab: true
    };

    this.projectUserBillService.getAllUserBill(body).pipe(
        catchError(this.projectUserBillService.handleError)
    ).subscribe(data => {
      this.totalHeadCount = data.result.reduce((sum, item) => sum + item.headCount, 0);
        this.userBillList = data.result.map(item => {
            if (item.id === id && userIdNew) {
                return { ...item, createMode: status, userId: userIdNew, initialIsExpose: item.isExpose };
            }
            return { ...item, createMode: false, contribute: 0, initialIsExpose: item.isExpose };
        });

        this.filteredUserBillList = _.cloneDeep(this.userBillList);
        this.isLoading = false;
    }, () => { this.isLoading = false; });
  }

  getUpdatedProjectUserBill(id: number){
    this.isLoading = true;
    this.projectUserBillService.GetProjectUserBillById(id).pipe(
      catchError(this.projectUserBillService.handleError)
    ).subscribe(data => {
        let updated = data.result as projectUserBillDto;
        this.filteredUserBillList = this.filteredUserBillList.map(item => {
          if (item.id === id) {
            return { ...updated, initialIsExpose: updated.isExpose };
          }
          return {...item, initialIsExpose: item.isExpose};
        });
        this.isLoading = false;
    }, () => { this.isLoading = false; });
  }

  GetChargeRoleData(){
    this.projectUserBillService.GetAllChargeRoleByProject(this.projectId).subscribe(data => {
      this.listSelectChargeRole = data.result;
    })
  }

  GetLinkedResourcesData(){
    this.projectUserBillService.GetAllLinkedResourcesByProject(this.projectId).subscribe(data => {
      this.listSelectLinkedResources = data.result.map(item => {
        return {
          id: item.id,
          name: `${item.fullName} (${item.emailAddress})`
        };
      });
    })
  }

  filterByIsCharge() {
    this.getUserBill()
    this.userBillProcess = false;
    this.isEditUserBill = false;
    this.searchUserBill = "";
    this.sortColumn = "";
  }

  filterByChargeType() {
    this.getUserBill();
  }

  selectAll(select: MatSelect) {
    select.value = this.getSelectableOptions(select);
    this.updateSelectedValues(select);
  }

  clearAll(select: MatSelect) {
      select.value = [];
      this.updateSelectedValues(select);
  }

  updateSelectedValues(select: MatSelect) {
      select.writeValue(select.value);
      // Trigger the selectionChange event manually
      select._onChange(select.value);
  }

  getSelectableOptions(select: MatSelect): any[] {
      const allOptions = select.options.toArray();
      return allOptions.filter(option => !option.disabled).map(option => option.value);
  }

  downloadFile(id: number){
    this.projectUserBillService.DownloadCVLink(id).subscribe(data => {
      this.fileHandlerService.downloadFile(data.result.data, data.result.fileName);
    });
  }

  openInNewTab(event: MouseEvent, id: any){
    event.preventDefault();
    if(id){
      this.projectUserBillService.DownloadCVLink(id).subscribe(data => {
        this.fileHandlerService.downloadFile(data.result.data, data.result.fileName);
      })
      window.open('_blank');
    }
  }

  changePageSizeCurrent()
  {
    this.userBillCurrentPage = 1
  }

  public removeUserBill(userBill: projectUserBillDto): void {
    abp.message.confirm(
      "Delete user bill?",
      "",
      (result: boolean) => {
        if (result) {
          this.isLoading = true
          this.projectUserBillService.deleteUserBill(userBill.id)
          .pipe(catchError(this.projectUserBillService.handleError)).subscribe(()=>{
            abp.notify.success("Delete Bill account success")
            this.filteredUserBillList = this.filteredUserBillList.filter(user => user.id !== userBill.id);
            this.isLoading = false;
          })
        }
      }
    );
  }
  public focusOut() {
    this.searchUserBill = '';
    this.searchResource = '';
  }

  cancelDiscount() {
    this.isEditDiscount = false;
  }

  public editBillNote(bill): any {
    let ref = this.dialog.open(EditNoteDialogComponent, {
      width: "600px",
      data: {
        id: bill.id,
        note:bill.note
      }
    });

    ref.afterClosed().subscribe(rs => {
      if (rs) {
       bill.note =rs;
      }
    });
  }

  getCurrentProjectInfo(){
    this.projectService.getProjectById(this.projectId).subscribe(rs => {
      this.currentProjectInfo = rs.result
    })
  }

  public addLinkResource(userBill: projectUserBillDto): void {
    let listLinkedResourceId = userBill.linkedResources.map(item => item.id);
    this.listAvailableResource = this.listAllResource.filter(resource => !listLinkedResourceId.includes(resource.id));
    userBill.createLinkResourceMode = true;
    this.userBillProcess = true;
    this.showSearchAndFilter = false;
    this.isAddingResource = true;
    userBill.contribute = 0;
  }

  public saveLinkResource(userBill: projectUserBillDto): void {
    const reqAdd = {
      projectUserBillId: userBill.id,
      userId: this.selectedResource,
      contribute: userBill.contribute || 0
    };

    this.projectUserBillService.LinkOneProjectUserBillAccount(reqAdd).pipe(
      catchError(this.projectUserBillService.handleError)
    ).subscribe((data) => {
      const userInfo: IGetUserInfo = data.result;
      abp.notify.success("Linked resources updated successfully");
      userBill.linkedResources.push(userInfo);
      userBill.createLinkResourceMode = false;
      this.selectedResource = null
      this.userBillProcess = false;
      this.searchResource = "";
      this.showSearchAndFilter = true;
      this.isAddingResource = false;
    });
  }

  public cancelLinkResource(userBill): void {
    userBill.createLinkResourceMode = false;
    this.selectedResource = null
    this.userBillProcess = false;
    this.searchResource = "";
    this.showSearchAndFilter = true;
    this.isAddingResource = false;
    userBill.contribute = 0;
  }

  sortData(data) {
    if (!this.showSearchAndFilter) {
      return;
  }
    if (this.sortColumn !== data) {
        this.sortDirect = -1;
    }
    this.sortColumn = data;
    this.sortDirect++;
    if (this.sortDirect > 1) {
        this.iconSort = "";
        this.sortDirect = -1;
    }
    if (this.sortDirect == 1) {
        this.iconSort = "fas fa-sort-amount-down";
        this.sortDesc(this.sortColumn);
    } else if (this.sortDirect == 0) {
        this.iconSort = "fas fa-sort-amount-up";
        this.sortAsc(this.sortColumn);
    } else {
        this.iconSort = "fas fa-sort";
        this.filteredUserBillList = _.cloneDeep(this.userBillList);
      }
}

  sortAsc(sortColumn: string){
    this.filteredUserBillList.sort((a,b) => (typeof a[sortColumn] === "number") ? a[sortColumn]-b[sortColumn] : (a[sortColumn] ?? "").localeCompare(b[sortColumn] ?? ""));
  }
  sortDesc(sortColumn: string){
    this.filteredUserBillList.sort((a,b) => (typeof a[sortColumn] === "number") ? b[sortColumn]-a[sortColumn] : (b[sortColumn] ?? "").localeCompare(a[sortColumn] ?? ""));
  }

  orderLinkedResourcesOnTop(data: string) {
    if (!this.showSearchAndFilter) {
      return;
  }
    if (this.sortColumn !== data) {
        this.sortDirect = -1;
    }
    this.sortColumn = data;
    this.sortDirect++;
    if (this.sortDirect > 1) {
        this.iconSort = "";
        this.sortDirect = -1;
    }
    if (this.sortDirect == 1) {
        this.iconSort = "fas fa-sort-amount-down";
        this.sortLinkedResourcesDesc();
    } else if (this.sortDirect == 0) {
        this.iconSort = "fas fa-sort-amount-up";
        this.sortLinkedResourcesAsc();
    } else {
        this.iconSort = "fas fa-sort";
        this.filteredUserBillList = _.cloneDeep(this.userBillList);
    }
  }

  sortLinkedResourcesDesc() {
      this.filteredUserBillList.sort((a, b) => {
          return b.linkedResources.length - a.linkedResources.length;
      });
  }

  sortLinkedResourcesAsc() {
      this.filteredUserBillList.sort((a, b) => {
          return a.linkedResources.length - b.linkedResources.length;
      });
  }



  searchContext() {
    this.getUserBill();
  }

  onChangeListLinkedResourcesSelected(selectedLinkedResources: number[]) {
    this.selectedLinkedResources = selectedLinkedResources;
    this.getUserBill();
  }

  onChangeListChargeRoleSelected(selectedChargeRole: string[]) {
    this.selectedChargeRole = selectedChargeRole;
    this.getUserBill();
  }

  onCancelFilterLinkedResources() {
    this.selectedLinkedResources = [];
    this.getUserBill();
  }

  onCancelFilterChargeRole() {
    this.selectedChargeRole = []
    this.getUserBill();
  }
  refresh(){
    this.searchText = "";
    this.selectedIsCharge = ChargeStatusFilter.IsCharge;
    this.selectedLinkedResources = [];
    this.selectedChargeRole = [];
    this.getUserBill();
  }

  getStyleStatusUser(isActive: boolean){
    return isActive?"badge badge-pill badge-success":"badge badge-pill badge-danger"
  }

  getValueStatusUser(isActive: boolean){
    return isActive?"Active":"InActive"
  }

  edit(source: number, index: number, field: string, contribute: number): void {

    this.editingRows[source] = {};
    this.originalContribute[source] = {};
    this.editingRows[source][index] = { [field]: true };
    this.originalContribute[source] = { [index]: { [field]: contribute }};
  }

  cancelUpdate(source: number, resource: any, index: number): void {
    this.editingRows[source] = {};
    resource.contribute = this.originalContribute[source][index]?.contribute;
  }

  updateContribute(projectUserBillId: number, userId: number, contribute: number) {
    this.isLoading = true
    const reqAdd = {
      projectUserBillId,
      userId,
      contribute
    };

    this.projectUserBillService.UpdateLinkOneProjectUserBillAccount(reqAdd).pipe(
      catchError(this.projectUserBillService.handleError)
    ).subscribe(() => {
      abp.notify.success("Linked resources updated successfully");
      this.isLoading = false;
      this.editingRows[projectUserBillId] = {};
    }, () => { this.isLoading = false; });
  }

  expandCollapseUserSkill(billId: number) {
    this.isViewAllUserSkill[billId] = !this.isViewAllUserSkill[billId];
  }

  updateUserSkill(projectUserBill: projectUserBillDto, note: string) {
    let ref = this.dialog.open(UpdateUserSkillDialogComponent, {
      width: "700px",
      data: {
        userSkills: projectUserBill.userSkills,
        id: projectUserBill.id,
        fullName: projectUserBill.billAccountName,
        note: note,
        viewStarSkillUser: this.permission.isGranted(this.Resource_TabAllResource_ViewUserStarSkill),
        typeUpdate: AppConsts.UpdateUserSkillType.PROJECT
      }

    });
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh()
      }
    })
  }

  onConfirmUpdateIsExpose(userBill: projectUserBillDto) {
    this.updateUserBill(userBill);
    userBill.initialIsExpose = userBill.isExpose;
  }

  onCancelUpdateIsExpose(userBill: projectUserBillDto) {
    userBill.isExpose = userBill.initialIsExpose;
    this.userBillProcess = false;
    this.showSearchAndFilter = true;
  }

  onUpdateIsExpose(userBill: projectUserBillDto) {
    const hasChanged = userBill.isExpose !== userBill.initialIsExpose;
    this.userBillProcess = hasChanged;
    this.showSearchAndFilter = !hasChanged;
  }

  openUploadCvDialog(projectUserBill: projectUserBillDto): void {
    const dialogRef = this.dialog.open(UploadCvBillAccountComponent, {
      data: { ...projectUserBill } as GetCvBillAccountDto,
      width: '500px',
    });
    dialogRef.afterClosed().subscribe((result?: GetCvBillAccountDto) => {
      if (result) {
        this.filteredUserBillList = this.filteredUserBillList.map(item =>
          item.id === result.id ? { ...item, ...result } : item
        );
      }
    });
  }
}




