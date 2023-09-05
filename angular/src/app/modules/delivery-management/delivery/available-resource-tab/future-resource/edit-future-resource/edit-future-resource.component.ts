import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

import { ProjectUserService } from './../../../../../../service/api/project-user.service';
import { ListProjectService } from './../../../../../../service/api/list-project.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectDto } from './../../../../../../service/model/project.dto';
import { editFutureResourceDto } from './../../../../../../service/model/delivery-management.dto';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import * as moment from 'moment';


@Component({
  selector: 'app-edit-future-resource',
  templateUrl: './edit-future-resource.component.html',
  styleUrls: ['./edit-future-resource.component.css']
})
export class EditFutureResourceComponent extends AppComponentBase implements OnInit {

  
  public editUser={} as editFutureResourceDto;
  public listProject:ProjectDto[]=[];
 

  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
    private listProjectService:ListProjectService,
    public injector:Injector,
    public dialogRef:MatDialogRef<EditFutureResourceComponent>,
    private projectUserService: ProjectUserService){super(injector) }

  ngOnInit(): void {
    this.getAllProject();
    this.editUser=this.data.futureResource;
  }
  public SaveAndClose(){
    if(this.data.command=="update"){
      this.editUser.startTime=moment(this.editUser.startTime).format("YYYY/MM/DD");
      this.projectUserService.update(this.editUser).pipe(catchError(this.projectUserService.handleError)).subscribe((res)=>{
      abp.notify.success("Update future resource for: "+ this.editUser.fullName);
      this.dialogRef.close(this.editUser);

    },()=>this.isLoading=false);
  }
  
  }

  public getAllProject(){
    this.listProjectService.getAll().subscribe(data=>{
      this.listProject=data.result;
      
    })
  }
  // getPercentage(user, data) {
  //   user.allocatePercentage = data
  // }


}
