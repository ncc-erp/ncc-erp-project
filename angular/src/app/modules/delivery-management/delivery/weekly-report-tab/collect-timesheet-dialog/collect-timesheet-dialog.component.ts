import { ListTimesheetByProjectCodeDialogComponent } from './../list-timesheet-by-project-code-dialog/list-timesheet-by-project-code-dialog.component';
import { PmReportService } from '@app/service/api/pm-report.service';
import { DialogDataDto } from './../../../../../service/model/common-DTO';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';

@Component({
  selector: 'app-collect-timesheet-dialog',
  templateUrl: './collect-timesheet-dialog.component.html',
  styleUrls: ['./collect-timesheet-dialog.component.css'],
  providers: [DatePipe]

})
export class CollectTimesheetDialogComponent implements OnInit {
  isLoading: boolean = false;
  timesheetWorking = {} as TimesheetWorkingDto
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, 
    public dialogRef: MatDialogRef<CollectTimesheetDialogComponent>, private dialog: MatDialog,
    private pmReportService: PmReportService) {
  }

  ngOnInit(): void {
    this.timesheetWorking.pmReportId = this.data
    var d = new Date();
    d.setDate(d.getDate() - (d.getDay() + 6) % 7);
    d.setDate(d.getDate() - 7);
    this.timesheetWorking.timeSheetStartTime = new Date(d.getFullYear(), d.getMonth(), d.getDate());
    this.timesheetWorking.timeSheetEndTime = new Date(d.getFullYear(), d.getMonth(), d.getDate() + 6);

  }
  SaveAndClose() {
    this.isLoading = true
    this.timesheetWorking.timeSheetStartTime = moment(this.timesheetWorking.timeSheetStartTime).format("YYYY-MM-DD")
    if (this.timesheetWorking.timeSheetEndTime) {
      this.timesheetWorking.timeSheetEndTime = moment(this.timesheetWorking.timeSheetEndTime).format("YYYY-MM-DD")
    }
    this.pmReportService.collectTimesheet(this.timesheetWorking.pmReportId, this.timesheetWorking.timeSheetStartTime, this.timesheetWorking.timeSheetEndTime)
      .subscribe(data => {
        this.dialog.open(ListTimesheetByProjectCodeDialogComponent, {
          data: data.result,
          width: "1200px",
          height: "90vh"
        })
        this.dialogRef.close()
        abp.notify.success("Collect timesheet successful")
        this.isLoading = false
      },
        () => {
          this.isLoading = false
        })
  }
}

export class TimesheetWorkingDto {
  pmReportId: number
  timeSheetStartTime: any
  timeSheetEndTime: any
}
