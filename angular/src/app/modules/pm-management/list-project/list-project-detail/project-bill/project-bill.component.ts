import { AddSubInvoiceDialogComponent } from './add-sub-invoice-dialog/add-sub-invoice-dialog.component';
import { ParentInvoice, SubInvoice } from './../../../../../service/model/bill-info.model';
import { ActivatedRoute } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { UserService } from '@app/service/api/user.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppComponentBase } from '@shared/app-component-base';
import { UserDto } from '@shared/service-proxies/service-proxies';
import { projectUserBillDto, ProjectRateDto } from './../../../../../service/model/project.dto';
import { ProjectUserBillService } from './../../../../../service/api/project-user-bill.service';
import { Component, OnInit, Injector } from '@angular/core';
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


@Component({
  selector: 'app-project-bill',
  templateUrl: './project-bill.component.html',
  styleUrls: ['./project-bill.component.css']
})
export class ProjectBillComponent extends AppComponentBase implements OnInit {
  public userBillList: projectUserBillDto[] = [];
  private filteredUserBillList: projectUserBillDto[] = [];
  public userForUserBill: UserDto[] = [];
  public parentInvoice: ParentInvoice = new ParentInvoice();
  public isEditUserBill: boolean = false;
  public userBillProcess: boolean = false;
  public panelOpenState: boolean = false;
  public isShowUserBill: boolean = false;
  public searchUserBill: string = ""
  public searchBillCharge: number
  private projectId: number
  public userBillCurrentPage: number = 1
  public rateInfo = {} as ProjectRateDto;
  public lastInvoiceNumber;
  public discount;
  public chargeTypeList = [{ name: 'Daily', value: 0 }, { name: 'Monthly', value: 1 }, { name: 'Hourly', value: 2 }];
  public isEditLastInvoiceNumber: boolean = false;
  public isEditDiscount: boolean = false;
  public maxBillUserCurrentPage = 10;
  public totalBillList: number;
  public invoiceSettingOptions = Object.entries(this.APP_ENUM.InvoiceSetting).map((item) => ({
    key: item[0],
    value: item[1]
  }))
  public expandInvoiceSetting: true;
  public selectedIsCharge: string = "Charge" ;
  public listProjectOfClient: SubInvoice[] = []
  public listSelectProject: DropDownDataDto[] = []
  public currentProjectInfo: ProjectDto
  public projectInvoiceSetting: ProjectInvoiceSettingDto;
  public updateInvoiceDto: UpdateInvoiceDto = {} as UpdateInvoiceDto;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View;
  Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit;
  constructor(
    private projectUserBillService: ProjectUserBillService,
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
    this.getAllFakeUser();
    this.getParentInvoice();
    this.getAllProject();
    this.getCurrentProjectInfo();
    this.getProjectBillInfo();

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

  private getAllFakeUser() {
    this.userService.GetAllUserActive(false, true).pipe(catchError(this.userService.handleError)).subscribe(data => {
      // this.userForProjectUser = data.result;
      this.userForUserBill = data.result;
    })
  }
  public addUserBill(): void {
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

    if (!this.isEditUserBill) {
      userBill.projectId = this.projectId
      this.projectUserBillService.create(userBill).pipe(catchError(this.projectUserBillService.handleError)).subscribe(res => {

        abp.notify.success(`Created new user bill`)
        if(this.selectedIsCharge === 'Not Charge'){
          this.getUserBill("Not Charge");
        }else if (this.selectedIsCharge === 'All') {
          this.getUserBill("All");
        }else{
          this.getUserBill("Charge");
        }
        this.userBillProcess = false;
        this.searchUserBill = ""
      }, () => {
        userBill.createMode = true
      })
    }
    else {
      this.projectUserBillService.update(userBill).pipe(catchError(this.projectUserBillService.handleError)).subscribe(res => {
        abp.notify.success(`Updated request user bill`)
        // if (userBill.isActive === true) {
        //   this.selectedIsCharge = 'Charge';
        //   this.getUserBill("Charge");
        // } else {
        //   this.selectedIsCharge = 'Not Charge'
        //   this.getUserBill("Not Charge");
        // }
        if(this.selectedIsCharge === 'Not Charge'){
          this.getUserBill("Not Charge");
        }else if (this.selectedIsCharge === 'All') {
          this.getUserBill("All");
        }else{
          this.getUserBill("Charge");
        }
        this.userBillProcess = false;
        this.isEditUserBill = false;
        this.searchUserBill = ""
      },
        () => {
          userBill.createMode = true;
        })
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
    userBill.createMode = true;
    this.userBillProcess = true;
    this.isEditUserBill = true;
    // userBill.billRole = this.APP_ENUM.ProjectUserRole[userBill.billRole];
  }
  private getUserBill(selectedIsCharge?: string): void {
    this.projectUserBillService.getAllUserBill(this.projectId).pipe(catchError(this.projectUserBillService.handleError)).subscribe(data => {
      this.userBillList = data.result
      this.filteredUserBillList = this.userBillList.filter(bill => bill.isActive === true);
      if (selectedIsCharge === 'All') {
        this.filteredUserBillList = this.userBillList;
      } else if (selectedIsCharge === 'Charge') {
        this.filteredUserBillList = this.userBillList.filter(bill => bill.isActive === true);
      } else if (selectedIsCharge === 'Not Charge') {
        this.filteredUserBillList = this.userBillList.filter(bill => bill.isActive === false);
      }
    })
  }

  filterByIsCharge() {
    if (this.selectedIsCharge === 'All') {
      this.getUserBill("All");
    } else if (this.selectedIsCharge === 'Charge') {
      this.getUserBill("Charge");
    } else if (this.selectedIsCharge === 'Not Charge') {
      this.getUserBill("Not Charge");
    }
    this.userBillProcess = false;
    this.isEditUserBill = false;
    this.searchUserBill = ""
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
          this.projectUserBillService.deleteUserBill(userBill.id).pipe(catchError(this.projectUserBillService.handleError)).subscribe(() => {
            abp.notify.success("Deleted user bill");
            if(this.selectedIsCharge === 'Not Charge'){
              this.getUserBill("Not Charge");
            }else if (this.selectedIsCharge === 'All') {
              this.getUserBill("All");
            }else{
              this.getUserBill("Charge");
            }
            // this.selectedIsCharge = 'Charge';
            // this.getUserBill("Charge");
          });
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

  updateNote(id, fullName, projectName, note) {
    let editNoteDialog: BsModalRef;
    editNoteDialog = this._modalService.show(EditNoteDialogComponent, {
      class: 'modal',
      initialState: {
        id: id,
        fullName: fullName,
        projectName: projectName,
        note: note,
      },
    });

    editNoteDialog.content.onSave.subscribe(() => {
      this.getUserBill();
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


}

export interface AddSubInvoicesDto {
  parentInvoiceId: number,
  subInvoiceIds: number[]
}
