import { TagsService } from './../../../../service/api/tags.service';
import { UserService } from './../../../../service/api/user.service';
import { CheckpointUserDto } from './../../../../service/model/checkpoint-user.dto';
import { SetupReviewerService } from './../../../../service/api/setup-reviewer.service';
import { MatDialog } from '@angular/material/dialog';
import { PhaseService } from './../../../../service/api/phase.service';
import { APP_ENUMS } from './../../../../../shared/AppEnums';
import { AppComponentBase } from '@shared/app-component-base';
import { result } from 'lodash-es';
import { catchError } from 'rxjs/operators';
import { CheckpointResultDto, CheckpointUserEditDto } from './../../../../service/model/result-review.dto';
import { CheckpointUserResultService } from './../../../../service/api/checkpoint-user-result.service';
import { ActivatedRoute, Router } from '@angular/router';

import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, Inject, OnInit, Injector, inject } from '@angular/core';
import { PhaseDto } from '@app/service/model/phase.dto';
import { EditResultReviewerComponent } from './edit-result-reviewer/edit-result-reviewer.component';

@Component({
  selector: 'app-result-reviewer',
  templateUrl: './result-reviewer.component.html',
  styleUrls: ['./result-reviewer.component.css']
})
export class ResultReviewerComponent extends PagedListingComponentBase<ResultReviewerComponent> implements OnInit {
  
  
   
  public phaseId="";
  public phaseName="";
  public phaseType="";
  public reviewerId="";
  // public tag="";
  public listYear=[];
  public year=-1;
  public currentYear=new Date().getFullYear();
  public phaseList=[];
  public reviewerUserList=[];
  public listTags=[];
  
  public listCheckpointResultSub:CheckpointResultDto[]=[];
  public listCheckpointResultMain:CheckpointUserDto[]=[];
  public listStatusCheckpointResult: string[]=Object.keys(APP_ENUMS.CheckpointUserResult);
  public reviewerTypeList: string[] = Object.keys(this.APP_ENUM.CheckPointUserType);
  public reiviewerStatus: string[] = Object.keys(this.APP_ENUM.CheckPointUserStatus); 
  constructor(
    public injector :Injector,
    public route:ActivatedRoute,
    public checkpointUserResultService: CheckpointUserResultService,
    public phaseService: PhaseService,
    public router:Router,
    public dialog: MatDialog,
    public checkpointsUserService: SetupReviewerService,
    public userService: UserService,
    public tabService: TagsService){
    super(injector);
    
  }

  ngOnInit(): void {
    this.phaseId=this.route.snapshot.queryParamMap.get("phaseId");
    this.phaseName=this.route.snapshot.queryParamMap.get("phaseName");
    this.phaseType=this.route.snapshot.queryParamMap.get("type");
    this.reviewerId=this.route.snapshot.queryParamMap.get("id");
    console.log(this.phaseId);
    this.refresh();
    this.getAllReviewers();
    this.getAllTag();
    for(let i=this.currentYear-2; i< this.currentYear+4;i++){
      this.listYear.push(i);
    }
    
  }
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    request.filterItems=this.AddFilterItem(request,"type",this.type)
    request.filterItems=this.AddFilterItem(request,"reviewerName",this.reviewerName)
    request.filterItems=this.AddFilterItem(request,"status",this.status)
    request.filterItems=this.AddFilterItem(request, "tags" , this.tags)
    if(this.phaseType=='1'){
      this.checkpointsUserService.getAllPagingSub(this.phaseId, request).subscribe((data)=>{
        this.listCheckpointResultSub=data.result.items;
        console.log(this.listCheckpointResultSub)
        request.filterItems=this.clearFilter(request,"type",this.type);
        request.filterItems=this.clearFilter(request,"reviewerName",this.reviewerName)
        request.filterItems=this.clearFilter(request,"status",this.status)
      })
    }
    if(this.phaseType=='0'){
      this.checkpointUserResultService.GetAllPagingMain(this.phaseId,request).subscribe((data)=>{
        this.listCheckpointResultMain=data.result.items;
        console.log(this.listCheckpointResultMain)
        request.filterItems=this.clearFilter(request,"type",this.type);
        request.filterItems=this.clearFilter(request,"reviewerName",this.reviewerName)
        request.filterItems=this.clearFilter(request,"status",this.status)
      })
    }
    
    this.setParamToUrl();
  }
  protected delete(entity: ResultReviewerComponent): void {
    throw new Error('Method not implemented.');
  }
  
  public getAll(){
    this.checkpointUserResultService.getAllUserResult(this.phaseId).subscribe((data)=>{
      this.listCheckpointResult=data.result;
    })
    this.setParamToUrl();
  }
  create(){

  }
  done(item){
    this.checkpointUserResultService.Done(item.id).subscribe((res)=>{
      abp.notify.success("Done!");
    })
  }
  public getByEnum(enumValue, enumObject){
    for(const key in enumObject){
      if(enumObject[key]==enumValue){
        return key;
      }
    }
  }
  filterYear(year){

    this.phaseService.getAllPhase(this.year).subscribe((data)=>{
      this.phaseList=data.result;
    })
  }
  phaseSelected="";
  filterPhase(id){
    this.phaseId=id;
    this.phaseList.forEach((ele)=>{
      if(ele.phaseId==id){
        this.phaseType=ele.type;
        this.phaseName=ele.phaseName;
      }
    })
 
    this.refresh();
  // this.checkpointUserResultService.getAllUserResult(id).subscribe((data)=>{

    this.setParamToUrl();
  }
  // choosePhase(item){
  //   this.phaseType=item.type;
  //   this.phaseId=item.phaseId;
  //   this.refresh();
  //   this.setParamToUrl();

  // }
  setParamToUrl(){
    this.router.navigate([],{
      queryParams:{
        phaseId:this.phaseId,
        phaseName:this.phaseName,
        phaseType:this.phaseType
      },
      queryParamsHandling:"merge"
    })
  }
  showDialog(command:String , ReviewerResult:any){
    let reviewer={} as CheckpointResultDto;
    reviewer={
      id:ReviewerResult.id,
      finalNote:ReviewerResult.finalNote,
      nowLevel:ReviewerResult.nowLevel,
      tag:ReviewerResult.tag
    }
    const show= this.dialog.open(EditResultReviewerComponent,{
      data: {
        item: reviewer,
        command: command,
      },
      width: "700px",
      disableClose: true,
    });
    show.afterClosed().subscribe((res)=>{
      if(res){
        this.getAll();
      }
    })



  }
  edit(item){
    this.showDialog("Edit",item);
  }
  
  getAllReviewers(){
    this.userService.GetAllUserActive(true).subscribe((data)=>{
      this.reviewerUserList=data.result;
      console.log(this.reviewerUserList)
      
    })
    
  }
  
  getAllTag(){
    this.tabService.getAll().subscribe((data)=>{
      this.listTags=data.result;
      console.log("hehe", this.listTags)
    })
  }
 

}
