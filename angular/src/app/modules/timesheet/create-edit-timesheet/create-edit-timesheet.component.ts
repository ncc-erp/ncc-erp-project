import { AppComponentBase } from '@shared/app-component-base';
import { Component, Inject, OnInit,Injector } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TimesheetDto } from '@app/service/model/timesheet.dto';
import { TimesheetService } from '@app/service/api/timesheet.service';
import { catchError } from 'rxjs/operators';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import * as moment from 'moment';

// tslint:disable-next-line:no-duplicate-imports






@Component({
  selector: 'app-create-edit-timesheet',
  templateUrl: './create-edit-timesheet.component.html',
  styleUrls: ['./create-edit-timesheet.component.css']
})

export class CreateEditTimesheetComponent extends AppComponentBase implements OnInit {
  public timesheet = {} as TimesheetDto;
  public isDisable = false;
  public listYear: number[] = [];
  public Months =
    [
      1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12
    ]
  private currentYear = new Date().getFullYear()
  private currentMonth = new Date().getMonth()+1;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<CreateEditTimesheetComponent>,
    private timesheetService: TimesheetService,
    private router: Router,
    private projectUserBillService: ProjectUserBillService,
    injector: Injector,){super(injector) }
    startDate: Date | string = '';
    minDate: Date;
    submitDate = '';

  ngOnInit(): void {
    if (this.data.command == "edit") {
      this.timesheet = this.data.item;
      this.startDate = this.timesheet.closeTime ? moment(this.timesheet.closeTime, 'DD-MM-YYYY HH:mm').toDate() : '';
      this.submitDate = moment(this.startDate.toString()).format('LLLL');
    }else{
      this.timesheet.year = this.currentYear;
      this.timesheet.month = this.currentMonth;
    }
    for (let i = this.currentYear - 4; i < this.currentYear + 2; i++) {
      this.listYear.push(i)
    }
    this.minDate = moment().toDate();
  }
  SaveAndClose() {
    this.isDisable = true
    this.timesheet.closeTime = (this.submitDate && this.submitDate !== "Invalid date") ? this.submitDate : '';
    if (this.data.command == "create") {
      this.timesheet.isActive = true;
      this.timesheetService.create(this.timesheet).pipe(catchError(this.timesheetService.handleError)).subscribe((res) => {
        abp.notify.success("Create timesheet "+ this.timesheet.name +" successfully ");
        this.dialogRef.close(this.timesheet);
      }, () => this.isDisable = false);
      //
    }
    else {
      this.timesheetService.update(this.timesheet).pipe(catchError(this.timesheetService.handleError)).subscribe((res) => {
        abp.notify.success("Timesheet "+ this.timesheet.name +" has been edited successfully");
        this.dialogRef.close(this.timesheet);
      }, () => this.isDisable = false);
    }
  }
  reloadComponent() {
    this.router.navigateByUrl('', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/app/timesheet']);
    });
  }
  onDateChange(): void {
    this.submitDate = moment(this.startDate.toString()).format('LLLL');
  }
  removeTime(){
    this.submitDate = ''
    this.startDate = ''
  }


}

