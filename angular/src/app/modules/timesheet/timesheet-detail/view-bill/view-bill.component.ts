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
  searchOtType: string = "";
  otTypeProcess: boolean = false;
  public isCreate: boolean = false;
  public isEdit: boolean = false;
  public isEdittingRows: boolean = false;
  tempUserList = []
  public chargeTypeList = [{name:'Daily', value: 0}, {name:'Monthly', value: 1}, {name:'Hourly', value: 2}];
  public updateAction = UpdateAction

  otTypeOptions = []

  Timesheets_TimesheetDetail_UpdateBill_Edit = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill_Edit
  Timesheets_TimesheetDetail_UpdateBill_SetDone = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill_SetDone
  Timesheets_TimesheetDetail_ViewBillRate = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ViewBillRate
  Timesheets_TimesheetDetail_UpdateBill = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill
  Timesheets_TimesheetDetail_UpdateTimsheet = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateTimsheet
  Timesheets_TimesheetDetail_RemoveAccount = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_RemoveAccount
  Timesheets_TimesheetDetail_OTType_View = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_OTType_View
  Timesheets_TimesheetDetail_OTType_Edit = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_OTType_Edit
  Timesheets_TimesheetDetail_OTType_Delete = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_OTType_Delete

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<ViewBillComponent>, private userService: UserService,
    private timesheetProjectService: TimesheetProjectService,
    private timesheetProjectBillService: TimeSheetProjectBillService, injector: Injector) {
    super(injector)
  }

  ngOnInit(): void {
    this.billDetail = this.data.billDetail
    this.getProjectOtTypesById(this.data.billInfo.projectId);
  }

  public addTimesheetBillOt(billDetail: TimesheetProjectBill) {
    billDetail.createTimesheetBillOtMode = true;
    this.otTypeProcess = true;
  }

  public cancelCreateTimesheetBillOt(billDetail: TimesheetProjectBill): void {
    billDetail.createTimesheetBillOtMode = false;
    billDetail.otType = null;
    billDetail.otHours = null;
    this.otTypeProcess = false;
  }

  public cancelEditTimesheetBillOt(ot: any) {
    ot.isEditing = false;
    ot.otHours = ot.originalHours;
    ot.otType = ot.originalType;
    this.otTypeProcess = false;
  }

  public saveTimesheetBillOt(billDetail: TimesheetProjectBill): void {
    const ot = this.otTypeOptions.find(ot => ot.otTypeName === billDetail.otType);
    if (!ot) {
      abp.notify.error("Invalid OT type");
      return;
    }

    const existed = billDetail.otTypes?.some(x => x.projectOtTypeId === ot.id);
    if (existed) {
      abp.notify.error("This OT type already exists for this bill");
      return;
    }
    const newTimesheetBillOt = {
      timesheetProjectBillId: billDetail.id,
      projectOtTypeId: ot.id,
      hours: billDetail.otHours,
      mode: 0
    };

    this.timesheetProjectBillService.createOrUpdateTimesheetBillOt(newTimesheetBillOt)
    .pipe(catchError(this.timesheetProjectBillService.handleError))
    .subscribe({
      next: (res: any) => {
        abp.notify.success("OT user added successfully");
        const newOt = {
          id: res.result ,
          projectOtTypeId: ot.id,
          otType: ot.otTypeName,
          otHours: billDetail.otHours,
          isEditing: false
        };

        billDetail.otTypes = billDetail.otTypes || [];
        billDetail.otTypes.push(newOt);
        billDetail.createTimesheetBillOtMode = false;
        billDetail.otType = null;
        billDetail.otHours = null;
        this.otTypeProcess = false;
      },
      error: () => {
        billDetail.createTimesheetBillOtMode = true;
        this.otTypeProcess = false;
      }
    });
  }

  public editTimesheetBillOt(ot: any): void {
    ot.isEditing = true;
    ot.originalHours = ot.otHours;
    ot.originalType = ot.otType;
    ot.originalMultiplier = ot.multiplier;
    this.otTypeProcess = true;
  }

  updateTimesheetBillOt(billDetail: TimesheetProjectBill, ot: any) {
    const otType = this.otTypeOptions.find(opt => opt.otTypeName === ot.otType);
    if (!ot.otType || ot.otHours == null) {
      return;
    }

    const existed = billDetail.otTypes?.some(x => x.projectOtTypeId === otType.id && x.id !== ot.id);
    if (existed) {
      abp.notify.error("This OT type already exists for this bill");
      return;
    }

    const updatePayload = {
      timesheetProjectBillOtTypesId: ot.id,
      timesheetProjectBillId: billDetail.id,
      projectOtTypeId: otType.id,
      hours: ot.otHours,
      mode: 1
    };

    this.otTypeProcess = true;
    this.timesheetProjectBillService.createOrUpdateTimesheetBillOt(updatePayload)
    .subscribe({
      next: (res: any) => {
        abp.notify.success("OT user updated successfully");

        ot.id = res.result;
        ot.projectOtTypeId = otType.id;
        ot.otType = otType.otTypeName;
        ot.otHours = updatePayload.hours;
        ot.isEditing = false;
        this.otTypeProcess = false;
      },
      error: (err) => {
        console.error(err);
        this.otTypeProcess = false;
      }
    });
  }

  getProjectOtTypesById(projectId: any) {
    this.timesheetProjectBillService.getProjectOtTypesById(projectId)
      .subscribe((res: any) => {
        if (res.success) {
          this.otTypeOptions = res.result;
        } else {
          abp.notify.error(res.message || "Failed to fetch OT types");
        }
      });
  }

  isShowOTType(){
    return this.isGranted(this.Timesheets_TimesheetDetail_OTType_View)
  }

  canEditOTType(){
    return this.isGranted(this.Timesheets_TimesheetDetail_OTType_Edit)
  }

  canDeleteOTType(){
    return this.isGranted(this.Timesheets_TimesheetDetail_OTType_Delete)
  }


  public removeTimesheetBillOt(billDetail: TimesheetProjectBill, ot: any) {
    const req = {
      otId: ot.id,
      timesheetProjectBillId: billDetail.id,
    }
    abp.message.confirm(
      "Remove OT user?",
      "",
      (result: boolean) => {
        if (result) {
          this.isLoading = true;
          this.timesheetProjectBillService.removeTimesheetBillOt(req).pipe(catchError(this.timesheetProjectBillService.handleError)).subscribe(data => {
            abp.notify.success(`OT user Removed Successfully!`)
            billDetail.otTypes = billDetail.otTypes?.filter(x => x.id !== ot.id);
          }, () => {
            this.isLoading = false
          })
        }
      }
    )
  }

  onHoursChange(target: any, event: any) {
    let inputValue = event.target.value;
    let value = parseFloat(inputValue);
    if (isNaN(value)) {
      value = null;
    } else {
      if (value < 0) value = 0;
      event.target.value = value;
    }
    if ('Hours' in target) {
      target.Hours = value;
    } else if ('otHours' in target) {
      target.otHours = value;
    }
  }

  public focusOut() {
    this.searchOtType = '';
  }

  public getProjectBill() {
    this.isLoading = true
    this.timesheetProjectBillService.getProjectBill(this.data.billInfo.projectId, this.data.billInfo.timesheetId)
    .subscribe(data => {
      this.billDetail = data.result
      this.isLoading = false
    },
      () => { this.dialogRef.close(); this.isLoading = false })
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

  updateAllWorkingTime(value: number) {
    this.billDetail.forEach(tpb => {
      tpb.workingTime = value;
    });
  }

  public cancelUpdateAll(): void {
    this.getProjectBill();
    this.searchUserBill = "";
    this.otTypeProcess = false;
  }

  public editUserBill(tpb: TimesheetProjectBill): void {
    tpb.isEditing = true;
    this.otTypeProcess = true;
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
        this.otTypeProcess = false;
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
