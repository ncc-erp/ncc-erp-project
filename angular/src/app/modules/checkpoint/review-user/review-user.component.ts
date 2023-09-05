import { element } from 'protractor';
import { Router } from '@angular/router';
import { getAllPhaseDto } from './../../../service/model/phase.dto';
import { result } from 'lodash-es';
import { PhaseService } from './../../../service/api/phase.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateEditReviewUserComponent } from './create-edit-review-user/create-edit-review-user.component';
import { catchError, filter } from 'rxjs/operators';
import { ReviewUserDto } from './../../../service/model/reviewUser.dto';
import { SetupReviewerService } from './../../../service/api/setup-reviewer.service';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { ResultReviewerComponent } from '../set-up-reviewer/result-reviewer/result-reviewer.component';
import { mixinTabIndex } from '@angular/material/core';
import { MatTabChangeEvent } from '@angular/material/tabs';

@Component({
  selector: 'app-review-user',
  templateUrl: './review-user.component.html',
  styleUrls: ['./review-user.component.css']
})
export class ReviewUserComponent extends PagedListingComponentBase<ResultReviewerComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.pageSizeType = 50;
    this.checkpointUserService.getAllReviewForSelf(request).pipe(catchError(this.checkpointUserService.handleError)).subscribe((data) => {
      this.reviewUserForSelf = data.result.items;
      this.showPaging(data.result, pageNumber);
    })
    this.checkpointUserService.getAllReviewBySelf(request).pipe(catchError(this.checkpointUserService.handleError)).subscribe((data) => {
      this.reviewUserBySelf = data.result.items;
      this.showPaging(data.result, pageNumber);
    })
    this.setParamToUrl();

  }
  protected delete(item): void {
    abp.message.confirm(
      "Delete Review " + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.checkpointUserService.delete(item.id).subscribe(() => {
            abp.notify.success("Deleted Reviewed");
            this.refresh();
          });
        }
      }
    );
  }
  public reviewUserForSelf: ReviewUserDto[] = [];
  public reviewUserBySelf: ReviewUserDto[] = [];
  public year = new Date().getFullYear();
  public listYear: number[] = [];
  private currentYear = new Date().getFullYear();
  public tabIndex=0;


  constructor(public injector: Injector,
    public checkpointUserService: SetupReviewerService,
    public dialog: MatDialog,
    public phaseService: PhaseService,
    public router: Router
  ) { super(injector) }

  ngOnInit(): void {
    for (let i = this.currentYear - 4; i < this.currentYear + 2; i++) {
      this.listYear.push(i)
    }


    this.refresh();
    this.getAllPhase();
  }
  onTabChange(e:MatTabChangeEvent){
    this.tabIndex=e.index;

  }
  showDialog(command: string, Review: any, typeReview:string) {
    let review = {} as ReviewUserDto;
   
   
    if (command == "edit") {
      review = {
        reviewerId: Review.reviewerId,
        reviewerName: Review.reviewerName,
        userId: Review.userId,
        userName: Review.userName,
        status: Review.status,
        type: Review.type,
        id: Review.id,
        note: Review.note,
        score: Review.score,

      }
    }
    const show = this.dialog.open(CreateEditReviewUserComponent, {
      data: {
        item: review,
        command: command,
        typeReview:typeReview
      },
      width: "700px",
      disableClose: true,
    });
    show.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }
  create() {
    if(this.tabIndex==0){
      this.showDialog("create", {},"ForSelf");
      console.log("Forself");
    }else{
      this.showDialog("create", {},"BySelf");
      console.log("Byself");
    }
    
  }
  edit(review: ReviewUserDto) {
    if(this.tabIndex==0){
      this.showDialog("edit", review,"ForSelf");
      console.log("Forself");
    }else{
      this.showDialog("edit", review,"BySelf");
      console.log("Byself");
    }
    
  }
  phaseList: getAllPhaseDto[] = []
  phaseActiveId="";
  phaseType="";
  getAllPhase() {
    this.phaseService.getAllPhase(this.year).subscribe((data) => {
      this.phaseList = data.result;
      data.result.forEach(element => {
        if(element.status==0){
          this.phaseActiveId=element.phaseId;
          this.setParamToUrl();
          this.phaseType=element.type;
        }
      
      })
      console.log(this.phaseActiveId)
    })
  }
  
  filterYear(year){
    this.phaseService.getAllPhase(this.year).subscribe((data) => {
      this.phaseList = data.result;
    })
  }
  filterByPhase(id){
  
    this.setParamToUrl();
  }
  setParamToUrl() {
    this.router.navigate([], {
      queryParams: {
        phaseId: this.phaseActiveId,
      },
      queryParamsHandling: "merge"
      // nối thêm param vào các param hiện tại
      
    })
  }
  
  
  

}
