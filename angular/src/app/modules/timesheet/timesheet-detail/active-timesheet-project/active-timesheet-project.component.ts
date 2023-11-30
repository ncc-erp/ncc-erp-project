import { Component, Inject, Injector, OnInit} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SelectProjectTailoringComponent } from '@app/modules/audit-management/criteria-management/tailoring-management/tailoring-management/select-project-tailoring/select-project-tailoring/select-project-tailoring.component';
import { AppConfigurationService } from '@app/service/api/app-configuration.service';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { AppComponentBase } from '@shared/app-component-base';
import * as _ from 'lodash';
import * as moment from 'moment';
import { catchError } from 'rxjs/operators';


@Component({
  selector: 'app-active-timesheet-project',
  templateUrl: './active-timesheet-project.component.html',
  styleUrls: ['./active-timesheet-project.component.css']
})
export class ActiveTimesheetProjectComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,

    public injector: Injector,
    private settingService: AppConfigurationService,
    public dialogRef: MatDialogRef<SelectProjectTailoringComponent>,
    private timesheetProjectService: TimesheetProjectService,) {
    super(injector);
  }
  submitDate='';
  startDate: any;
  minDate: Date;
  timer
  timeConfig = ''
  public selectedProjects = [];
  public projectList= [];
  ngOnInit(): void {
    this.minDate= moment().toDate();
    this.projectList = _.cloneDeep(this.data.listTimesheetProject);
    this.settingService.getCloseTimesheetNotification().pipe(catchError(this.settingService.handleError)).subscribe((res:any)=>{
      this.timeConfig = res.result
      let current = new Date().getTime()  + Number(res.result)
      this.startDate = new Date(current)
    })
   this.timer = setInterval(() => {
      this.updateTime();
    },1000);
  }

  SaveAndClose() {
    this.selectedProjects = this.projectList.map(x => x.id);
    const input = {
      closeDate: this.submitDate,
      timesheetProjectIds:this.selectedProjects
    }
    if(this.data.type==='Active'){
      this.timesheetProjectService.reAcTiveTimesheet(input).pipe(catchError(this.timesheetProjectService.handleError)).subscribe((res) => {
        abp.notify.success("Reactivate Timesheet Project Successfully");
        this.dialogRef.close(true);
      }, () => { this.isLoading = false })
    }
    else{
      this.timesheetProjectService.deAcTiveTimesheet(this.selectedProjects).pipe(catchError(this.timesheetProjectService.handleError))
      .subscribe((res) => {
        abp.notify.success("Close timesheet successfully");
        this.dialogRef.close(true);
      }, () => { this.isLoading = false })
      }
    }

  changeTextProjectType(projectType: string) {
    return projectType === 'TAM' ? 'T&M' : projectType
  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
  public onDateChange(): void {
    this.submitDate = moment(this.startDate.toISOString()).format('LLLL');
  }
  public removeTime(){
    this.submitDate = ''
    this.startDate=''
  }
  removeTimesheetProject(item) {
    this.projectList = this.projectList.filter(rs=> {
      return rs.id != item.id
    })
    if (this.projectList.length <= 0) {
      this.dialogRef.close();
    }
  }
  updateTime(): void {
    if(this.startDate  <= new Date()){
      let current = new Date().getTime()  +  Number(this.timeConfig)
      this.startDate = new Date(current)
    }
  }
  ngOnDestroy() {
    clearInterval(this.timer);
  }
}
