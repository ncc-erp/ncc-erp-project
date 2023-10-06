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
  userSkillListCr: any[] = []
  skillList: SkillDto[] = []
  unSelectSkillList: SkillDto[] = []
  selectSkillList: SkillDto[] = []
  skillRankList=[]
  subscription: Subscription[] = [];
  isUpdate:boolean = false;
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
    this.isUpdate = this.data.isUpdate
    this.userSkillList = this.data.userSkills.map(skill => skill.skillId)
    this.userSkillListCr = this.data.userSkills.map(skill => skill.skillId)
    this.getAllSkill()
  }

  getAllSkill() {
    this.subscription.push(
      this.skillService.getAll().subscribe(data => {
        this.skillList = data.result
        this.setSkillRankList()
        this.orderListSelect()
      })
    )
  }
  openedChange(e){
    if(!e){
      this.searchSkill = ''
    }
  }
  onSelectChange(id){
    this.skillRankList=[]
    if(this.userSkillListCr.includes(id)){
      this.userSkillListCr = this.userSkillListCr.filter(res => res != id)
      this.userSkillList = [...this.userSkillListCr]
    }
    else{
      this.userSkillListCr.push(id)
      this.userSkillList = [...this.userSkillListCr]
    }
    this.setSkillRankList()
    this.orderListSelect()
  }

  orderListSelect(){
    this.selectSkillList = this.skillList.filter(item => this.userSkillList.includes(item.id))
    this.unSelectSkillList = this.skillList.filter(item => !this.userSkillList.includes(item.id))
    this.skillList = [...this.selectSkillList, ...this.unSelectSkillList]
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
      userSkills: userSkills,
      note: this.data.note
    }
    this.subscription.push(
      this.userService.updateUserSkills(requestBody).pipe(catchError(this.userService.handleError)).subscribe(rs => {
        abp.notify.success(`Update skill for user ${this.data.fullName}`)
        this.dialogRef.close(true)
      })
    )
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

  selectAll(){
    this.skillRankList = this.skillList.map(item => {
      return {skillId:item.id,skillRank:0,name:item.name}
    })
    this.userSkillList = this.skillList.map(item => item.id)
    this.userSkillListCr = [...this.userSkillList]
  }

  clear(){
    this.skillRankList = []
    this.userSkillList = []
    this.userSkillListCr = []
  }

  ngOnDestroy(): void {
    this.subscription.forEach(sub => sub.unsubscribe())
  }
}
