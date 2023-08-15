import { Component, Inject, Injector, OnInit, ViewChild ,CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ThemePalette } from '@angular/material/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SelectProjectTailoringComponent } from '@app/modules/audit-management/criteria-management/tailoring-management/tailoring-management/select-project-tailoring/select-project-tailoring/select-project-tailoring.component';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { GetAllPagingProjectProcessCriteriaDto, CreateProjectProcessCriteriaDto } from '@app/service/model/project-process-criteria.dto';
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
    public dialogRef: MatDialogRef<SelectProjectTailoringComponent>,
    private timesheetProjectService: TimesheetProjectService,) {
    super(injector);
  }
  submitDate='';
  startDate: any;
  minDate: Date;
  public selectedProjects = [];
  public projectList= [];
  ngOnInit(): void {
    this.minDate= moment().toDate();
    this.projectList = _.cloneDeep(this.data.listActiveTimesheetProject);
  }

  SaveAndClose() {
    this.selectedProjects = this.projectList.map(x => x.id);

    const input = {
      closeDate: this.submitDate,
      timesheetProjectIds:this.selectedProjects
    }
    this.timesheetProjectService.reAcTiveTimesheet(input).pipe(catchError(this.timesheetProjectService.handleError)).subscribe((res) => {
      abp.notify.success("Reactivate Timesheet Project Successfully");
      this.dialogRef.close();
    }, () => { this.isLoading = false })
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
  removeTimesheetProject(item) {
    const index = this.projectList.indexOf(item.id);
    this.projectList.splice(index, 1);
    if (this.projectList.length <= 0) {
      this.dialogRef.close();
    }
  }
}
