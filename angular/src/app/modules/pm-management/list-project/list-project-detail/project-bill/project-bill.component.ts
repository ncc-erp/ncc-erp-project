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
import { Component, OnInit, Injector, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
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
import { ChargeStatus } from '@app/service/model/project-process-criteria-result.dto';
import { MatSelect } from '@angular/material/select';


@Component({
  selector: 'app-project-bill',
  templateUrl: './project-bill.component.html',
  styleUrls: ['./project-bill.component.css']
})

export class ProjectBillComponent extends AppComponentBase implements OnInit {
  // public theadTable: THeadTable[] = [
  //   { name: "#" },
  //   { name: "Employee", sortName: "emailAddress", defaultSort: "" },
  //   { name: "Charge Name" },
  //   { name: "Charge Role", sortName: "billRole", defaultSort: "" },
  //   { name: "Linked resources" },
  //   { name: "Rate", sortName: "billRate", defaultSort: "" },
  //   { name: "Charge Type" },
  //   { name: "Is Charge", sortName: "startTime", defaultSort: "" },
  //   { name: "Note", width: "200px" },
  //   { name: "Action", width: "100px" },
  // ];
  public userBillList: projectUserBillDto[] = [];
  private filteredUserBillList: projectUserBillDto[] = [];
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
  public searchUserBill: string = "";
  public searchByEmail: string = "";
  public searchBillCharge: number
  private projectId: number
  public projectUserBillId: number
  public userBillCurrentPage: number = 1
  public rateInfo = {} as ProjectRateDto;
  public lastInvoiceNumber;
  public discount;
  public chargeTypeList = [{ name: 'Daily', value: 0 }, { name: 'Monthly', value: 1 }, { name: 'Hourly', value: 2 }];
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
  public ChargeStatus = ChargeStatus;
  public selectedIsCharge: ChargeStatus = ChargeStatus.IsCharge;
  public listProjectOfClient: SubInvoice[] = []
  public listSelectProject: DropDownDataDto[] = []
  public currentProjectInfo: ProjectDto
  public projectInvoiceSetting: ProjectInvoiceSettingDto;
  public updateInvoiceDto: UpdateInvoiceDto = {} as UpdateInvoiceDto;

  public selectedChargeName: any;
  public selectedChargeRole: any;
  public listSelectChargeName: any;
  public listSelectChargeRole: any;

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
    private ref: ChangeDetectorRef,
    private route: ActivatedRoute,
    injector: Injector,
    private userService: UserService,
    private _modalService: BsModalService,
    private dialog: MatDialog,
    private projectService: ListProjectService) {
    super(injector)
    this.projectId = Number(this.route.snapshot.queryParamMap.get("id"));
  }

  ngOnInit(): void {
    this.getUserBill();
    this.getParentInvoice();
    this.getAllProject();
    this.getCurrentProjectInfo();
    this.getProjectBillInfo();
    this.searchEmail();
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

  private getAllFakeUser(userId?) {
    this.projectUserBillService.GetAllUserActive(this.projectId, userId, false, true).pipe(catchError(this.userService.handleError)).subscribe(data => {
      // this.userForProjectUser = data.result;
      this.userForUserBill = data.result;
    })
  }
  public removeLinkResource(projectId, projectUserBillId, userId, id){
    // this.isLoading = true
    const req = {
      projectUserBillId: id,
      userIds: [userId]
    }
    const status = this.userBillList.find(item => item.id === id).createMode
    abp.message.confirm(
      "Remove linked resource?",
      "",
      (result: boolean) => {
        if (result) {
          this.isLoading = true;
          this.projectUserBillService.RemoveUserFromBillAccount(req).pipe(catchError(this.projectUserBillService.handleError)).subscribe(data => {
            abp.notify.success(`Linked Resource Removed Successfully!`)
             this.getUserBill(id, status);
          }, () => {
            this.isLoading = false
          })
        }
      }
    )
  }
  public addUserBill(): void {
    this.getAllFakeUser('')
    let newUserBill = {} as projectUserBillDto
    newUserBill.createMode = true;
    newUserBill.isActive = true;
    this.userBillProcess = true;
    this.filteredUserBillList.unshift(newUserBill)
    if( newUserBill.createMode == true){
      this.filteredUserBillList.length = this.filteredUserBillList.length;
    }

  }
  public saveUserBill(userBill: projectUserBillDto): void {
    delete userBill["createMode"]
    userBill.startTime = moment(userBill.startTime).format("YYYY-MM-DD");
    if (userBill.endTime) {
      userBill.endTime = moment(userBill.endTime).format("YYYY-MM-DD");
    }
    this.isLoading = true

    if (!this.isEditUserBill) {
      userBill.projectId = this.projectId
      this.projectUserBillService.create(userBill).pipe(catchError(this.projectUserBillService.handleError)).subscribe(res => {
        abp.notify.success(`Created new user bill`)
        this.getUserBill()
        this.userBillProcess = false;
        this.searchUserBill = ""
      }, () => {
        userBill.createMode = true;
        this.isLoading = false
      })
    }
    else {
      if(this.userIdOld == userBill.userId){
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
          this.getUserBill()
          this.userBillProcess = false;
          this.isEditUserBill = false;
          this.searchUserBill = ""
      },
        () => {
          userBill.createMode = true;
          this.isLoading = false
        }
        )
      }
      else {
        const reqAdd = {
          projectUserBillId: userBill.id,
          userIds: userBill.linkedResources ? userBill.linkedResources.map(item => item.id): []
        }
        const reqDelete = {
          projectUserBillId: this.userIdOld,
          userIds: userBill.linkedResources ? userBill.linkedResources.map(item => item.id) : []
        }
        concat(this.projectUserBillService.RemoveUserFromBillAccount(reqDelete),this.projectUserBillService.update(userBill),this.projectUserBillService.LinkUserToBillAccount(reqAdd))
        .pipe(catchError(this.projectUserBillService.handleError))
        .subscribe(() => {
            abp.notify.success("Update successfully")
            this.getUserBill()
            this.userBillProcess = false;
            this.isEditUserBill = false;
            this.searchUserBill = ""
        },
          () => {
            userBill.createMode = true;
            this.isLoading = false
          })
      }
    }
  }
  // private filterProjectUserDropDown() {

  //   let userProjectList = this.projectUserList.map(item => item.userId)
  //   this.userForProjectUser = this.userForUserBill.filter(user => userProjectList.indexOf(user.id) == -1)
  // }
  public cancelUserBill(): void {
    this.getUserBill();
    this.userBillProcess = false;
    this.isEditUserBill = false;
    this.searchUserBill = ""
  }
  public editUserBill(userBill: projectUserBillDto): void {
    this.getAllFakeUser(userBill.userId)
    this.userIdOld = userBill.userId
    userBill.createMode = true;
    this.userBillProcess = true;
    this.isEditUserBill = true;
    // userBill.billRole = this.APP_ENUM.ProjectUserRole[userBill.billRole];
  }
  private getUserBill( id?: number,status?: boolean, userIdNew?: number): void {
    this.isLoading = true
    const body = {
      projectId: this.projectId,
      chargeStatus: this.selectedIsCharge
    }
    this.projectUserBillService.getAllUserBill(body).pipe(catchError(this.projectUserBillService.handleError)).subscribe(data => {
      this.userBillList = data.result.map(item=> {
      if(item.id === id && userIdNew){
        return {...item,createMode:status,userId:userIdNew}
      }
      return {...item, createMode:false}
      })
      const distinctSelectChargeName = new Set(data.result.map(item => item.accountName));
      const distinctSelectChargeRole = new Set(data.result.map(item => item.billRole));
      this.listSelectChargeName =  Array.from(distinctSelectChargeName).sort(this.customSort);
      this.listSelectChargeRole = Array.from(distinctSelectChargeRole).sort(this.customSort);

      this.filteredUserBillList = this.userBillList.slice();
      this.isLoading = false
    }, ()=>{ this.isLoading = false })
  }

  customSort(a: string, b: string): number {
    return a.localeCompare(b, 'en', { sensitivity: 'accent' });
  }
  filterByIsCharge() {
    this.getUserBill()
    this.userBillProcess = false;
    this.isEditUserBill = false;
    this.searchUserBill = "";
    this.sortColumn = "";
  }

  filterChargeName() {
    this.filteredUserBillList = this.userBillList.filter(item => this.selectedChargeName.includes(item.accountName) || this.selectedChargeName.length === 0);
  }

  filterChargeRole() {
      this.filteredUserBillList = this.userBillList.filter(item => this.selectedChargeRole.includes(item.billRole) || this.selectedChargeRole.length === 0);
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
    const reqDelete = {
      projectUserBillId: userBill.id,
      userIds: userBill.linkedResources.map(item => item.id)
    }
    abp.message.confirm(
      "Delete user bill?",
      "",
      (result: boolean) => {
        if (result) {
          this.isLoading = true
          concat(this.projectUserBillService.RemoveUserFromBillAccount(reqDelete),this.projectUserBillService.deleteUserBill(userBill.id))
          .pipe(catchError(this.projectUserBillService.handleError)).subscribe(()=>{
                abp.notify.success("Delete Bill account success")
                this.getUserBill()
          })
        }
      }
    );
  }
  public focusOut() {
    this.searchUserBill = '';
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

  handleOpenDialogShadowAccount(projectId, userId, listResource, id)
  {
    const show = this.dialog.open(ShadowAccountDialogComponent, {
      data: {
        projectId: projectId,
        userId: this.userIdOld != userId && this.isEditUserBill ? this.userIdOld : userId,
        listResource: listResource ? listResource.map(item=> item.id) : [],
        userIdNew: userId,
        projectUserBillId: id
      },
      width: "700px",
    })

    const status = this.userBillList.find(item => item.id === id).createMode

    show.afterClosed().subscribe((res) => {
      if (res.isSave) {
        this.getUserBill(id,status,res.userIdNew);
      }
    })
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
  styleThead(item: any) {
    return {
      width: item.width,
      height: item.height,
    };
  }

  sortTable(event: any) {
    this.sortable = event;
    this.changeSortableByName(
      this.sortable.sort,
      this.sortable.typeSort,
      this.sortable.sortDirection
    );
    this.getUserBill();
  }


  changeSortableByName(sort: string, sortType: string, sortDirection?: number) {
    if (!sortType) {
      delete this.sortResource[sort];
    } else {
      this.sortResource[sort] = sortDirection;
    }
    this.ref.detectChanges();
  }

  sortData(data) {
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
        this.iconSort = "fas fa-sort-amount-down";  // Descending sort
        this.sortDesc(this.sortColumn);
    } else if (this.sortDirect == 0) {
        this.iconSort = "fas fa-sort-amount-up";    // Ascending sort
        this.sortAsc(this.sortColumn);
    } else {
        this.iconSort = "fas fa-sort";
        this.filteredUserBillList = this.userBillList.slice();
      }
}

  sortAsc(sortColumn: string){
    this.filteredUserBillList.sort((a,b) => (typeof a[sortColumn] === "number") ? a[sortColumn]-b[sortColumn] : (a[sortColumn] ?? "").localeCompare(b[sortColumn] ?? ""));
  }
  sortDesc(sortColumn: string){
    this.filteredUserBillList.sort((a,b) => (typeof a[sortColumn] === "number") ? b[sortColumn]-a[sortColumn] : (b[sortColumn] ?? "").localeCompare(a[sortColumn] ?? ""));
  }

  searchEmail() {
    // Chuyển đổi giá trị nhập vào thành chữ thường để so sánh không phân biệt chữ hoa chữ thường
    const searchValue = this.searchByEmail.toLowerCase().trim();

    // Nếu giá trị tìm kiếm không tồn tại hoặc rỗng, hiển thị lại danh sách đầy đủ
    if (!searchValue) {
      this.filteredUserBillList = this.userBillList.slice();
      return;
    }

    // Lọc danh sách userBillList theo email nhập vào
    this.filteredUserBillList = this.userBillList.filter(item =>
      item.emailAddress.toLowerCase().includes(searchValue)
    );
  }
}



export interface AddSubInvoicesDto {
  parentInvoiceId: number,
  subInvoiceIds: number[]
}

export class THeadTable {
  name: string;
  width?: string = "auto";
  height?: string = "auto";
  backgroud_color?: string;
  sortName?: string;
  defaultSort?: string;
  padding?: string;
  whiteSpace?: string;
}

export class SendRecruitmentModel {
  id: number;
  name: string;
  dmNote: string;
  pmNote: string;
}
