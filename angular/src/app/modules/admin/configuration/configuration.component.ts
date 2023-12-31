import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppConfigurationService } from '../../../service/api/app-configuration.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbTimeStruct, NgbTimepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { MatDialog } from '@angular/material/dialog';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { BillAccountDialogComponent } from './bill-account-dialog/bill-account-dialog.component';

@Component({
  selector: 'app-configuration',
  templateUrl: './configuration.component.html',
  styleUrls: ['./configuration.component.css'],
})
export class ConfigurationComponent extends AppComponentBase implements OnInit {
  Admin_Configuartions_Edit = PERMISSIONS_CONSTANT.Admin_Configuartions_Edit;
  Admin_Configuartions_ViewKomuSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewKomuSetting;
  Admin_Configuartions_ViewProjectSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewProjectSetting;
  Admin_Configuartions_ViewHrmSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewHrmSetting;
  Admin_Configuartions_ViewTimesheetSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewTimesheetSetting;
  Admin_Configuartions_ViewFinanceSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewFinanceSetting;
  Admin_Configuartions_ViewTalentSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewTalentSetting;
  Admin_Configuartions_ViewGoogleClientAppSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewGoogleClientAppSetting;
  Admin_Configuartions_ViewDefaultWorkingHourPerDaySetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewDefaultWorkingHourPerDaySetting;
  Admin_Configuartions_ViewMaxCountHistoryOfRetroAndReviewPoint = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewMaxCountHistoryOfRetroAndReviewPoint;
  Admin_Configuartions_ViewAuditScoreSetting = PERMISSIONS_CONSTANT.Admin_Configuartions_ViewAuditScoreSetting;
  Admin_Configurations_ViewGuideLineSetting = PERMISSIONS_CONSTANT.Admin_Configurations_ViewGuideLineSetting;
  Admin_Configurations_ViewInformPmSetting = PERMISSIONS_CONSTANT.Admin_Configurations_ViewInformPmSetting;
  WeeklyReport_ReportDetail_GuideLine_View = PERMISSIONS_CONSTANT.WeeklyReport_ReportDetail_GuideLine_View

  configuration = {} as ConfigurationDto;
  googleToken: string = '';
  public isEditClientId: boolean = false;
  public isEditProjectUri: boolean = false;
  public isEditSecretCode: boolean = false;
  public isEditUserBot: boolean = false;
  public isEditPassBot: boolean = false;
  public isEditKomuUrl: boolean = false;
  public isEditDefaultWorkingHours: boolean = false;
  public isEditMaxCountHistory: boolean = false;
  public isEditAuditScore: boolean = false;
  public isEditSendTime: boolean = false;
  public isEditActiveTimesheetProjectPeriod = false;
  public isShowKomuSetting: boolean = false;
  public isExpandingProjectSetting: boolean = false;
  public isShowHRMSetting: boolean = false;
  public isShowTimesheetSetting: boolean = false;
  public isShowFinanceSetting: boolean = false;
  public isShowTalentSetting: boolean = false;
  public isShowGoogleClientApp: boolean = false;
  public isShowDefaultWorking: boolean = false;
  public isShowMaxCountHistory: boolean = false;
  public isShowDiscordChannel: boolean = false;
  public isEditDiscordChannel: boolean = false;
  public isShowAuditScore: boolean = false;
  public isShowGuideLineSetting: boolean = false;
  public isShowActiveTimesheetProjectPeriod = false;
  public isEditSendHour = false;
  public  timesheetConnectResult: GetConnectResultDto = {} as GetConnectResultDto;
  public  talentConnectResult: GetConnectResultDto = {} as GetConnectResultDto;
  public  hrmConnectResult: GetConnectResultDto = {} as GetConnectResultDto;
  public  finfastConnectResult: GetConnectResultDto = {} as GetConnectResultDto
  public  auditScore: AuditScoreDto = {} as AuditScoreDto;
  public guideLine: GuideLineDto = {} as GuideLineDto ;
  public CloseTimesheetChannelId: string = ''
  public ChannelId:string=''
  // auto and noti charge status
  public isShowAutoUpdateNotifyBillAcc = false;
  public isEditAutoUpdateNotifyBillAcc = false;
  public notiChargeChannelId: string = '';
  public billAccountIds: any[] = [];
  public showSpinner: boolean = false;
  public autoUpdateChargeStatus : TimeSendDto = new TimeSendDto(false, '00:00', 6);
  public time : NgbTimeStruct = {hour: 10, minute: 10, second: 0};
  public listDateofMonth: number[] = Array.from({ length: 31 }, (_, index) => index + 1);
  public ischargeStatusFormValid = false;


  public listDays: any[] = [
    { value: 1, text: 'Monday' },
    { value: 2, text: 'Tuesday' },
    { value: 3, text: 'Wednesday' },
    { value: 4, text: 'Thursday' },
    { value: 5, text: 'Friday' },
    { value: 6, text: 'Saturday' },
    { value: 0, text: 'Sunday' },
  ];
  public listHours = [];
  public isEditingKomu: boolean = false;
  public isEditingTimesheet: boolean = false;
  public isEditingGuideLine: boolean = false;

  form : FormGroup;
  chargeStatusForm : FormGroup;
  closeTimesheetFrom : FormGroup

  constructor(private fb: FormBuilder,
    private settingService: AppConfigurationService,
    private dialog: MatDialog,
    injector: Injector, config : NgbTimepickerConfig
  ) {
    super(injector);
    this.form = this.fb.group({
      credentials: this.fb.array([])
    });
    this.closeTimesheetFrom = this.fb.group({
      closeTimesheetCredentials: this.fb.array([])
    });
    this.chargeStatusForm = this.fb.group({
      chargeStatusCredentials: this.fb.array([])
    });
    config.seconds = false;
    config.spinners = false;
  }

  ngOnInit(): void {
    for (let index = 0; index < 24; index++) {
      let hour = { value: index.toString(), text: index.toString() + 'h' };
      this.listHours.push(hour);
    }
    this.getSetting();
    this.checkConnectToTimesheet();
    this.checkConnectToFinance();
    this.checkConnectToTalent();
    this.getAuditScore();
    this.getCloseTimesheetTime();
    // this.getGuideLine();
    if(this.permission.isGranted(this.Admin_Configurations_ViewInformPmSetting)){
    this.getSendTime()
    }
    this.getChargeStatusConfig();
  }
  getSetting() {
    this.settingService.getConfiguration().subscribe((data) => {
      this.configuration = data.result;
    });
  }

  checkConnectToFinance(){
    this.finfastConnectResult = {} as GetConnectResultDto;
    this.settingService.checkConnectToFinfast().subscribe((data) => {
      this.finfastConnectResult = data.result;
    })
  }

  checkConnectToTalent(){
    this.talentConnectResult = {} as GetConnectResultDto;
    this.settingService.checkConnectToTalent().subscribe((data) => {
      this.talentConnectResult = data.result;
    })
  }

  checkConnectToTimesheet(){
    this.timesheetConnectResult = {} as GetConnectResultDto;
    this.settingService.checkConnectToTimesheet().subscribe((data) => {
      this.timesheetConnectResult = data.result;
    })
  }

  saveConfiguration() {
    this.settingService
      .editConfiguration(this.configuration)
      .subscribe((data) => {
        abp.notify.success('Edited');
      });
  }
  enableNoticeToKomu(value){
    this.configuration.noticeToKomu = value.toString();
  }
  enableAutoUpdateProjectInfoToTimesheetTool(value){
    this.configuration.autoUpdateProjectInfoToTimesheetTool = value.toString();
  }
  checkNoticeToKomu() {
    if (this.configuration.noticeToKomu == 'true') return true;
    return false;
  }

  public updateProjectSettingConfig(){
    let projectConfig = {} as ProjectSetting;
    projectConfig.securityCode = this.configuration.securityCode;
    this.settingService
    .updateProjectSettingConfig(projectConfig)
    .subscribe((rs)=>{
      abp.notify.success('Update project setting successful!');

    })
  }

  // getGuideLine(){
  //   if(this.permission.isGranted(this.WeeklyReport_ReportDetail_GuideLine_View)){
  //     this.settingService.getGuideLine().subscribe((data) => {
  //       this.guideLine = data.result;
  //     });
  //   }
  // }

  // saveGuideLine(){
  //   this.settingService
  //     .editGuideLine(this.guideLine)
  //     .subscribe((data) => {
  //       abp.notify.success('Edited successfully!');
  //     });
  // }

  getAuditScore(){
    this.settingService.getAuditScore().subscribe((data) => {
      this.auditScore = data.result;
    });
  }
  saveAuditScore(){
    this.settingService
      .editAuditScore(this.auditScore)
      .subscribe((data) => {
        abp.notify.success('Edited successfully!');
      });
  }

  addTime(){
    this.credentials.push( this.fb.group({
      IsCheck: true,
      Time: ['',Validators.required],
      Day: 2
    }))
  }

  addCloseTimesheetTime(){
    this.closeTimesheetCredentials.push( this.fb.group({
      IsCheck: true,
      Hours: [],
      Minutes: [ ,Validators.required],
      Day: 0
    }))
  }

  getSendTime(){
      this.settingService.getTimeSend().subscribe((data:any) => {
        this.ChannelId = data.result.channelId;
        this.credentials.clear()
        data.result.checkDateTimes.forEach(item =>{
          const cred = this.fb.group({
            IsCheck: item.isCheck,
            Time: [item.time,Validators.required],
            Day: item.day
          })
         this.credentials.push(cred)
        })
      });
  }
  removeItem(i){
  this.credentials.removeAt(i)

  }
  saveSendTime(){
    this.settingService
      .setTimeSend({ChannelId: this.ChannelId,CheckDateTimes:this.form.value.credentials})
      .subscribe((data) => {
        this.isEditSendTime = !this.isEditSendTime
        abp.notify.success('Edited successfully!');
      });
  }
  get credentials(){
   return this.form.controls.credentials as FormArray;
  }

  checkDisabledSave(){
    if(( this.credentials.value.some(item=>{
      return item.IsCheck == true
    }) && !this.ChannelId) || this.form.invalid){
      return true
    }
    else{
      return false
    }
  }
  checkRequiredChannelId(){
    if( this.credentials.value.some(item=>{
      return item.IsCheck == true
    }) && !this.ChannelId && this.isEditSendTime){
      return  true
    }
    else{
      return false
    }
  }

  getCloseTimesheetTime(){
    this.settingService.getCloseTimesheetNotification().subscribe((data:any) => {
      this.CloseTimesheetChannelId = data.result.channelId;
      this.closeTimesheetCredentials.clear()
      data.result.checkDateTimes.forEach(item =>{
        const cred = this.fb.group({
          IsCheck: item.isCheck,
          Hours: [item.time.split(':')[0]],
          Minutes: [item.time.split(':')[1]],
          Day: item.day
        })
       this.closeTimesheetCredentials.push(cred)
      })
    });
  }

  get closeTimesheetCredentials(){
    return this.closeTimesheetFrom.controls.closeTimesheetCredentials as FormArray;
   }

  saveCloseTimesheetTime(){
      // Check any null or invalid values in the array
  if (this.closeTimesheetFrom.value.closeTimesheetCredentials.some((credential) => {
    return !credential || (!credential.Hours || credential.Hours == 0) && (!credential.Minutes || credential.Minutes == 0);
  })) {
    abp.notify.error('Edited close time fail!');
    return;
  }
    const modifiedCloseTimesheetCredentials = this.closeTimesheetFrom.value.closeTimesheetCredentials.map((credential) => {
      const mergedTime = (credential.Hours ?? 0).toString() + ":" + (credential.Minutes ?? 0).toString();
      return {
        IsCheck: credential.IsCheck,
        Time: mergedTime, // The merged time property
        Day: credential.Day
      };
    });
    this.settingService
      .setCloseTimesheetNotification({ChannelId: this.ChannelId,CheckDateTimes:modifiedCloseTimesheetCredentials})
      .subscribe((data) => {
        this.isEditSendHour = !this.isEditSendHour
        abp.notify.success('Edited timesheet close time successfully!');
      });
  }

  checkDisabledTSaveTimesheetClose(){
    if(( this.closeTimesheetCredentials.value.some(item=>{
      return item.IsCheck == true
    }) && !this.CloseTimesheetChannelId)){
      return true
    }
    else{
      return false
    }
  }

  get chargeStatusCredentials(){
    return this.chargeStatusForm.controls.chargeStatusCredentials as FormArray;
   }

  getChargeStatusConfig() {
    this.settingService.getChargeStatusConfig().subscribe((data:any) => {
      this.notiChargeChannelId = data.result.notiUsers ? data.result.notiUsers.channelId : '';
      this.billAccountIds = data.result.userIds;
      this.autoUpdateChargeStatus = data.result.autoUpdateBillAccount ?? new TimeSendDto(false, '00:00', 6);
      this.chargeStatusCredentials.clear()
      if(data.result.notiUsers){
        data.result.notiUsers.checkDateTimes.forEach(item =>{
        const cred = this.fb.group({
          IsCheck: item.isCheck,
          Time: item.time,
          Day: item.day
        })
       this.chargeStatusCredentials.push(cred)
      })
    }
    });
  }

  addChargeStatusNotiTime(){
    this.chargeStatusCredentials.push( this.fb.group({
      IsCheck: true,
      Time: ['00:00',Validators.required],
      Day: 6
    }))
  }

  addBillAccount(){
    const show = this.dialog.open(BillAccountDialogComponent, {
      data: {
        selectedIds: this.billAccountIds ? this.billAccountIds: [],
      },
      width: "700px",
    })
    show.afterClosed().subscribe((res) => {
      if (res?.isSave) {
        this.billAccountIds = res.updateBill;
      }
    })
  }

  checkDisabledTSaveUpdateNotiCharge(){
    if(( this.chargeStatusCredentials.value.some(item=>{
      return item.IsCheck == true
    }) && !this.notiChargeChannelId) || this.chargeStatusForm.invalid){
      return true
    }
    else{
      return false
    }
  }

  removeItemChargeStatusNoti(i){
    this.chargeStatusCredentials.removeAt(i)
  }

  saveAutoUpdateNotiChargeStatus(){
    this.IschargeStatusFormValid();
    if(this.ischargeStatusFormValid){
    this.settingService
      .setChargeStatusConfig({AutoUpdateBillAccount: this.autoUpdateChargeStatus,
        UserIds : this.billAccountIds,
        NotiUsers:{ChannelId: this.notiChargeChannelId, CheckDateTimes:this.chargeStatusForm.value.chargeStatusCredentials }
        })
      .subscribe((data) => {
        this.isEditAutoUpdateNotifyBillAcc = !this.isEditAutoUpdateNotifyBillAcc;
        abp.notify.success('Edited successfully!');
        this.ischargeStatusFormValid = false;
      });
    }
  }

  IschargeStatusFormValid() {
  const controls = (this.chargeStatusForm.get('chargeStatusCredentials') as FormArray).controls;

  const valuesArray = controls.map(control => {
    return {
      IsCheck: control.get('IsCheck').value,
      Time: control.get('Time').value,
      Day: control.get('Day').value
    };
  });

  // Check for duplicates based on IsCheck, Time, and Day
  const duplicates = valuesArray.some((value, index) => {
    return (
      index !== valuesArray.findIndex(
        item =>
          item.IsCheck === value.IsCheck &&
          item.Time === value.Time &&
          item.Day === value.Day
      )
    );
  });
  if(duplicates){
    abp.notify.error("Notification times are duplicated");
    return;
  }
  this.ischargeStatusFormValid = true;
  return;
  }


  removeItemCloseTimesheet(i){
    this.closeTimesheetCredentials.removeAt(i)
    }
}
export class ConfigurationDto {
  clientAppId: string;
  projectUri: string;
  securityCode: string;
  userBot: string;
  passwordBot: string;
  komuUrl: string;
  komuUserNames: string;
  komuSecretCode: string;
  financeUri: string;
  financeSecretCode: string;
  timesheetUri: string;
  timesheetSecretCode: string;
  canSendDay: string;
  canSendHour: string;
  expiredDay: string;
  expiredHour: string;
  hrmUri: string;
  hrmSecretCode: string;
  defaultWorkingHours: string;
  noticeToKomu: string;
  autoUpdateProjectInfoToTimesheetTool: string;
  trainingRequestChannel: string;
  talentUriBA: string;
  talentUriFE: string;
  talentSecurityCode: string;
  maxCountHistory: number;
  activeTimesheetProjectPeriod:number
}

export class ProjectSetting{
  securityCode: string;
}

export class GetConnectResultDto {
  isConnected: boolean;
  message: string;
}

export class AuditScoreDto{
  giveN_SCORE: number;
  projecT_PROCESS_CRITERIA_RESULT_STATUS_EX: number
  projecT_PROCESS_CRITERIA_RESULT_STATUS_NC: number
  projecT_PROCESS_CRITERIA_RESULT_STATUS_OB: number
  projecT_PROCESS_CRITERIA_RESULT_STATUS_RE: number
  projecT_SCORE_WHEN_STATUS_AMBER: number
  projecT_SCORE_WHEN_STATUS_GREEN: number
  projecT_SCORE_WHEN_STATUS_RED: number
}

export class GuideLineDto{
  issue: string;
  risk: string;
  pmNote: string;
  criteriaStatus: string;
}

export class TimeSendDto {
  isCheck : boolean;
  time: string;
  day:number;

  constructor(isCheck : boolean, time: string,day:number){
    this.isCheck = isCheck;
    this.time = time;
    this.day = day;
  }
}
