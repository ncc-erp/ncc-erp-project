import { UserService } from './../../../../../../service/api/user.service';
import { catchError } from 'rxjs/operators';
import { Inject, Injector } from '@angular/core';
import { AppComponentBase } from 'shared/app-component-base';
import { Component, OnInit } from '@angular/core';
import { planUserDto } from '@app/service/model/delivery-management.dto';
import { ProjectDto } from '@app/service/model/project.dto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';
import * as moment from 'moment';

@Component({
  selector: 'app-add-future-resource-dialog',
  templateUrl: './add-future-resource-dialog.component.html',
  styleUrls: ['./add-future-resource-dialog.component.css']
})
export class AddFutureResourceDialogComponent extends AppComponentBase implements OnInit {
  public planUser = {} as planUserDto;
  public editUser = {} as planUserDto;
  public listProject: ProjectDto[] = [];
  public projectRoleList = Object.keys(this.APP_ENUM.ProjectUserRole);
  public searchProject: string = ""
  public tomorrowDate = new Date();
  public userList: any[] = [];
  public searchUser:string = "";
  public isEdittingPlan:boolean = false;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private resourceService: ResourceManagerService,
    public injector: Injector, private userSevice:UserService,
    public dialogRef: MatDialogRef<AddFutureResourceDialogComponent>,
    private projectUserService: ProjectUserService) { super(injector) }

  ngOnInit(): void {
    this.tomorrowDate.setDate(this.tomorrowDate.getDate() + 1)
    if (this.data.command != "edit") {
      this.planUser.projectId = this.data.projectId
      this.planUser.allocatePercentage = 100;
      this.planUser.isPool = false;
    }
    else {
      this.planUser = this.data.item;
      this.isEdittingPlan = true;
    }
    this.getAllUser()
  }
  public SaveAndClose() {
    this.isLoading = true
    this.planUser.startTime = moment(this.planUser.startTime).format("YYYY-MM-DD");
    if (this.data.command != "edit") {
      this.resourceService.planUser(this.planUser).pipe(catchError(this.resourceService.handleError)).subscribe((res) => {
        abp.notify.success("Plan Successful");
        this.dialogRef.close(this.planUser);
      }, () => this.isLoading = false);
    }
    else {
      this.resourceService.EditProjectUserPlan(this.planUser).pipe(catchError(this.resourceService.handleError)).subscribe((res) => {
        abp.notify.success("update Successful");
        this.dialogRef.close(this.planUser);
      }, () => this.isLoading = false);
    }
  }

 
  getPercentage(user, data) {
    user.percentUsage = data
  }
  getAllUser(){
    this.userSevice.getAllActiveUser().subscribe(data=>{
      this.userList = data.result
    })
  }

}
