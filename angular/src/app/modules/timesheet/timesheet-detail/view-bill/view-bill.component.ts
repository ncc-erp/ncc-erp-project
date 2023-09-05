import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { UserDto } from './../../../../../shared/service-proxies/service-proxies';
import { UserService } from './../../../../service/api/user.service';
import { catchError } from 'rxjs/operators';
import { projectUserBillDto } from './../../../../service/model/project.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { TimesheetProjectBill } from './../../../../service/model/timesheet.dto';
import { TimeSheetProjectBillService } from './../../../../service/api/time-sheet-project-bill.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector } from '@angular/core';
import * as moment from 'moment';
import { UpdateAction } from '../timesheet-detail.component';

@Component({
  selector: 'app-view-bill',
  templateUrl: './view-bill.component.html',
  styleUrls: ['./view-bill.component.css']
})
export class ViewBillComponent extends AppComponentBase implements OnInit {
  billDetail: TimesheetProjectBill[] = []
  userForUserBill: UserDto[] = []
  searchUserBill: string = "";
  public isCreate: boolean = false;
  public isEdit: boolean = false;
  public isEdittingRows: boolean = false;
  tempUserList = []
  public chargeTypeList = [{name:'Daily', value: 0}, {name:'Monthly', value: 1}, {name:'Hourly', value: 2}];
  public updateAction = UpdateAction
  Timesheets_TimesheetDetail_UpdateBill_Edit = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill_Edit
  Timesheets_TimesheetDetail_UpdateBill_SetDone = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill_SetDone
  Timesheets_TimesheetDetail_ViewBillRate = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ViewBillRate
  Timesheets_TimesheetDetail_UpdateBill = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill
  Timesheets_TimesheetDetail_UpdateTimsheet = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateTimsheet
  Timesheets_TimesheetDetail_RemoveAccount = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_RemoveAccount

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<ViewBillComponent>, private userService: UserService,
    private timesheetProjectService: TimesheetProjectService,
    private timesheetProjectBillService: TimeSheetProjectBillService, injector: Injector) {
    super(injector)
  }

  ngOnInit(): void {
    this.getProjectBill();
    // this.getAllFakeUser(false);
  }
  public getProjectBill() {
    this.isLoading = true
    this.timesheetProjectBillService.getProjectBill(this.data.billInfo.projectId, this.data.billInfo.timesheetId)
    .subscribe(data => {
      this.billDetail = data.result
      this.isLoading = false
      //this.getAllFakeUser(false)
    },
      () => { this.isLoading = false })
  }


  public saveUserBill(tpb: TimesheetProjectBill): void {
    delete tpb["isEditing"];
    //tpb.isEditing = false;

    tpb.startTime = moment(tpb.startTime).format("YYYY-MM-DD");
    if (tpb.endTime) {
      tpb.endTime = moment(tpb.endTime).format("YYYY-MM-DD");
    }
    tpb.timesheetId = this.data.billInfo.timesheetId;
    tpb.projectId = this.data.billInfo.projectId;

    if (!tpb.id) {
      tpb.projectId = this.data.billInfo.projectId;
      delete tpb['userList'];
      this.timesheetProjectBillService.createProjectBill(tpb)
      .pipe(catchError(this.timesheetProjectBillService.handleError))
      .subscribe(res => {
        abp.notify.success(`Created successfull`);
        this.getProjectBill();
        this.searchUserBill = "";
        // this.getAllFakeUser(false)
      },
        () => {
          tpb.isEditing = true;
        })

    } else {
      let bill =
      [{
        "userId": tpb.userId,
        "billRole": tpb.billRole,
        "billRate": tpb.billRate,
        "note": tpb?.note,
        "isActive": tpb.isActive,
        "workingTime": tpb.workingTime,
        "id": tpb.id,
        "accountName": tpb.accountName,
        "chargeType": tpb.chargeType,
      }]
      this.timesheetProjectBillService.updateProjectBill(bill)
      .pipe(catchError(this.timesheetProjectBillService.handleError))
      .subscribe(res => {
        abp.notify.success(`Update successfull`)
        this.getProjectBill();
        this.searchUserBill = "";
        // this.getAllFakeUser(true)

      },
        () => {
          tpb.isEditing = true;
        })
    }


  }

  isComplete(e) {
    this.data.billInfo.isComplete = e.checked;
    const input = {
      isComplete: this.data.billInfo.isComplete,
      id: this.data.billInfo.id
    }

    this.timesheetProjectService.setComplete(input).subscribe(res => {
      abp.notify.success(`Update successfull`);
    })
  }

  saveAllUpdateBill() {
    let tpbList = this.billDetail.map((tpb) => {
      return {
        projectId: tpb.projectId,
        timeSheetId: this.data.billInfo.timesheetId,
        billAccountName: tpb.billAccountName,
        accountName: tpb.accountName,
        userId: tpb.userId,
        billRole: tpb.billRole,
        billRate: tpb.billRate,
        startTime: tpb.startTime,
        endTime: tpb.endTime,
        currency: tpb.currency,
        note: tpb?.note,
        shadowNote: tpb.shadowNote,
        isActive: tpb.isActive,
        workingTime: tpb.workingTime,
        id: tpb.id,
        chargeType: tpb.chargeType
      }
    })

    this.timesheetProjectBillService.updateProjectBill(tpbList)
    .pipe(catchError(this.timesheetProjectBillService.handleError))
    .subscribe(res => {
      abp.notify.success(`Update successfull`)
      this.getProjectBill();
      this.searchUserBill = "";
      this.isEdittingRows = false;
    })

  }
  public cancelUpdateAll(): void {
    this.getProjectBill();
    this.searchUserBill = "";
  }

  public editUserBill(tpb: TimesheetProjectBill): void {
    tpb.isEditing = true;
  }

  searchUser(bill) {
    bill.userList = this.tempUserList.filter(item =>
      (this.removeAccents(item?.fullName.toLowerCase().replace(/\s/g, "")).includes(this.removeAccents(bill.searchText.toLowerCase().replace(/\s/g, ""))) || this.removeAccents(item.email?.toLowerCase().replace(/\s/g, "")).includes(this.removeAccents(bill.searchText.toLowerCase().replace(/\s/g, "")))))
  }
  removeAccents(str) {
    var AccentsMap = [
      "aàảãáạăằẳẵắặâầẩẫấậ",
      "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
      "dđ", "DĐ",
      "eèẻẽéẹêềểễếệ",
      "EÈẺẼÉẸÊỀỂỄẾỆ",
      "iìỉĩíị",
      "IÌỈĨÍỊ",
      "oòỏõóọôồổỗốộơờởỡớợ",
      "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
      "uùủũúụưừửữứự",
      "UÙỦŨÚỤƯỪỬỮỨỰ",
      "yỳỷỹýỵ",
      "YỲỶỸÝỴ"
    ];
    for (var i = 0; i < AccentsMap.length; i++) {
      var re = new RegExp('[' + AccentsMap[i].substr(1) + ']', 'g');
      var char = AccentsMap[i][0];
      str = str.replace(re, char);
    }
    return str;
  }

  public onActiveChange(active, userBill) {
    userBill.isActive = active.checked
  }

  public create() {
    let bill = {} as TimesheetProjectBill;
    this.billDetail.unshift(bill)
    bill.isEditing = true;

  }

  editAllRow() {
    this.setEditingAllRow();
  }

  onUserSelect(bill) {
    bill.searchText = ""
  }

  private setViewAllRow(){
    this.billDetail.forEach(s => s.isEditing = false);
  }

  private setEditingAllRow(){
    this.billDetail.forEach(s => s.isEditing = true);
  }

  saveUpdateTS(data){
    let request = [{
      Id: data.id,
      workingTime: data.workingTime,
      isActive: data.isActive,
      note: data.note,
      accountName: data.accountName
    }]

    this.timesheetProjectBillService.updateTS(request).subscribe((response) =>{
      if(response.success){
        abp.notify.success(response.result)
        this.setViewAllRow();
        this.getProjectBill();
      }
      else{
        abp.notify.error(response.message)
      }
    })
  }


  protected removeAccountTS(tpb:TimesheetProjectBill): void {
    abp.message.confirm(
      "Remove account " + tpb.fullName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.timesheetProjectBillService.removeAccountTS(tpb.id)
          .pipe(catchError(this.timesheetProjectBillService.handleError))
          .subscribe((response) => {
            if(response.success){
              abp.notify.success("Remove successfull")
              this.getProjectBill();
            }
            else{
              abp.notify.error(response.message)
            }
          });
        }
      }
    );
  }


  saveAllUpdateTS(){
    let arr = this.billDetail.map((tpb) => {
      return {
        note: tpb?.note,
        isActive: tpb.isActive,
        workingTime: tpb.workingTime,
        id: tpb.id,
        accountName: tpb.accountName
      }
    })

    this.timesheetProjectBillService.updateTS(arr)
    .pipe(catchError(this.timesheetProjectBillService.handleError))
    .subscribe(res => {
      abp.notify.success(`Update successfull`)
      this.getProjectBill();
      this.searchUserBill = "";
      this.setViewAllRow()
    })
  }


  public isShowRate(){
    return this.isGranted(PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ViewBillRate)
     && this.data.action == this.updateAction.UpdateBillInfo
  }

  public isBillPopUp(){
    return this.data.action == this.updateAction.UpdateBillInfo;
  }

  public isTSPopUp(){
    return this.data.action == this.updateAction.UpdateTimesheet;
  }


  public isEditingAllRow(){
    return this.billDetail.find(s => !s.isEditing) == undefined;
  }

  public isEditingAnyRow(){
    return this.billDetail.find(s => s.isEditing) != undefined;
  }

  public isShowEditBtnOnRow(){
    return this.isGranted(this.Timesheets_TimesheetDetail_UpdateBill)
    || this.isGranted(this.Timesheets_TimesheetDetail_UpdateTimsheet)
  }

  public isShowEditAllBtn(){
    return !this.isEditingAllRow()
    && this.billDetail
    && this.billDetail.length
    && this.isShowEditBtnOnRow()
  }

  public getCurrencyName(){
    return this.billDetail && this.billDetail.length > 0 ? this.billDetail[0].currency : '';
  }

}
