import { PERMISSIONS_CONSTANT } from './../../constant/permission.constant';
import { ActivatedRoute, Router } from '@angular/router';
import { TimesheetDto } from './../../service/model/timesheet.dto';
import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { DropDownDataDto, InputFilterDto } from '@shared/filter/filter.component';
import { TimesheetService } from '@app/service/api/timesheet.service'
import { catchError, finalize } from 'rxjs/operators';
import { CreateEditTimesheetComponent } from './create-edit-timesheet/create-edit-timesheet.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'app-timesheet',
  templateUrl: './timesheet.component.html',
  styleUrls: ['./timesheet.component.css']
})
export class TimesheetComponent extends PagedListingComponentBase<TimesheetDto> implements OnInit {
  Timesheets_View = PERMISSIONS_CONSTANT.Timesheets_View;
  Timesheets_Create = PERMISSIONS_CONSTANT.Timesheets_Create;
  Timesheets_Edit = PERMISSIONS_CONSTANT.Timesheets_Edit;
  Timesheets_Delete = PERMISSIONS_CONSTANT.Timesheets_Delete;
  Timesheets_ForceDelete = PERMISSIONS_CONSTANT.Timesheets_ForceDelete;
  Timesheets_CloseAndActive = PERMISSIONS_CONSTANT.Timesheets_CloseAndActive;

  public timesheetList: TimesheetDto[] = [];
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', displayName: "Name", comparisions: [0, 6, 7, 8] },
    { propertyName: 'isActive', displayName: "IsActive", comparisions: [0], filterType: 2},
    { propertyName: 'month', displayName: "Month", comparisions: [0, 1, 2, 3, 4] },
    { propertyName: 'year', displayName: "Year", comparisions: [0, 1, 2, 3, 4]},
  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    // throw new Error('Method not implemented.');
    request.sort = this.transDate;
    request.sortDirection = this.sortDrirect;
    this.timesheetService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.timesheetService.handleError)).subscribe(data => {

      this.timesheetList = data.result.items;
      this.showPaging(data.result, pageNumber);

    })
  }
  protected delete(item: TimesheetDto): void {
    abp.message.confirm(
      "Delete TimeSheet " + item.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.timesheetService.delete(item.id).pipe(catchError(this.timesheetService.handleError)).subscribe(() => {
            abp.notify.success("Deleted TimeSheet " + item.name);
            this.refresh()
          });
        }
      }
    );

  }


  constructor(
    private timesheetService: TimesheetService,
    private dialog: MatDialog,
    injector: Injector,
    private route: ActivatedRoute,

  ) {
    super(injector)
  }
  ngOnInit(): void {
    this.refresh()
    this.requestId = this.route.snapshot.queryParamMap.get("id")

  }
  showDialog(command: String, Timesheet: any): void {
    let timesheet = {} as TimesheetDto
    if (command == "edit") {
      timesheet = {
        name: Timesheet.name,
        month: Timesheet.month,
        year: Timesheet.year,
        status: Timesheet.status,
        isActive: Timesheet.isActive,
        totalWorkingDay : Timesheet.totalWorkingDay,
        id: Timesheet.id
      }
    }

    const show = this.dialog.open(CreateEditTimesheetComponent, {
      data: {
        item: timesheet,
        command: command,
      },
      width: "800px",
      disableClose: true,
    });
    show.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });

  }
  createTimeSheet() {
    this.showDialog('create', {})
  }
  editTimesheet(timesheet: TimesheetDto) {
    this.showDialog("edit", timesheet);
  }

  showDetail(item: any) {
    this.router.navigate(['app/timesheetDetail'], {
      queryParams: {
        id: item.id,
        name: item.name,
        createdInvoice: item.createdInvoice,
        isActive: item.isActive
      }
    })

  }

  changeStatus(timesheet) {
    this.timesheetService.ReverseActive(timesheet.id).pipe(catchError(this.timesheetService.handleError)).subscribe(rs => {
      abp.notify.success("Update timesheet: " + timesheet.name)
      if (timesheet.isActive) {
        abp.notify.success("DeActive timesheet: " + timesheet.name)

      }
      else {
        abp.notify.success("Active timesheet: " + timesheet.name)

      }
      this.refresh();
    })
  }

  colorTSFile(item){
    if(item.totalHasFile == item.totalIsRequiredFile)
      return 'text-success';
    return ''
  }
  showColumnAction(){
    if(
      this.permission.isGranted(this.Timesheets_Edit) ||
      this.permission.isGranted(this.Timesheets_Delete) ||
      this.permission.isGranted(this.Timesheets_CloseAndActive)
    )
    {
      return true
    }
    return false
  }
  forceDelete(timesheet){
    abp.message.confirm(
      "Force Delete report: " + timesheet.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.timesheetService.ForceDelete(timesheet.id).pipe(catchError(this.timesheetService.handleError)).subscribe(() => {
            abp.notify.success("Force Deleted project: " + timesheet.name);
            this.refresh()
          });
        }
      }
    );
  }
}
