import { ResourceManagerService } from './../../../../../../service/api/resource-manager.service';
import { catchError } from 'rxjs/operators';
import { DeliveryResourceRequestService } from './../../../../../../service/api/delivery-request-resource.service';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ListProjectService } from '@app/service/api/list-project.service';

@Component({
  selector: 'app-add-user-to-temp-project-dialog',
  templateUrl: './add-user-to-temp-project-dialog.component.html',
  styleUrls: ['./add-user-to-temp-project-dialog.component.css']
})

export class AddUserToTempProjectDialogComponent extends AppComponentBase implements OnInit {
  public projectRoleList = Object.keys(this.APP_ENUM.ProjectUserRole);
  public searchProject: string = ""
  public user = {} as any
  public listProject: any[] = []
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private resourceService: ResourceManagerService,
   private listProjectService: ListProjectService,
    injector: Injector, public dialogRef: MatDialogRef<AddUserToTempProjectDialogComponent>) {
    super(injector)
  }

  ngOnInit(): void {
    if(this.data.project){
      this.user = this.data.project
    }
    this.user.fullName = this.data.fullName
    this.getAllProject()
  }

  public getAllProject() {
    this.listProjectService.getAll().subscribe(data => {
      this.listProject = data.result;

    })
  }

  SaveAndClose() {
    let requestBody = {
      projectId: this.user.projectId,
      startTime: this.formatDateYMD(this.user.startTime),
      projectRole: this.user.projectRole,
      id: this.data.project.id,
    }
    this.resourceService.updateTempProjectForUser(requestBody).pipe(catchError(this.resourceService.handleError)).subscribe(rs=>{
      abp.notify.success("Update successful")
      this.dialogRef.close(true)
    })
  }
}
