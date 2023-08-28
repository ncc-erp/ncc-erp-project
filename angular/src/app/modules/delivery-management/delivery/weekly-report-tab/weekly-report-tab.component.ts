import { Permission } from './../../../../../shared/service-proxies/service-proxies';
import { projectReportDto, projectProblemDto } from './../../../../service/model/projectReport.dto';
import { result } from 'lodash-es';
import { finalize, catchError } from 'rxjs/operators';
import { PmReportService } from './../../../../service/api/pm-report.service';
import { pmReportDto } from './../../../../service/model/pmReport.dto';
import { Router } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { MatDialog } from '@angular/material/dialog';
import { EditReportComponent } from './edit-report/edit-report.component';
import { ReportInfoComponent } from './report-info/report-info.component';
import { InputFilterDto } from '@shared/filter/filter.component';
import { CollectTimesheetDialogComponent } from './collect-timesheet-dialog/collect-timesheet-dialog.component';
import { AddReportNoteComponent } from './weekly-report-tab-detail/add-report-note/add-report-note.component';

@Component({
  selector: 'app-weekly-report-tab',
  templateUrl: './weekly-report-tab.component.html',
  styleUrls: ['./weekly-report-tab.component.css']
})
export class WeeklyReportTabComponent extends PagedListingComponentBase<WeeklyReportTabComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.isLoading = true
    this.pmReportService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.pmReportService.handleError)).subscribe((data) => {
      this.pmReportList = data.result.items;
      this.showPaging(data.result, pageNumber);
      this.isLoading = false;

    })
  }
  protected delete(entity: WeeklyReportTabComponent): void {
    throw new Error('Method not implemented.');
  }

  public pmReportList: pmReportDto[] = [];
  public closeReportMessage;

  WeeklyReport_CollectTimesheet = PERMISSIONS_CONSTANT.WeeklyReport_CollectTimesheet;
  WeeklyReport_Rename = PERMISSIONS_CONSTANT.WeeklyReport_Rename
  WeeklyReport_View = PERMISSIONS_CONSTANT.WeeklyReport_View
  WeeklyReport_ReportDetail = PERMISSIONS_CONSTANT.WeeklyReport_ReportDetail
  WeeklyReport_ViewInfo = PERMISSIONS_CONSTANT.WeeklyReport_ViewInfo
  WeeklyReport_CloseAndAddNew = PERMISSIONS_CONSTANT.WeeklyReport_CloseAndAddNew

  projectTypeParam = [
    { displayName: 'OUTSOURCING', value: 0 },
    { displayName: 'TRAINING', value: 1 },
    { displayName: 'PRODUCT', value: 2 },
  ];
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Name" },
    { propertyName: 'year', comparisions: [0, 1, 3], displayName: "Year" },
    { propertyName: 'projectType', comparisions: [0], displayName: "Loại dự án", filterType: 3, dropdownData: this.projectTypeParam }
  ];

  constructor(public router: Router,
    private pmReportService: PmReportService, private dialog: MatDialog,
    public injector: Injector) { super(injector) }


  ngOnInit(): void {
    this.refresh();

  }
  showDetail(item: any) {
    if (this.permission.isGranted(this.WeeklyReport_ReportDetail)) {
      this.router.navigate(['app/weeklyReportTabDetail'], {
        queryParams: {
          id: item.id,
          isActive: item.isActive

        }
      })
    }

  }

  closeReport(report: any) {
    let dialogData = {
      name: "",
      isActive: 1,
      year: new Date().getFullYear(),
      type: this.pmReportList[0].type,
      pmReportProjectId: this.pmReportList[0].pmReportProjectId,
      pmReportStatus: this.pmReportList[0].status,
      lastReviewDate: this.pmReportList[0].lastPreviewDate
    }
    const dialogRef = this.dialog.open(EditReportComponent, {
      width: '700px',
      disableClose: true,
      data: {
        command: "CREATE  ",
        dialogData
      }
    })
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refresh()
      }
    });
  }
  editReport(pmReport: any) {
    let dialogData = {} as any
    dialogData = {
      id: pmReport.id,
      name: pmReport.name,
      isActive: pmReport.isActive,
      year: pmReport.year,
      type: pmReport.type,
      pmReportStatus: pmReport.pmReportStatus,
      note: pmReport.note,
      lastPreviewDate: pmReport.lastPreviewDate
    }

    const dialogRef = this.dialog.open(EditReportComponent, {
      width: '700px',
      disableClose: true,
      data: {
        dialogData: dialogData,
      },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refresh()
      }
    });
  }

  viewReportInfo(report) {
    let dialogRef = this.dialog.open(ReportInfoComponent, {
      width: '90vw',
      height: "93vh",
      disableClose: true,
      data: {
        report: report
      }
    })
  }

  collectTimesheet(pmReportId: number) {
    this.dialog.open(CollectTimesheetDialogComponent, {
      width: "700px",
      data: pmReportId
    })
  }
  editNote(reportId) {
    const show = this.dialog.open(AddReportNoteComponent, {
      data: {
        reportId: reportId
      },
      width: "700px",
    })
    show.afterClosed().subscribe(result => {
        this.refresh()
    });
  }
}
