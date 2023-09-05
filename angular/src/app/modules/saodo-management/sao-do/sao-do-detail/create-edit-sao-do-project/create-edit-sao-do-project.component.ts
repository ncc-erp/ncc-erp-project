import { AppComponentBase } from '@shared/app-component-base';

import { ActivatedRoute } from '@angular/router';
import { UserService } from './../../../../../service/api/user.service';
import { catchError } from 'rxjs/operators';
import { AuditResultService } from './../../../../../service/api/auditresult.service';
import { ProjectDto } from './../../../../../service/model/list-project.dto';
import { ListProjectService } from './../../../../../service/api/list-project.service';
import { SaodoDetailDto, ProjectSaodoDto } from './../../../../../service/model/saodo.dto';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector } from '@angular/core';

@Component({
  selector: 'app-create-edit-sao-do-project',
  templateUrl: './create-edit-sao-do-project.component.html',
  styleUrls: ['./create-edit-sao-do-project.component.css']
})
export class CreateEditSaoDoProjectComponent extends AppComponentBase implements OnInit {
  public statusList: string[] = Object.keys(this.APP_ENUM.SaodoStatus);
  public saodoProject= {} as ProjectSaodoDto;
  public projectList=[];
  public pmList=[];
  public projectName:any;
  
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
  private projectService: ListProjectService, private auditResultService: AuditResultService,
  public dialogRef: MatDialogRef<CreateEditSaoDoProjectComponent>,
  private userService: UserService,private route: ActivatedRoute, injector:Injector) {super(injector) }

  ngOnInit(): void {
    this.projectName = this.data.projectSaodoName;
    this.saodoProject=this.data.item;
    this.saodoProject.auditSessionId=this.route.snapshot.queryParamMap.get('id');
    
    this.getAllProject();
    this.getAllUser();
  }
  SaveAndClose(){
    this.isLoading = true;
    if (this.data.command == "create") {
      this.auditResultService.create(this.saodoProject).pipe(catchError(this.auditResultService.handleError)).subscribe((res) => {
        abp.notify.success("Created timesheet detail successfully");
        this.dialogRef.close(this.saodoProject)
      }, () => this.isLoading = false);
    }
    else {
      this.auditResultService.update(this.saodoProject).pipe(catchError(this.auditResultService.handleError)).subscribe((res) => {
        abp.notify.success("Edited timesheet detail successfully");
        this.dialogRef.close(this.saodoProject)

      }, () => this.isLoading = false);
    }
  }
  getAllProject() {
    this.projectService.getAll().subscribe(data => {
      this.projectList = data.result.filter(item =>!this.projectName.includes(item.name) && item.status!=2);
    })
  }
  getAllUser() {
    this.userService.GetAllUserActive(true).subscribe(data => {
      this.pmList = data.result;
      this.saodoProject.pmId=this.pmList.filter(el=>el.name == this.saodoProject.pmName)[0]?.id;
    })
  }

}
