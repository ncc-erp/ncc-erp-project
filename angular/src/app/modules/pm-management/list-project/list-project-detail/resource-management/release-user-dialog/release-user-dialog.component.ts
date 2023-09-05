import { catchError } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';
import * as moment from 'moment';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { PMReportProjectService } from '@app/service/api/pmreport-project.service';

@Component({
  selector: 'app-release-user-dialog',
  templateUrl: './release-user-dialog.component.html',
  styleUrls: ['./release-user-dialog.component.css']
})
export class ReleaseUserDialogComponent implements OnInit {

  public releaseDate: any = new Date()
  public note: string = ""
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private pmReportProService:PMReportProjectService,
   private projectUserService: ProjectUserService,
    public dialogRef: MatDialogRef<ReleaseUserDialogComponent>) { }

  ngOnInit(): void {
    if(this.data.type =="confirmOut"){
      this.releaseDate = moment(this.data.user.startTime).format("YYYY-MM-DD")
    }
  }

  confirmRelease() {
    if (this.data.type === 'confirmOut') {
      let requestBody = {
        projectUserId: this.data.user.id,
        startTime: moment(this.releaseDate).format("YYYY-MM-DD"),
      }
      if (this.data.page == "weekly"){
        this.pmReportProService.ConfirmOutProject(requestBody).pipe(catchError(this.projectUserService.handleError)).subscribe(rs => {
          abp.notify.success(`confirmed for user ${this.data.user.fullName} out project`)
          this.dialogRef.close(true)
        })
      }
      else{
        this.projectUserService.ConfirmOutProject(requestBody).pipe(catchError(this.projectUserService.handleError)).subscribe(rs => {
          abp.notify.success(`confirmed for user ${this.data.user.fullName} out project`)
          this.dialogRef.close(true)
        })
      }
    }
    else {
      let requestBody = {
        releaseDate: moment(this.releaseDate).format("YYYY-MM-DD"),
        note: this.note,
        projectUserId: this.data.user.id
      }
      this.projectUserService.ReleaseUserToPool(requestBody).pipe(catchError(this.projectUserService.handleError)).subscribe(rs => {
        abp.notify.success(`Successful release user ${this.data.user.fullName}`)
        this.dialogRef.close(true)
      })
    }
  }



}
