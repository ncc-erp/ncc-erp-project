import { catchError } from 'rxjs/operators';
import { UploadFileDto } from './../../../../service/model/timesheet.dto';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';
import {TimesheetService} from '@app/service/api/timesheet.service'


@Component({
  selector: 'app-import-file-timesheet-detail',
  templateUrl: './import-file-timesheet-detail.component.html',
  styleUrls: ['./import-file-timesheet-detail.component.css']
})
export class ImportFileTimesheetDetailComponent implements OnInit {
  selectedFiles: FileList;
  currentFileUpload: File;
  public uploadFile= {} as UploadFileDto;
  public isDisable = false;
  public TimesheetProjectId:any;
  constructor(private dialogRef: MatDialogRef<ImportFileTimesheetDetailComponent> ,
    private timesheeService: TimesheetService,
    @Inject(MAT_DIALOG_DATA) public data: any,
  private timesheetService:TimesheetService,
  private router: Router, private dialog: MatDialog,
  private timesheetProjectService: TimesheetProjectService,
  ) { }


  ngOnInit(): void {
    this.uploadFile.TimesheetProjectId= this.data.id;
    
  }
  selectFile(event) {
    this.selectedFiles = event.target.files.item(0);
  }
  

  importExcel() {
    if (!this.selectedFiles) {
      abp.message.error("Choose a file!")
      return
    }
    this.timesheetProjectService.UpdateFileTimeSheetProject(this.selectedFiles, this.uploadFile.TimesheetProjectId ).pipe(catchError(this.timesheetProjectService.handleError))
    .subscribe((res) => {
      console.log("response: ", res, this.uploadFile.TimesheetProjectId)
      if(!!res.body){
        abp.notify.success("Upload file successfully!");
        this.dialogRef.close(this.uploadFile.TimesheetProjectId);
      }
    });
  }
}