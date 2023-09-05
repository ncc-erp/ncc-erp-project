import { result } from 'lodash-es';
import { TimesheetService } from '@app/service/api/timesheet.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ImportFileTimesheetDetailComponent } from './../../../../timesheet/timesheet-detail/import-file-timesheet-detail/import-file-timesheet-detail.component';
import { MatDialog } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { ProjectTimesheetDto } from './../../../../../service/model/timesheet.dto';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, inject } from '@angular/core';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-project-timesheet',
  templateUrl: './project-timesheet.component.html',
  styleUrls: ['./project-timesheet.component.css']
})
export class ProjectTimesheetComponent extends AppComponentBase implements OnInit {
  
  // Timesheet_TimesheetProject = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject;
  // Timesheet_TimesheetProject_Create= PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_Create;
  // Timesheet_TimesheetProject_CreateInvoice = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_CreateInvoice;
  // Timesheet_TimesheetProject_Delete = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_Delete;
  // Timesheet_TimesheetProject_DownloadFileTimesheetProject =PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_DownloadFileTimesheetProject;
  // Timesheet_TimesheetProject_GetAllByProject = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_GetAllByProject;
  // Timesheet_TimesheetProject_GetAllRemainProjectInTimesheet = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_GetAllRemainProjectInTimesheet;
  // Timesheet_TimesheetProject_Update = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_Update;
  // Timesheet_TimesheetProject_UploadFileTimesheetProject = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_UploadFileTimesheetProject;
  // Timesheet_TimesheetProject_ViewInvoice = PERMISSIONS_CONSTANT.Timesheet_TimesheetProject_ViewInvoice;
  public listTimesheetByProject: ProjectTimesheetDto[] = [];
  private projectId:number;
  constructor(injector:Injector, 
    public timesheetProjectService: TimesheetProjectService,
    private timesheetService: TimesheetService,
    private timesheetSerivce:TimesheetProjectService, private route:ActivatedRoute, private dialog:MatDialog) {
    super(injector);
    this.projectId = Number(route.snapshot.queryParamMap.get("id"));
   }

  ngOnInit(): void {
    this.getAllTimesheet();
  }
  private getAllTimesheet(){
      this.timesheetSerivce.getAllByProject(this.projectId).pipe(catchError(this.timesheetSerivce.handleError)).subscribe(data=>{
        this.listTimesheetByProject =data.result;
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

  importFile(id:number){
    this.timesheetProjectService.DownloadFileTimesheetProject(id).subscribe(data=>{
        this.getAllTimesheet();
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
  
  downloadFile(projectTimesheet:any){
    this.timesheetProjectService.GetTimesheetFile(projectTimesheet.id).subscribe(data=>{
      const file = new Blob([this.s2ab(atob(data.result.data))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, data.result.fileName);
    })
   
  }
  s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i=0; i!=s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }
  // downloadFile(projectTimesheet:any){
  //   const zip = new JSZip();  

  //   this.timesheetProjectService.GetTimesheetFile(projectTimesheet.id).subscribe(data=>{
  //     const file = new Blob([this.s2ab(atob(data.result.data))], {
  //       type: "application/vnd.ms-excel;charset=utf-8"
  //     })
      // FileSaver.saveAs(file, data.result.fileName);
      // zip.folder("test");
      // zip.generateAsync({ type: 'blob' }).then((content) => {  
      //   if (content) {  
      //     FileSaver.saveAs(file, data.result.fileName);  
      //   }  
      // }); 
      // this.createZip(file,"test")

      // zip.file(data.result.fileName, file);

      // var img = zip.folder("test");
      // img.file("smile.gif", file, {base64: true});


      // zip.generateAsync({type:"blob"}).then(function(content) {
        // see FileSaver.js
  //       FileSaver.saveAs(content, "example.zip");
  //   });
  //   })
  // }


}
