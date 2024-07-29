import { Component, Inject, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Route, Router } from '@angular/router';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';
import { UploadFileDto } from '@app/service/model/resource-request.dto';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-import-file-resource',
  templateUrl: './import-file-resource.component.html',
  styleUrls: ['./import-file-resource.component.css']
})
export class ImportFileResourceComponent implements OnInit {
  selectedFiles: FileList;
  currentFileUpload: File;
  public uploadFile = {} as UploadFileDto;
  public isDisable = false;
  public ResourceRequestId: any;

  constructor(
    private dialogRef: MatDialogRef<ImportFileResourceComponent>,
    private resourceService: ResourceManagerService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private router: Router,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.uploadFile.resourceRequestId = this.data.id;
  }
  selectFile(event) {
    this.selectedFiles = event.target.files.item(0);
  }
  importExcel() {
    if (!this.selectedFiles) {
      abp.message.error("Choose a File!");
      return;
    }
    this.resourceService
      .UpdateFileCvLink(
        this.selectedFiles,
        this.uploadFile.resourceRequestId
      )
      .pipe(catchError(this.resourceService.handleError))
      .subscribe((res) => {
        console.log("response: ", res, this.uploadFile.resourceRequestId);
        if (!!res.body) {
          abp.notify.success("Upload file successfully!");
          this.dialogRef.close(this.uploadFile.resourceRequestId);
        }
      });
  }

}
