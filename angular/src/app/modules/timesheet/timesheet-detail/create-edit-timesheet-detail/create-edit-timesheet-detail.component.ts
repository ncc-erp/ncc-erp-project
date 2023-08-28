import { ProjectTimesheetDto, TimesheetDetailDto } from './../../../../service/model/timesheet.dto';
import { filter } from 'rxjs/operators';
import { ListProjectService } from '@app/service/api/list-project.service';
import { ProjectDto } from './../../../../service/model/list-project.dto';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import * as _ from 'lodash';
@Component({
  selector: 'app-create-edit-timesheet-detail',
  templateUrl: './create-edit-timesheet-detail.component.html',
  styleUrls: ['./create-edit-timesheet-detail.component.css']
})
export class CreateEditTimesheetDetailComponent implements OnInit {
  public searchProject:string =""
  public isDisable = false;
  public timesheetDetail = {} as TimesheetDetailDto;
  public project = {} as ProjectDto;
  public projectList: any;
  public projectTimesheet = {} as ProjectTimesheetDto;
  selectedFiles: FileList;
  currentFileUpload: File;
  projectTimesheetId: any;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<CreateEditTimesheetDetailComponent>,
    private timesheetProjectService: TimesheetProjectService,
    private projectService: ListProjectService,
    private route: ActivatedRoute,) { }

  ngOnInit(): void {
    this.getAllProject();
    this.projectTimesheetId = this.data.projectTimesheetDetailId;
    if (this.data.command == 'edit') {
      this.projectTimesheet = this.data.item;
    }
    this.projectTimesheet.timesheetId = Number(this.route.snapshot.queryParamMap.get('id'));
  }

  SaveAndClose() {
    this.isDisable = true;
    if (this.data.command == "create") {
      this.timesheetProjectService.create(this.projectTimesheet).pipe(catchError(this.projectService.handleError)).subscribe((res) => {
        abp.notify.success("Created timesheet detail successfully");
        this.dialogRef.close(this.projectTimesheet)
      }, () => this.isDisable = false);
    }
    else {
      let request = {
        Id: this.projectTimesheet.id,
        Note: this.projectTimesheet.note
      }
      this.timesheetProjectService
      .updateNote(request)
      .subscribe((res) => {
        if(res.success){
          abp.notify.success(res.result)
        }
        else{
          abp.notify.error(res.result)
        }
        this.dialogRef.close(this.projectTimesheet)
      }, () => this.isDisable = false)
      // this.timesheetProjectService.update(this.projectTimesheet).pipe(catchError(this.projectService.handleError)).subscribe((res) => {
      //   abp.notify.success("Edited timesheet detail successfully");
      //   this.dialogRef.close(this.projectTimesheet)

      // }, () => this.isDisable = false);
    }

  }

  getAllProject() {
    this.projectService.getAll().subscribe(data => {
      this.projectList = data.result.filter(item => !this.projectTimesheetId.includes(item.id));
    })
  }
}
