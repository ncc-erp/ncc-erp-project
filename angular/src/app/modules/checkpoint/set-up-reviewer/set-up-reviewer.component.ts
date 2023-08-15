import { UserService } from './../../../service/api/user.service';
import { InputFilterDto } from './../../../../shared/filter/filter.component';
import { result } from 'lodash-es';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateEditSetupReviewerComponent } from './create-edit-setup-reviewer/create-edit-setup-reviewer.component';
import { CreateEditSaoDoProjectComponent } from './../../saodo-management/sao-do/sao-do-detail/create-edit-sao-do-project/create-edit-sao-do-project.component';
import {CheckpointUserDto } from './../../../service/model/checkpoint-user.dto';
import { catchError } from 'rxjs/operators';
import { SetupReviewerService } from './../../../service/api/setup-reviewer.service';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, inject, Inject, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-set-up-reviewer',
  templateUrl: './set-up-reviewer.component.html',
  styleUrls: ['./set-up-reviewer.component.css']
})
export class SetUpReviewerComponent extends PagedListingComponentBase<SetUpReviewerComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    request.filterItems=this.AddFilterItem(request,"type",this.type)
    request.filterItems=this.AddFilterItem(request,"reviewerName",this.reviewerName)
    request.filterItems=this.AddFilterItem(request,"status",this.status)
    
    this.pageSizeType=50;
    this.reviewerService.getAllPagging(request, this.phaseId).pipe(catchError(this.reviewerService.handleError)).subscribe((data)=>{
      this.reviewerList= data.result.items;
      this.showPaging(data.result, pageNumber);
      request.filterItems=this.clearFilter(request,"type",this.type);
      request.filterItems=this.clearFilter(request,"reviewerName",this.reviewerName)
      request.filterItems=this.clearFilter(request,"status",this.status)
      

    })
  }
  protected delete(item): void {
    abp.message.confirm(
      "Delete Reviewer " + item.userName+ "?",
      "",
      (result: boolean) => {
        if (result) {
          this.reviewerService.delete(item.id).subscribe(() => {
            abp.notify.success("Deleted TimeSheet " + item.userName);
            this.refresh();
          });
        }
      }
    );
  }
  public reviewerList:CheckpointUserDto[]=[];
  public reviewerUserList=[];
  public phaseId="";
  public phaseName="";
  public phaseType="";
  public reviewerName="";
  public type="";
  public status="";
  public reviewerTypeList: string[] = Object.keys(this.APP_ENUM.CheckPointUserType);
  public reiviewerStatus: string[] = Object.keys(this.APP_ENUM.CheckPointUserStatus);
  public searchUser: string = "";
  public searchReviewer: string = "";
  // public readonly FILTER_CONFIG: InputFilterDto[] = [
  //   { propertyName: 'status', comparisions: [0, 6, 7, 8], displayName: "Status" },
  //   { propertyName: 'reviewerId', comparisions: [0, 6, 7, 8], displayName: "Reviewer Name" },
  //   { propertyName: 'type', comparisions: [0, 6, 7, 8], displayName: "Type" },
  // ];

  constructor(public injector: Injector,
    public reviewerService: SetupReviewerService,
    public dialog:MatDialog,
    private route: ActivatedRoute,
    public userService: UserService
    ) { super(injector) }

  ngOnInit(): void {
    this.refresh();
    this.phaseId=this.route.snapshot.queryParamMap.get("id");
    this.phaseName=this.route.snapshot.queryParamMap.get("name");
    this.phaseType=this.route.snapshot.queryParamMap.get("type");
    this.getAllReviewers();
    console.log(this.phaseId)
  }
  public showDialog(command: string , Reviewer:any){
    let reviewer={} as CheckpointUserDto;
    if(command== "edit"){
      reviewer={
        reviewerId:Reviewer.reviewerId,
        reviewerName:Reviewer.reviewerName,
        userId:Reviewer.userId,
        userName:Reviewer.userName,
        status:Reviewer.status,
        type:Reviewer.type,
        id:Reviewer.id
      }
    }
    const show = this.dialog.open(CreateEditSetupReviewerComponent, {
      data: {
        item: reviewer,
        command: command,
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
  create(){
    this.showDialog("create",{});
  }
  edit(item){
    this.showDialog("edit",item);
  }
  generateReviewer(){
    this.reviewerService.generateReviewer(this.phaseId).subscribe((data)=>{
      abp.notify.success("Generate Success!")
       this.refresh();

    })
  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
  
  getAllReviewers(){
    this.userService.GetAllUserActive(true).subscribe((data)=>{
      this.reviewerUserList=data.result;
      
    })
    
  }
  filterByReviewerName(){
    this.refresh();
      
  }
  filerByType(){
    this.refresh();
    
  }
  filterByStatus(){
    this.refresh();
      
  }
  showDetail(item){
    this.router.navigate(['app/result-reviewer'], {
      queryParams: {
        phaseId: this.phaseId,
        phaseName:this.phaseName,
        type:this.phaseType,
        id:item.id,
      }
    })
  }

}
