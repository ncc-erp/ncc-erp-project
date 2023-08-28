import { ProjectResourceRequestService } from './../../../../../../service/api/project-resource-request.service';
import { catchError } from 'rxjs/operators';
import { UserDto } from './../../../../../../../shared/service-proxies/service-proxies';
import { UserService } from './../../../../../../service/api/user.service';
import { projectResourceRequestDto, projectUserDto } from './../../../../../../service/model/project.dto';
import { DialogDataDto } from './../../../../../../service/model/common-DTO';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-approve-dialog',
  templateUrl: './approve-dialog.component.html',
  styleUrls: ['./approve-dialog.component.css']
})
export class ApproveDialogComponent extends AppComponentBase implements OnInit {
  public resourcerequest = {} as projectUserDto;
  public searchUser: string = "";
  public userList: UserDto[] = [];
  public projectRoleList = Object.keys(this.APP_ENUM.ProjectUserRole)
  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogDataDto, injector: Injector,
   private userService: UserService, private pmReportService:ProjectResourceRequestService,
   public dialogRef:MatDialogRef<ApproveDialogComponent>) {
    super(injector);
  
  }

  ngOnInit(): void {
    this.getAllUser();
    this.resourcerequest = this.data.dialogData
    this.resourcerequest.projectRole = this.APP_ENUM.ProjectUserRole[this.resourcerequest.projectRole]
  }
  // public getAllUser(): void {
  //   this.userService.GetAllUserActive(true).pipe(catchError(this.userService.handleError)).subscribe(data => this.userList = data.result);
  // }
  public getAllUser(): void {
    this.userService.GetAllUserActive(false).pipe(catchError(this.userService.handleError)).subscribe(data => {
      this.userList = data.result;
    })
  }
  public saveAndClose(): void{
    this.resourcerequest.startTime = moment(this.resourcerequest.startTime).format("YYYY-MM-DD")
    this.isLoading =true;
    this.pmReportService.approveRequest(this.resourcerequest).pipe(catchError(this.pmReportService.handleError)).subscribe(res=>{
      abp.notify.success(`Approved!`);
      this.dialogRef.close(this.resourcerequest);
      this.isLoading =false;
    },()=>{
      this.isLoading =false;
    })
  }
  getFuturePercentage(report, data) {
    report.allocatePercentage = data
  }
}
