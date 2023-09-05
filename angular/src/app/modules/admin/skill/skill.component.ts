import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { InputFilterDto } from '@shared/filter/filter.component';
import { SkillDto } from './../../../service/model/list-project.dto';
import { SkillService } from './../../../service/api/skill.service';
import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateSkillComponent } from './create-update-skill/create-update-skill.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-skill',
  templateUrl: './skill.component.html',
  styleUrls: ['./skill.component.css']
})
export class SkillComponent extends PagedListingComponentBase<SkillComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: "Tên", },
   
  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
      this.skillService.getAllPaging(request).pipe(finalize(()=>{
        finishedCallback();
      }),catchError(this.skillService.handleError)).subscribe(data=>{
        this.skillList=data.result.items;
        this.showPaging(data.result, pageNumber)
      })
  }

  protected delete(skill: SkillComponent): void {
    abp.message.confirm(
      "Delete Skill " + skill.name+ "?",
      "",
      (result:boolean)=>{
        if(result){
          this.skillService.deleteSkill(skill.id).pipe(catchError(this.skillService.handleError)).subscribe((res)=>{
            abp.notify.success("Delele Skill "+ skill.name);
            this.refresh()
          })
        }
      }
    )
  }
  public skillList:SkillDto[]=[];
  public Admin_Skills_View = PERMISSIONS_CONSTANT.Admin_Skills_View;
  Admin_Skills_Create = PERMISSIONS_CONSTANT.Admin_Skills_Create;
  Admin_Skills_Edit = PERMISSIONS_CONSTANT.Admin_Skills_Edit;
  Admin_Skills_Delete = PERMISSIONS_CONSTANT.Admin_Skills_Delete;
 

  constructor(private skillService:SkillService,
    injector:Injector,
    private dialog:MatDialog) {super(injector) }

  ngOnInit(): void {
    this.refresh();
  }
  public showDialog(command:string , Skill: any){
    let skill={
      name:Skill.name,
      id:Skill.id
    }
    const show= this.dialog.open(CreateUpdateSkillComponent,{
      data:{
        item:skill,
        command:command
      },
      width:"700px"
    })
    show.afterClosed().subscribe((res)=>{
      if(res){
        this.refresh();
      }
    })

  }
  public createSkill(){
    this.showDialog("create",{});
  }
  public editSkill(client){
    this.showDialog("update",client);

  }

}
