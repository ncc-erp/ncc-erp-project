import { result } from 'lodash-es';
import { Component, Inject, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Route, Router } from '@angular/router';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';
import { UploadFileDto } from '@app/service/model/resource-request.dto';
import { catchError } from 'rxjs/operators';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';
@Component({
  selector: 'upload-cvPath-resource-requestCV',
  templateUrl: './upload-cvPath-resource-requestCV.component.html',
  styleUrls: ['./upload-cvPath-resource-requestCV.component.css']
})
export class UploadCVPathResourceRequestCV implements OnInit {
  selectedFiles: FileList;
  currentFileUpload: File;
  public uploadFile = {} as UploadFileDto;
  public isDisable = false;
  public ResourceRequestCVId : any;

  constructor(
    public dialogRef: MatDialogRef<UploadCVPathResourceRequestCV>,
    private resourceService: ResourceManagerService,
    private resourceRequestService: DeliveryResourceRequestService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private router: Router,
    private dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.uploadFile.resourceRequestId = this.data.id;
  }
  cancel(){
    this.dialogRef.close()
  }
  selectFile(event) {
    this.selectedFiles = event.target.files.item(0);
  }
 upLoadCVPath(){
  if (!this.selectedFiles) {
    abp.message.error("Choose a File!");
    return;
  }
  this.resourceRequestService
    .UploadCVPath(
      this.selectedFiles,
      this.uploadFile.resourceRequestId
    )
    .pipe(catchError(this.resourceService.handleError))
    .subscribe((res) => {  
      if (res.body != null) {    
        abp.notify.success("Upload file successfully!");
         console.log("Result to close dialog:", res.body.result);
        this.dialogRef.close(res.body.result);
      }
    });
 }
  
}
