import { SkillService } from './../../../../service/api/skill.service';
import { SkillDto } from './../../../../service/model/list-project.dto';
import { Inject, Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-create-update-skill',
  templateUrl: './create-update-skill.component.html',
  styleUrls: ['./create-update-skill.component.css']
})
export class CreateUpdateSkillComponent extends AppComponentBase implements OnInit {
  public title:string ="";
  public skill={} as SkillDto;
  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
    public injector:Injector,
    public skillService: SkillService,
    public dialogRef:MatDialogRef<CreateUpdateSkillComponent>,) {super(injector) }

  ngOnInit(): void {
    this.skill=this.data.item;
    this.title =this.skill.name
  }
  SaveAndClose(){
    if(this.data.command == "create"){
      this.skillService.create(this.skill).pipe(catchError(this.skillService.handleError)).subscribe((res)=>{
        abp.notify.success("Create Skill Successfully!");
        this.dialogRef.close(this.skill);
      },()=>{this.isLoading=false})
    }
    else{
      this.skillService.update(this.skill).pipe(catchError(this.skillService.handleError)).subscribe((res)=>{
        abp.notify.success("Update Skill Successfully!");
        this.dialogRef.close(this.skill);
      },()=>{this.isLoading=false})
    }

  }

}
