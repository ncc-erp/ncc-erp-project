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

interface ExtendedTimesheetProjectBill extends TimesheetProjectBill {
  originalOtTypes?: any[];
  pendingOtChanges?: {
    added: any[];
    updated: any[];
    removed: number[];
  };
}

@Component({
  selector: 'app-view-bill',
  templateUrl: './view-bill.component.html',
  styleUrls: ['./view-bill.component.css']
})

export class ViewBillComponent extends AppComponentBase implements OnInit {
  billDetail: ExtendedTimesheetProjectBill[] = []
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
    // Backup original OT data for each bill
    this.billDetail.forEach(bill => {
      bill.originalOtTypes = bill.otTypes ? JSON.parse(JSON.stringify(bill.otTypes)) : [];
      bill.pendingOtChanges = {
        added: [],
        updated: [],
        removed: []
      };
    });
    this.getProjectOtTypesById(this.data.billInfo.projectId);
  }

  // ADD OT - Only UI change
  public addTimesheetBillOt(billDetail: ExtendedTimesheetProjectBill) {
    billDetail.createTimesheetBillOtMode = true;
    this.otTypeProcess = true;
  }

  public cancelCreateTimesheetBillOt(billDetail: ExtendedTimesheetProjectBill): void {
    billDetail.createTimesheetBillOtMode = false;
    billDetail.otType = null;
    billDetail.otHours = null;
    this.otTypeProcess = false;
  }

  // EDIT OT - Only UI change
  public editTimesheetBillOt(ot: any): void {
    ot.isEditing = true;
    ot.originalHours = ot.otHours;
    ot.originalType = ot.otType;
    ot.originalMultiplier = ot.multiplier;
    this.otTypeProcess = true;
  }

  public cancelEditTimesheetBillOt(ot: any) {
    ot.isEditing = false;
    ot.otHours = ot.originalHours;
    ot.otType = ot.originalType;
    this.otTypeProcess = false;
  }

  private findOtTypeByName(otTypeName: string) {
    return this.otTypeOptions.find(ot => ot.otTypeName === otTypeName);
  }

  private isOtTypeExisted(billDetail: ExtendedTimesheetProjectBill, projectOtTypeId: any): boolean {
    return billDetail.otTypes?.some(x => x.projectOtTypeId === projectOtTypeId) || false;
  }

  // SAVE OT - Only update UI, mark as pending
  public saveTimesheetBillOt(billDetail: ExtendedTimesheetProjectBill): void {
    const ot = this.findOtTypeByName(billDetail.otType);
    if (!ot) {
      abp.notify.error("Invalid OT type");
      return;
    }

    if (this.isOtTypeExisted(billDetail, ot.id)) {
      abp.notify.error("This OT type already exists for this bill");
      return;
    }

    const newOt = {
      id: null, 
      projectOtTypeId: ot.id,
      otType: billDetail.otType,
      otHours: billDetail.otHours,
      multiplier: ot.multiplier,
      isNew: true,
      isEditing: false
    };

    if (!billDetail.otTypes) {
      billDetail.otTypes = [];
    }
    billDetail.otTypes.push(newOt);

    billDetail.pendingOtChanges.added.push(newOt);

    // Reset form
    billDetail.createTimesheetBillOtMode = false;
    billDetail.otType = null;
    billDetail.otHours = null;
    this.otTypeProcess = false;
  }

  // UPDATE OT - Only update UI, mark as pending
  updateTimesheetBillOt(billDetail: ExtendedTimesheetProjectBill, ot: any) {
    const otType = this.otTypeOptions.find(opt => opt.otTypeName === ot.otType);
    if (!ot.otType || ot.otHours == null) {
      return;
    }

    const existed = billDetail.otTypes?.some(x => x.projectOtTypeId === otType.id && x.id !== ot.id);
    if (existed) {
      abp.notify.error("This OT type already exists for this bill");
      return;
    }

    // Update OT data in UI
    ot.projectOtTypeId = otType.id;
    ot.multiplier = otType.multiplier;
    ot.isEditing = false;

    // Track pending change (only if it's not a new entry)
    if (!ot.isNew) {
      const existingUpdate = billDetail.pendingOtChanges.updated.find(u => u.id === ot.id);
      if (existingUpdate) {
        // Update existing pending change
        existingUpdate.otType = ot.otType;
        existingUpdate.otHours = ot.otHours;
        existingUpdate.projectOtTypeId = otType.id;
      } else {
        // Add new pending change
        billDetail.pendingOtChanges.updated.push({
          id: ot.id,
          otType: ot.otType,
          otHours: ot.otHours,
          projectOtTypeId: otType.id
        });
      }
    }

    this.otTypeProcess = false;
  }

  // REMOVE OT - Only update UI, mark as pending
  public removeTimesheetBillOt(billDetail: ExtendedTimesheetProjectBill, ot: any) {
    abp.message.confirm(
      "Remove OT user?",
      "",
      (result: boolean) => {
        if (result) {
          // Find index and remove from UI
          const index = billDetail.otTypes.findIndex(o => o === ot);
          if (index !== -1) {
            billDetail.otTypes.splice(index, 1);
          }

          // Track pending change
          if (ot.isNew) {
            // Remove from pending added list
            const addedIndex = billDetail.pendingOtChanges.added.findIndex(a => a === ot);
            if (addedIndex !== -1) {
              billDetail.pendingOtChanges.added.splice(addedIndex, 1);
            }
          } else {
            // Add to pending removed list
            billDetail.pendingOtChanges.removed.push(ot.id);
          }
        }
      }
    );
  }

  // ACTUAL API CALLS - Called when Save button is clicked
  private async savePendingOtChanges(billDetail: ExtendedTimesheetProjectBill): Promise<void> {
    const changes = billDetail.pendingOtChanges;

    // Process removals
    for (const otId of changes.removed) {
      const req = {
        otId: otId,
        timesheetProjectBillId: billDetail.id,
      };
      await this.timesheetProjectBillService.removeTimesheetBillOt(req)
        .pipe(catchError(this.timesheetProjectBillService.handleError))
        .toPromise();
    }

    // Process additions
    for (const newOt of changes.added) {
      const payload = {
        timesheetProjectBillId: billDetail.id,
        projectOtTypeId: newOt.projectOtTypeId,
        hours: newOt.otHours,
        mode: 0
      };
      await this.timesheetProjectBillService.createOrUpdateTimesheetBillOt(payload)
        .pipe(catchError(this.timesheetProjectBillService.handleError))
        .toPromise();
    }

    // Process updates
    for (const update of changes.updated) {
      const payload = {
        timesheetProjectBillOtTypesId: update.id,
        timesheetProjectBillId: billDetail.id,
        projectOtTypeId: update.projectOtTypeId,
        hours: update.otHours,
        mode: 1
      };
      await this.timesheetProjectBillService.createOrUpdateTimesheetBillOt(payload)
        .pipe(catchError(this.timesheetProjectBillService.handleError))
        .toPromise();
    }

    // Reset pending changes
    billDetail.pendingOtChanges = {
      added: [],
      updated: [],
      removed: []
    };
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

  readonly HOURS_PER_DAY = 8;

  normalUnit: 'Day' | 'Hour' = 'Day';
  otUnit: 'Day' | 'Hour' = 'Hour';

  private daysToHours(days: number): number {
    return parseFloat((days * this.HOURS_PER_DAY).toFixed(2));
  }

  private hoursToDays(hours: number): number {
    return parseFloat((hours / this.HOURS_PER_DAY).toFixed(2));
  }

  getDisplayWorkingTime(value: number): number {
    if (!value) return 0;
    if (this.normalUnit === 'Hour') {
      return parseFloat(this.daysToHours(value).toFixed(2));
    }
    return parseFloat(value.toFixed(2));
  }

  getDisplayOTTime(value: number): number {
    if (!value) return 0;
    if (this.otUnit === 'Day') {
      return parseFloat(this.hoursToDays(value).toFixed(2));
    }
    return parseFloat(value.toFixed(2));
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
      // Backup original OT data after loading
      this.billDetail.forEach(bill => {
        bill.originalOtTypes = bill.otTypes ? JSON.parse(JSON.stringify(bill.otTypes)) : [];
        bill.pendingOtChanges = {
          added: [],
          updated: [],
          removed: []
        };
      });
      this.isLoading = false
    },
      () => { this.dialogRef.close(); this.isLoading = false })
  }

  public async saveUserBill(tpb: ExtendedTimesheetProjectBill): Promise<void> {
    delete tpb["isEditing"];
    
    tpb.startTime = moment(tpb.startTime).format("YYYY-MM-DD");
    if (tpb.endTime) {
      tpb.endTime = moment(tpb.endTime).format("YYYY-MM-DD");
    }
    tpb.timesheetId = this.data.billInfo.timesheetId;
    tpb.projectId = this.data.billInfo.projectId;

    // Save pending OT changes first
    if (tpb.pendingOtChanges) {
      await this.savePendingOtChanges(tpb);
    }

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

  async saveAllUpdateBill() {
    // Save all pending OT changes for all bills
    for (const bill of this.billDetail) {
      if (bill.pendingOtChanges) {
        await this.savePendingOtChanges(bill);
      }
    }

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
    // Restore original OT data
    this.billDetail.forEach(bill => {
      if (bill.originalOtTypes) {
        bill.otTypes = JSON.parse(JSON.stringify(bill.originalOtTypes));
        bill.pendingOtChanges = {
          added: [],
          updated: [],
          removed: []
        };
      }
      // Reset create mode
      bill.createTimesheetBillOtMode = false;
      bill.otType = null;
      bill.otHours = null;
      
      // Reset editing state for all OT entries
      bill.otTypes?.forEach(ot => {
        ot.isEditing = false;
      });
    });
    
    this.getProjectBill();
    this.searchUserBill = "";
    this.otTypeProcess = false;
  }

  public editUserBill(tpb: ExtendedTimesheetProjectBill): void {
    tpb.isEditing = true;
    // Không set otTypeProcess = true nữa, để user có thể thao tác với OT
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
    let bill = {} as ExtendedTimesheetProjectBill;
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

  async saveUpdateTS(data){
    // Save pending OT changes first
    if (data.pendingOtChanges) {
      await this.savePendingOtChanges(data);
    }

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

  protected removeAccountTS(tpb: ExtendedTimesheetProjectBill): void {
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

  async saveAllUpdateTS(){
    // Save all pending OT changes for all bills
    for (const bill of this.billDetail) {
      if (bill.pendingOtChanges) {
        await this.savePendingOtChanges(bill);
      }
    }

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