import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectResourceRequestService } from './../../../../../../service/api/project-resource-request.service';
import { UserService } from './../../../../../../service/api/user.service';
import { DialogDataDto } from './../../../../../../service/model/common-DTO';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserDto } from './../../../../../../../shared/service-proxies/service-proxies';
import { projectUserDto } from './../../../../../../service/model/project.dto';
import { Component, Inject, OnInit, Injector } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-product-approved-dialog',
  templateUrl: './product-approved-dialog.component.html',
  styleUrls: ['./product-approved-dialog.component.css']
})
export class ProductApprovedDialogComponent extends AppComponentBase implements OnInit {

  public resourceRequest = {} as projectUserDto;
  public searchUser: string = "";
  public userList: UserDto[] = [];
  public projectRoleList = Object.keys(this.APP_ENUM.ProjectUserRole)
  constructor(@Inject(MAT_DIALOG_DATA) public data: DialogDataDto, injector: Injector,
   private userService: UserService, private pmReportService:ProjectResourceRequestService,
   public dialogRef:MatDialogRef<ProductApprovedDialogComponent>) {
    super(injector);
  
  }

  ngOnInit(): void {
    this.getAllUser();
    this.resourceRequest = this.data.dialogData
    this.resourceRequest.projectRole = this.APP_ENUM.ProjectUserRole[this.resourceRequest.projectRole]
  }
  public getAllUser(): void {
    this.userService.GetAllUserActive(false).pipe(catchError(this.userService.handleError)).subscribe(data => {
      this.userList = data.result;
    })
  }
  public saveAndClose(): void{
    this.resourceRequest.startTime = moment(this.resourceRequest.startTime).format("YYYY-MM-DD")
    this.isLoading =true;
    this.pmReportService.approveRequest(this.resourceRequest).pipe(catchError(this.pmReportService.handleError)).subscribe(res=>{
      abp.notify.success(`Approved!`);
      this.dialogRef.close(this.resourceRequest);
      this.isLoading =false;
    },()=>{
      this.isLoading =false;
    })
  }
  getFuturePercentage(report, data) {
    report.allocatePercentage = data
  }
}
