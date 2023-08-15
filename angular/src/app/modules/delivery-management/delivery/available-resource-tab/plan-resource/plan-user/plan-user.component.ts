import { ResourceManagerService } from './../../../../../../service/api/resource-manager.service';
import { ProjectUserService } from './../../../../../../service/api/project-user.service';
import { catchError } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from '../../../../../../service/api/delivery-request-resource.service';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectDto } from '../../../../../../service/model/project.dto';
import { ListProjectService } from '../../../../../../service/api/list-project.service';
import { planUserDto } from '../../../../../../service/model/delivery-management.dto';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'app-plan-user',
  templateUrl: './plan-user.component.html',
  styleUrls: ['./plan-user.component.css']
})
export class PlanUserComponent extends AppComponentBase implements OnInit {
  public planUser = {} as planUserDto;
  public editUser = {} as planUserDto;
  public listProject: ProjectDto[] = [];
  public projectRoleList = Object.keys(this.APP_ENUM.ProjectUserRole);
  public searchProject: string = ""
  public tomorrowDate = new Date();

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    private listProjectService: ListProjectService,
    private resourceService: ResourceManagerService,
    public injector: Injector,
    public dialogRef: MatDialogRef<PlanUserComponent>,
    private projectUserService: ProjectUserService) { super(injector) }

  ngOnInit(): void {
    this.tomorrowDate.setDate(this.tomorrowDate.getDate() + 1)
    this.getAllProject();
    if (this.data.command != "edit") {
      this.planUser.startTime = moment(this.tomorrowDate).format("YYYY-MM-DD");
      this.planUser.isPool = false
      this.planUser.allocatePercentage = 100
    }
    else {
      this.planUser.startTime = moment(this.planUser.startTime).format("YYYY-MM-DD");
      this.planUser = this.data.item
    }
    this.planUser.userId = this.data.item.userId;
    this.planUser.fullName = this.data.item.fullName;
  }
  public SaveAndClose() {
    this.isLoading = true
    this.planUser.startTime = moment(this.planUser.startTime).format("YYYY-MM-DD");
    if (this.data.command != "edit") {
      this.resourceService.planUser(this.planUser).pipe(catchError(this.resourceService.handleError)).subscribe((res) => {
        abp.notify.success("Planed Successfully!");
        this.dialogRef.close(this.planUser);
      }, () => this.isLoading = false);
    }
    else {
      this.resourceService.EditProjectUserPlan(this.planUser).pipe(catchError(this.resourceService.handleError)).subscribe((res) => {
        abp.notify.success("Planed Successfully!");
        this.dialogRef.close(this.planUser);
      }, () => this.isLoading = false);
    }
  }

  public getAllProject() {
    this.listProjectService.getAllFilter(true).subscribe(data => {
      this.listProject = data.result;

    })
  }
  getPercentage(user, data) {
    user.percentUsage = data
  }


}
