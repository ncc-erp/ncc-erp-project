import { catchError } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { result } from 'lodash-es';
import { SetupReviewerService } from './../../../../service/api/setup-reviewer.service';
import { UserService } from '@app/service/api/user.service';
import { CheckpointUserDto } from './../../../../service/model/checkpoint-user.dto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';

@Component({
  selector: 'app-create-edit-setup-reviewer',
  templateUrl: './create-edit-setup-reviewer.component.html',
  styleUrls: ['./create-edit-setup-reviewer.component.css']
})
export class CreateEditSetupReviewerComponent extends AppComponentBase implements OnInit {

  public reviewer={} as CheckpointUserDto;
  public userList=[];
  public reviewerList=[];
  public phaseId="";
  public command="";
  public reviewerTypeList: string[] = Object.keys(this.APP_ENUM.CheckPointUserType);
  public searchReviewer="";
  public searchUser="";
  constructor(@Inject(MAT_DIALOG_DATA ) public data: any,
  public userService: UserService,
  public reviewerService: SetupReviewerService,
  public dialogRef: MatDialogRef<CreateEditSetupReviewerComponent>,
  public injector: Injector,
  public route: ActivatedRoute) {
    super(injector);
  }

  ngOnInit(): void {
    this.phaseId= this.route.snapshot.queryParamMap.get('id');
    this.reviewer= this.data.item;
    console.log(this.reviewer)
    this.getAllUsers();
    this.getAllReviewers();

  }
  getAllUsers(){
    
    this.reviewerService.getUserUnreview(this.phaseId).subscribe((data)=>{
      this.userList=data.result;
    })
  }
  getAllReviewers(){
    this.userService.GetAllUserActive(true).subscribe((data)=>{
      this.reviewerList=data.result;
      
    })
    
  }
  onTypeChange(){

  }
  
  SaveAndClose(){
    this.reviewer.phaseId=this.phaseId;
    if (this.data.command == "create") {
      this.reviewerService.createReviewer(true,this.reviewer).pipe(catchError(this.reviewerService.handleError)).subscribe((res) => {
        abp.notify.success("Create reviewer "+this.reviewer.reviewerName+ "successfully");
        this.dialogRef.close(this.reviewer);
      });
    }
    else {
      this.reviewerService.updateReviewer(this.reviewer).pipe(catchError(this.reviewerService.handleError)).subscribe((res) => {
        abp.notify.success("Reviewer "+this.reviewer.reviewerName+" has been edited successfully");
        this.dialogRef.close(this.reviewer);
      });
    }
    
  }
}
