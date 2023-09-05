import { PERMISSIONS_CONSTANT } from './../../../../../constant/permission.constant';
import { AppSessionService } from './../../../../../../shared/session/app-session.service';
import { TimesheetService } from './../../../../../service/api/timesheet.service';
import { ProjectTimesheetDto } from './../../../../../service/model/timesheet.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { TimesheetProjectService } from './../../../../../service/api/timesheet-project.service';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { ImportFileTimesheetDetailComponent } from './../../../../timesheet/timesheet-detail/import-file-timesheet-detail/import-file-timesheet-detail.component';
import { Component, OnInit, Injector } from '@angular/core';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-product-project-timesheet',
  templateUrl: './product-project-timesheet.component.html',
  styleUrls: ['./product-project-timesheet.component.css']
})
export class ProductProjectTimesheetComponent extends AppComponentBase implements OnInit {
 
  public listTimesheetByProject: ProjectTimesheetDto[] = [];
  private projectId: number;
  constructor(injector: Injector,public sessionService:AppSessionService,
    public timesheetProjectService: TimesheetProjectService,
    private timesheetService: TimesheetService,
    private timesheetSerivce: TimesheetProjectService, private route: ActivatedRoute, private dialog: MatDialog) {
    super(injector);
    this.projectId = Number(route.snapshot.queryParamMap.get("id"));
  }

  ngOnInit(): void {
    this.getAllTimesheet();
  }
  private getAllTimesheet() {
    this.timesheetSerivce.getAllByProject(this.projectId).pipe(catchError(this.timesheetSerivce.handleError)).subscribe(data => {
      this.listTimesheetByProject = data.result;
    })
  }
  importExcel(id: any) {
    const dialogRef = this.dialog.open(ImportFileTimesheetDetailComponent, {
      data: { id: id, width: '500px' }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.reloadTimesheetFile(result);
      }
      
    });
  }
  reloadTimesheetFile(id) {
    this.timesheetSerivce.getAllByProject(this.projectId).pipe(catchError(this.timesheetSerivce.handleError)).subscribe((data) => {
      this.listTimesheetByProject =data.result;
        if (!this.listTimesheetByProject.filter(timesheet => timesheet.id == id)[0].timesheetFile) {
          setTimeout(() => {
            this.reloadTimesheetFile(id)
          }, 1000)
        }
      })
  }
  importFile(id: number) {
    this.timesheetProjectService.DownloadFileTimesheetProject(id).subscribe(data => {
    })
  }
  DeleteFile(item: any) {
    abp.message.confirm(
      "Delete File " + item.timesheetFile + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.timesheetProjectService.UpdateFileTimeSheetProject(null, item.id).pipe(catchError(this.timesheetService.handleError)).subscribe(() => {
            abp.notify.success("Deleted File  " + item.timesheetFile);
            this.getAllTimesheet();
          });
        }
      }
    );

  }

  downloadFile(projectTimesheet: any) {
    this.timesheetProjectService.GetTimesheetFile(projectTimesheet.id).subscribe(data => {
      const file = new Blob([this.s2ab(atob(data.result.data))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, data.result.fileName);
    })

  }
  s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }
}

