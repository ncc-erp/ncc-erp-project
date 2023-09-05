import { DatePipe } from '@angular/common';
import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PmReportService } from '@app/service/api/pm-report.service';
import { PmReportInfoDto } from '@app/service/model/pmReport.dto';
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from 'moment';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-report-info',
  templateUrl: './report-info.component.html',
  styleUrls: ['./report-info.component.css'],

})
export class ReportInfoComponent extends AppComponentBase implements OnInit {
  searchWeekResource: string = "";
  searchFutureResource: string = "";
  problemCurrentPage: number = 1;
  weeklyCurrentPage: number = 1;
  futureCurrentPage: number = 1;
  itemPerPage: number = 20;
  tempReportInfo = {} as PmReportInfoDto
  currentDate = new Date();
  reportDate: any = new Date()
  weeklyPercentage: number = 0;
  searchWeeklyCom: number = 2;
  futurePercentage: number = 0;
  searchFutureCom: number = 2;

  public reportInfo = {} as PmReportInfoDto
  constructor(private reportService: PmReportService,
    public dialogRef: MatDialogRef<ReportInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, injector: Injector) {
    super(injector)
  }

  ngOnInit(): void {
    this.getReportInfo();
  }
  getReportInfo() {
    this.reportDate = moment(this.reportDate).format("YYYY-MM-DD")
    this.isLoading = true;
    this.reportService.getStatisticsReport(this.data.report.id, this.reportDate).pipe(catchError(this.reportService.handleError))
      .subscribe(data => {
        this.reportInfo = data.result
        this.tempReportInfo.resourceInTheWeek = data.result.resourceInTheWeek
        this.tempReportInfo.resourceInTheFuture = data.result.resourceInTheFuture
        this.isLoading = false;
      })
  }
  getByDate() {

    this.getReportInfo()
  }

  searchWeeklyUser() {
    this.reportInfo.resourceInTheWeek = this.tempReportInfo.resourceInTheWeek.filter(user => {
      return user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
        || user.fullName.toLowerCase().includes(this.searchWeekResource.toLowerCase())
    })
  }
  searchFutureUser() {
    this.reportInfo.resourceInTheFuture = this.tempReportInfo.resourceInTheFuture.filter(user => {
      return user.email.toLowerCase().includes(this.searchFutureResource.toLowerCase())
        || user.fullName.toLowerCase().includes(this.searchFutureResource.toLowerCase())
    })
  }
  searchWeeklyPercentage() {
    switch (this.searchWeeklyCom) {
      case 0:
        this.reportInfo.resourceInTheWeek = this.tempReportInfo.resourceInTheWeek.filter(user => {
          return user.allocatePercentage > this.weeklyPercentage && user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
            && (user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchWeekResource.toLowerCase()))
        })
        break;
      case 1:
        this.reportInfo.resourceInTheWeek = this.tempReportInfo.resourceInTheWeek.filter(user => {
          return user.allocatePercentage < this.weeklyPercentage
            && (user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchWeekResource.toLowerCase()))
        })
        break;
      case 2:
        this.reportInfo.resourceInTheWeek = this.tempReportInfo.resourceInTheWeek.filter(user => {
          return user.allocatePercentage >= this.weeklyPercentage
            && (user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchWeekResource.toLowerCase()))
        })
        break;
      case 3:
        this.reportInfo.resourceInTheWeek = this.tempReportInfo.resourceInTheWeek.filter(user => {
          return user.allocatePercentage <= this.weeklyPercentage
            && (user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchWeekResource.toLowerCase()))
        })
        break;
      case 4:
        this.reportInfo.resourceInTheWeek = this.tempReportInfo.resourceInTheWeek.filter(user => {
          return user.allocatePercentage == this.weeklyPercentage
            && (user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchWeekResource.toLowerCase()))
        })
        break;

      default:
        break;
    }

  }
  searchFuturePercentage() {
    switch (this.searchFutureCom) {
      case 0:
        this.reportInfo.resourceInTheFuture = this.tempReportInfo.resourceInTheFuture.filter(user => {
          return user.allocatePercentage > this.futurePercentage && user.email.toLowerCase().includes(this.searchWeekResource.toLowerCase())
            && (user.email.toLowerCase().includes(this.searchFutureResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchFutureResource.toLowerCase()))
        })
        break;
      case 1:
        this.reportInfo.resourceInTheFuture = this.tempReportInfo.resourceInTheFuture.filter(user => {
          return user.allocatePercentage < this.futurePercentage
            && (user.email.toLowerCase().includes(this.searchFutureResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchFutureResource.toLowerCase()))
        })
        break;
      case 2:
        this.reportInfo.resourceInTheFuture = this.tempReportInfo.resourceInTheFuture.filter(user => {
          return user.allocatePercentage >= this.futurePercentage
            && (user.email.toLowerCase().includes(this.searchFutureResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchFutureResource.toLowerCase()))
        })
        break;
      case 3:
        this.reportInfo.resourceInTheFuture = this.tempReportInfo.resourceInTheFuture.filter(user => {
          return user.allocatePercentage <= this.futurePercentage
            && (user.email.toLowerCase().includes(this.searchFutureResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchFutureResource.toLowerCase()))
        })
        break;
      case 4:
        this.reportInfo.resourceInTheFuture = this.tempReportInfo.resourceInTheFuture.filter(user => {
          return user.allocatePercentage == this.futurePercentage
            && (user.email.toLowerCase().includes(this.searchFutureResource.toLowerCase())
              || user.fullName.toLowerCase().includes(this.searchFutureResource.toLowerCase()))
        })
        break;

      default:
        break;
    }

  }

}
