import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppConfigurationService } from '../../../service/api/app-configuration.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

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
  public  timesheetConnectResult: GetConnectResultDto = {} as GetConnectResultDto;
  public  talentConnectResult: GetConnectResultDto = {} as GetConnectResultDto;
  public  hrmConnectResult: GetConnectResultDto = {} as GetConnectResultDto;
  public  finfastConnectResult: GetConnectResultDto = {} as GetConnectResultDto
  public  auditScore: AuditScoreDto = {} as AuditScoreDto;
  public guideLine: GuideLineDto = {} as GuideLineDto ;
  public ChannelId:string=''

 
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


  form : FormGroup

  constructor(private fb: FormBuilder,
    private settingService: AppConfigurationService,
    injector: Injector
  ) {
    super(injector);
    this.form = this.fb.group({
      credentials: this.fb.array([])
    });
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
    // this.getGuideLine();
    if(this.permission.isGranted(this.Admin_Configurations_ViewInformPmSetting)){
    this.getSendTime()
    }
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
  day:string;
}
