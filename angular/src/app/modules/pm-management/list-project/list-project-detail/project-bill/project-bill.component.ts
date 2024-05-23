import { result } from 'lodash-es';
import { AddSubInvoiceDialogComponent } from './add-sub-invoice-dialog/add-sub-invoice-dialog.component';
import { ParentInvoice, SubInvoice } from './../../../../../service/model/bill-info.model';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { UserService } from '@app/service/api/user.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppComponentBase } from '@shared/app-component-base';
import { UserDto } from '@shared/service-proxies/service-proxies';
import { projectUserBillDto, ProjectRateDto } from './../../../../../service/model/project.dto';
import { ProjectUserBillService } from './../../../../../service/api/project-user-bill.service';
import { Component, OnInit, Injector, ViewChildren, QueryList, ChangeDetectorRef, ViewChild } from '@angular/core';
import * as moment from 'moment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditNoteDialogComponent } from './add-note-dialog/edit-note-dialog.component';
import { DropDownDataDto } from '@shared/filter/filter.component';
import { ProjectDto } from '@app/service/model/list-project.dto';
import { ListProjectService } from '@app/service/api/list-project.service';
import { InvoiceSettingDialogComponent } from '@app/modules/pm-management/list-project/list-project-detail/project-bill/invoice-setting-dialog/invoice-setting-dialog.component';
import { ProjectInvoiceSettingDto } from '@app/service/model/project-invoice-setting.dto';
import { UpdateInvoiceDto } from '@app/service/model/updateInvoice.dto';
import { MatDialog } from '@angular/material/dialog';
import { ShadowAccountDialogComponent } from './shadow-account-dialog/shadow-account-dialog.component';
import { Observable, concat } from 'rxjs';
import { SortableModel } from '@shared/components/sortable/sortable.component';
import { ChargeStatusFilter } from '@app/service/model/project-process-criteria-result.dto';
import { MatSelect } from '@angular/material/select';
import { Pipe, PipeTransform } from '@angular/core';
import { optionDto } from '@shared/components/multiple-select/multiple-select.component';
import * as _ from 'lodash';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';


@Component({
  selector: 'app-project-bill',
  templateUrl: './project-bill.component.html',
  styleUrls: ['./project-bill.component.css']
})

export class ProjectBillComponent extends AppComponentBase implements OnInit {
  public userBillList: projectUserBillDto[] = [];
  private filteredUserBillList: projectUserBillDto[] = [];

  public filteredChargeRoles: any;
  public userForUserBill: UserDto[] = [];
  public userIdOld: number;
  sortColumn: string;
  sortDirect: number;
  iconSort: string;
  public parentInvoice: ParentInvoice = new ParentInvoice();
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
  public accountName;
  public billRole;
  private projectId: number
  public projectUserBillId: number
  public userBillCurrentPage: number = 1
  public rateInfo = {} as ProjectRateDto;
  public lastInvoiceNumber;
  public discount;
  public isEditLastInvoiceNumber: boolean = false;
  public isEditDiscount: boolean = false;
  public maxBillUserCurrentPage = 10;
  public totalBillList: number;
  public sortable = new SortableModel("", 0, "");
  @ViewChildren("sortThead") private elementRefSortable: QueryList<any>;
  public sortResource = {};
  public invoiceSettingOptions = Object.entries(this.APP_ENUM.InvoiceSetting).map((item) => ({
    key: item[0],
    value: item[1]
  }))
  public expandInvoiceSetting: true;
  public ChargeStatusFilter = ChargeStatusFilter;
  public selectedIsCharge: ChargeStatusFilter = ChargeStatusFilter.IsCharge;
  public chargeTypeList = [{ name: 'Daily', value: 0 }, { name: 'Monthly', value: 1 }, { name: 'Hourly', value: 2 }];

  public listProjectOfClient: SubInvoice[] = []
  public listSelectProject: DropDownDataDto[] = []
  public currentProjectInfo: ProjectDto
  public projectInvoiceSetting: ProjectInvoiceSettingDto;
  public updateInvoiceDto: UpdateInvoiceDto = {} as UpdateInvoiceDto;

  public selectedChargeRole: string[] = [];
  public listSelectChargeRole: string[] = [];

  public selectedLinkedResources: number[] = [];
  public listSelectLinkedResources: optionDto[] = [];
  public isHideRates:boolean = false;

  public listAllResource = []
  public listAvailableResource = []

  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount

  constructor(private router: Router,
    private projectUserBillService: ProjectUserBillService,
    private route: ActivatedRoute,
    injector: Injector,
    private userService: UserService,
    private _modalService: BsModalService,
    private dialog: MatDialog,
    private projectService: ListProjectService,
    private resourceManagerService: ResourceManagerService) {
    super(injector)
    this.projectId = Number(this.route.snapshot.queryParamMap.get("id"));
  }

  ngOnInit(): void {
    this.getUserBill();
    this.GetChargeRoleData();
    this.GetLinkedResourcesData();
    this.getParentInvoice();
    this.getAllProject();
    this.getCurrentProjectInfo();
    this.getProjectBillInfo();
    this.getListUserAndResources();
  }

  isShowInvoiceSetting(){
    return this.isGranted(PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_View)
  }

  canEditInvoiceSetting(){
    return this.isGranted(PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit)
  }

  getRate() {
    this.projectUserBillService.getRate(this.projectId).subscribe(data => {
      this.rateInfo = data.result;
    })
  }
  getLastInvoiceNumber() {
    this.projectUserBillService.getLastInvoiceNumber(this.projectId).subscribe(data => {
      this.lastInvoiceNumber = data.result;
    })
  }

  updateLastInvoiceNumber() {
    let data = {
      projectId: this.projectId,
      lastInvoiceNumber: this.lastInvoiceNumber,
    }
    if (+this.lastInvoiceNumber <= 0) {
      abp.message.error(this.l("Last Invoice Number must be bigger than 0!"));
      this.getLastInvoiceNumber();
      return;
    }
    this.projectUserBillService.updateLastInvoiceNumber(data).subscribe(data => {
      this.lastInvoiceNumber = data.result;
      abp.notify.success(`Updated Last Invoice Number`);
      this.isEditLastInvoiceNumber = false;
    })
  }

  getDiscount() {
    this.projectUserBillService.getDiscount(this.projectId).subscribe(data => {
      this.discount = data.result;
    })
  }
  updateDiscount() {
    let data = {
      projectId: this.projectId,
      discount: this.discount,
    }
    if (+this.discount < 0) {
      abp.message.error(this.l("Discount must be bigger than or equal to 0!"));
      this.getLastInvoiceNumber();
      return;
    }
    this.projectUserBillService.updateDiscount(data).subscribe(data => {
      this.discount = data.result;
      this.isEditDiscount = false;
      abp.notify.success(`Updated Discount`)
    })
  }

  //#region Integrate Finfast
  showAddSubInvoiceDialog(): void {
    const subInvoiceDialog = this._modalService.show(AddSubInvoiceDialogComponent, {
      class: 'modal',
      initialState: {
        projectId: this.projectId,
        subInvoices: this.parentInvoice.subInvoices
      }
    })
  }
  private getParentInvoice(): void {
    this.projectUserBillService.getParentInvoice(this.projectId)
      .subscribe(response => {
        if (!response.success) return;
        this.parentInvoice = response.result;
      });
  }
  //#endregion

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
          billRate: userBill.billRate,
          startTime: userBill.startTime,
          endTime: userBill.endTime,
          note: userBill.note,
          shadowNote: userBill.shadowNote,
          isActive: userBill.isActive,
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
          this.isLoading = false
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
  }
  private getUserBill(id?: number, status?: boolean, userIdNew?: number): void {
    this.isLoading = true;
    const body = {
        projectId: this.projectId,
        chargeStatusFilter: this.selectedIsCharge,
        linkedResourcesFilter: this.selectedLinkedResources,
        chargeRoleFilter: this.selectedChargeRole,
        searchText: this.searchText,
    };

    this.projectUserBillService.getAllUserBill(body).pipe(
        catchError(this.projectUserBillService.handleError)
    ).subscribe(data => {
        this.userBillList = data.result.map(item => {
            if (item.id === id && userIdNew) {
                return { ...item, createMode: status, userId: userIdNew };
            }
            return { ...item, createMode: false };
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
        let updated = data.result;
        this.filteredUserBillList = this.filteredUserBillList.map(item => (item.userId === updated.userId ? updated : item));
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

  cancelLastInvoiceNumber() {
    this.isEditLastInvoiceNumber = false;
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

  getAllProject() {
    this.projectUserBillService.getAllProjectCanUsing(this.projectId).subscribe(rs => {
      this.listProjectOfClient = rs.result
      this.listSelectProject = this.listProjectOfClient.map(item => ({
        displayName: item.projectName,
        value: item.projectId
      }))
    })
  }

  getCurrentProjectInfo(){
    this.projectService.getProjectById(this.projectId).subscribe(rs => {
      this.currentProjectInfo = rs.result
    })
  }

  getProjectBillInfo(){
    this.projectUserBillService.getBillInfo(this.projectId).subscribe((rs) => {
      this.projectInvoiceSetting = rs.result;
      this.updateInvoiceDto = {
        discount: rs.result.discount,
        invoiceNumber: rs.result.invoiceNumber,
        isMainProjectInvoice: rs.result.isMainProjectInvoice,
        mainProjectId: rs.result.mainProjectId,
        projectId: this.projectId,
        subProjectIds: rs.result.subProjectIds,
      }
      this.rateInfo = {
        currencyName: rs.result.currencyName
      } as ProjectRateDto
      this.discount = rs.result.discount,
      this.lastInvoiceNumber = rs.result.invoiceNumber
    })
  }

  public addLinkResource(userBill: projectUserBillDto): void {
    let listLinkedResourceId = userBill.linkedResources.map(item => item.id);
    this.listAvailableResource = this.listAllResource.filter(resource => !listLinkedResourceId.includes(resource.id));
    userBill.createLinkResourceMode = true;
    this.userBillProcess = true;
    this.showSearchAndFilter = false;
    this.isAddingResource = true;
  }

  public saveLinkResource(userBill): void {
    this.isLoading = true
    const reqAdd = {
      projectUserBillId: userBill.id,
      userId: this.selectedResource
    };
    
    this.projectUserBillService.LinkOneProjectUserBillAccount(reqAdd).pipe(
      catchError(this.projectUserBillService.handleError)
    ).subscribe(() => {
      abp.notify.success("Linked resources updated successfully");
      this.getUpdatedProjectUserBill(userBill.id)
      userBill.createLinkResourceMode = false;
      this.selectedResource = null
      this.userBillProcess = false;
      this.searchResource = "";
      this.showSearchAndFilter = true;
      this.isAddingResource = false;
      this.isLoading = false;
    }, () => { this.isLoading = false; });
  }

  public cancelLinkResource(userBill): void {
    userBill.createLinkResourceMode = false;
    this.selectedResource = null
    this.userBillProcess = false;
    this.searchResource = "";
    this.showSearchAndFilter = true;
    this.isAddingResource = false;
  }

  openInvoiceSettingDialog(){
    const editDialogRef = this.dialog.open(InvoiceSettingDialogComponent, {
      width: '700px',
      data: {
        dialogData: {
          projectId: this.projectId,
          projectName: this.currentProjectInfo.name,
          updateInvoiceDto: this.updateInvoiceDto
        }
      }
    })
    editDialogRef.afterClosed().subscribe(() => {
      this.getProjectBillInfo();
      this.getParentInvoice();
    })
  }

  routingProject(projectId, projectName, projectCode){
    let routingToUrl:string = (this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport)
    && this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View))
   ? "/app/list-project-detail/weeklyreport" : "/app/list-project-detail/list-project-general"
   // this.router.navigate([routingToUrl],{queryParams:{
   //   id: project.id,
   //   type: project.projectType,
   //   projectName: project.name,
   //   projectCode: project.code}
   // })

   const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], { queryParams: {
     id: projectId,
     projectName: projectName,
     projectCode: projectCode} }));
      window.open(url, '_blank');
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
}

export interface AddSubInvoicesDto {
  parentInvoiceId: number,
  subInvoiceIds: number[]
}
