import { catchError } from 'rxjs/operators';
import { AppSessionService } from './../../../../../shared/session/app-session.service';
import { Router, ActivatedRoute } from '@angular/router';
import { SetupReviewerService } from './../../../../service/api/setup-reviewer.service';
import { UserService } from './../../../../service/api/user.service';
import { ReviewUserDto } from './../../../../service/model/reviewUser.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-edit-review-user',
  templateUrl: './create-edit-review-user.component.html',
  styleUrls: ['./create-edit-review-user.component.css']
})
export class CreateEditReviewUserComponent extends AppComponentBase implements OnInit {
  public review= {} as ReviewUserDto;
  public userList=[];
  public reviewerList=[];
  public activePhaseId="";
  public typeReview="";
  public command="";

  public reviewerTypeList: string[] = Object.keys(this.APP_ENUM.CheckPointUserType);
  
  constructor(@Inject(MAT_DIALOG_DATA ) public data: any,public injector:Injector,
  public userService: UserService,
  public reviewerService: SetupReviewerService,
  public dialogRef:MatDialogRef<CreateEditReviewUserComponent>,
  public route: ActivatedRoute,
  public sessionService:AppSessionService) {
    super(injector);
  }

  ngOnInit(): void {
    this.review=this.data.item;
    this.activePhaseId=this.route.snapshot.queryParamMap.get("phaseId");
    this.typeReview=this.data.typeReview;
    this.command= this.data.command;
    console.log(this.typeReview)
    this.getAllUsers();
    this.getAllReviewers();
  }
  SaveAndClose(){
    this.review.phaseId=this.activePhaseId;
    this.review.type=1;
    if(this.command=="create"){
      if(this.typeReview=="ForSelf"){
        this.review.userId=this.sessionService.userId.toString();
        this.reviewerService.createReviewer(true, this.review).pipe(catchError(this.reviewerService.handleError)).subscribe((res)=>{
          abp.notify.success("Create Successful!");
          this.dialogRef.close(this.review);
        })
        
      }else{
        this.review.reviewerId=this.sessionService.userId.toString();
        this.reviewerService.createReviewer(true, this.review).pipe(catchError(this.reviewerService.handleError)).subscribe((res)=>{
          abp.notify.success("Create Successful!");
          this.dialogRef.close(this.review);
        })
      }
    }else{
      if(this.typeReview=="BySelf"){
        this.review.status=1;
        this.review.reviewerId=this.sessionService.userId.toString();
        this.reviewerService.update(this.review).pipe(catchError(this.reviewerService.handleError)).subscribe((res)=>{
          abp.notify.success("Create Successful!");
          this.dialogRef.close(this.review);
        })

      }
    }
    

  }

  getAllUsers(){
    
    this.reviewerService.getUserUnreview(this.activePhaseId).subscribe((data)=>{
      this.userList=data.result;
    })
  }
  getAllReviewers(){
    this.userService.GetAllUserActive(true).subscribe((data)=>{
      this.reviewerList=data.result;
      
    })
    
  }
}
