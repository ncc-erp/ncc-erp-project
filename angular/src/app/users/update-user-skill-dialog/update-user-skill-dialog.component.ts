import { catchError } from 'rxjs/operators';
import { result } from 'lodash-es';
import { SkillDto } from './../../service/model/list-project.dto';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SkillService } from '@app/service/api/skill.service';
import { UserService } from '@app/service/api/user.service';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-update-user-skill-dialog',
  templateUrl: './update-user-skill-dialog.component.html',
  styleUrls: ['./update-user-skill-dialog.component.css']
})
export class UpdateUserSkillDialogComponent implements OnInit {
  userSkillList: any[] = []
  skillList: SkillDto[] = []
  skillRankList=[]
  tempSkillList: SkillDto[] = []
  subscription: Subscription[] = [];
  rating:number = 0;
  starCount:number = 5;
  public snackBarDuration: number = 2000;
  public ratingArr = [];
  public searchSkill: string = ""
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private userService: UserService,private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<UpdateUserSkillDialogComponent>,
    private skillService: SkillService) { }

  ngOnInit(): void {
    for (let index = 0; index < this.starCount; index++) {
      this.ratingArr.push(index);
    }
    this.userSkillList = this.data.userSkills.map(skill => skill.skillId)
    this.getAllSkill()
  }

  getAllSkill() {
    this.subscription.push(
      this.skillService.getAll().subscribe(data => {
        this.skillList = data.result
        this.tempSkillList = this.skillList
        this.setSkillRankList()
      })
    )
  }
  onSelectChange(){
    this.skillRankList=[]
    this.setSkillRankList()
  }

  setSkillRankList(){
    this.userSkillList.forEach(data=>{
      if(this.data.userSkills.some(res=>{
        return data == res.skillId
      })){
        this.data.userSkills.forEach(result=>{
          if(data==result.skillId){
            this.skillRankList.push({skillId:result.skillId,skillRank:result.skillRank,name:result.skillName})
          }
        })
      }
      else{
        this.skillList.forEach(item=>{
          if(data==item.id){
            this.skillRankList.push({skillId:item.id,skillRank:0,name:item.name})
          }
        })
      }
    })
  }

  saveAndClose() {
    const userSkills = this.skillRankList.map(skill => { return {skillId:skill.skillId,skillRank:skill.skillRank}})
    let requestBody = {
      userId: this.data.id,
      userSkills:userSkills
    }
    this.subscription.push(
      this.userService.updateUserSkills(requestBody).pipe(catchError(this.userService.handleError)).subscribe(rs => {
        abp.notify.success(`Update skill for user ${this.data.fullName}`)
        this.dialogRef.close(true)
      })
    )
  }

  filterSkill() {
    let selectedSkills = this.tempSkillList.filter(skill=>this.userSkillList.includes(skill.id))
   this.skillList = this.tempSkillList.filter(skill => skill.name.toLowerCase()
    .includes(this.searchSkill.toLowerCase())).filter(s => !this.userSkillList.includes(s.id))
         this.skillList.unshift(...selectedSkills)
  }

  onClick(rating:number,item) {
    this.snackBar.open('You rated ' + rating + ' / ' + this.starCount, '', {
      duration: this.snackBarDuration
    });
    item.skillRank=rating
    return false;
  }

  showIcon(index:number,item) {
    if (item.skillRank>= index + 1) {
      return 'fa fa-star';
    } else {
      return 'far fa-star';
    }
  }

  ngOnDestroy(): void {
    this.subscription.forEach(sub => sub.unsubscribe())
  }
}
