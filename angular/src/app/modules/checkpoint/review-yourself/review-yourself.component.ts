import { result } from 'lodash-es';
import { ReviewYourselfDto } from './../../../service/model/review-yourself.dto';
import { ReviewUserDto } from './../../../service/model/reviewUser.dto';
import { CheckpointUserDetailService } from './../../../service/api/checkpoint-user-detail.service';
import { AppSessionService } from './../../../../shared/session/app-session.service';
import { PhaseService } from '@app/service/api/phase.service';
import { getAllPhaseDto } from './../../../service/model/phase.dto';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-review-yourself',
  templateUrl: './review-yourself.component.html',
  styleUrls: ['./review-yourself.component.css']
})
export class ReviewYourselfComponent implements OnInit {

  constructor(public phaseService: PhaseService,public sessionService:AppSessionService,
  public  checkpointDetailService: CheckpointUserDetailService) { }
  public year = new Date().getFullYear();
  public listYear: number[] = [];
  public listReviewYourself={} as ReviewYourselfDto;
  public listDetail=[];
  private currentYear = new Date().getFullYear();
  public loginUser= this.sessionService.userId;
  phaseIdActive=0;
    list=[1,2,3]
  ngOnInit(): void {
    for (let i = this.currentYear - 4; i < this.currentYear + 2; i++) {
      this.listYear.push(i)
    }
    this.getAllPhase();
    console.log("id",this.sessionService.userId);
    this.getAllUserDetail();
  }
  phaseList: getAllPhaseDto[] = []
  getAllPhase() {
    this.phaseService.getAllPhase(this.year).subscribe((data) => {
      this.phaseList = data.result;
      this.phaseList.forEach((item)=>{
        if(item.status==0){
          this.phaseIdActive=item.phaseId;
        }
      })

      
    })
  }
  getShowHistory(){
    
  }
  getAllUserDetail(){
    this.checkpointDetailService.getAllUserDetail(10015,120018).subscribe((data)=>{
      this.listReviewYourself=data.result;
      this.listDetail=data.result.listDetail;
      console.log(data.result)
    })
    console.log("gi")
  }
  submit(){
    
  }
  
}
