import { APP_ENUMS } from './../../../../../../shared/AppEnums';
import { PERMISSIONS_CONSTANT } from './../../../../../constant/permission.constant';
import { CreateEditTrainingProjectChecklistComponent } from './create-edit-training-project-checklist/create-edit-training-project-checklist.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ProjectChecklistService } from './../../../../../service/api/project-checklist.service';
import { projectChecklistDto } from './../../../../../service/model/checklist.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-training-project-checklist',
  templateUrl: './training-project-checklist.component.html',
  styleUrls: ['./training-project-checklist.component.css']
})
export class TrainingProjectChecklistComponent extends AppComponentBase implements OnInit {
  public listCheckList: projectChecklistDto[] = [];
  public projectId: any;
  public projectType = APP_ENUMS.ProjectType.TRAINING;

  public listChecklistItem = [];
  constructor(private projectChecklistService: ProjectChecklistService,
    public route: ActivatedRoute,
    private dialog: MatDialog,
    injector: Injector) { super(injector) }
  ngOnInit(): void {
    this.projectId = this.route.snapshot.queryParamMap.get('id');
    this.getAllCheckList();
  }
  getAllCheckList() {
    this.projectChecklistService.GetCheckListItemByProject(this.projectId).subscribe(data => {
      this.listCheckList = data.result;
      this.listCheckList.map((item)=>{
        item.mandatories.forEach((type)=>{
          if(type == this.projectType){
            item.checkType = true;
          }
        })
      })
      this.listChecklistItem = this.listCheckList.map(el => el.id);
    })
  }
  showDialog(command: string, projectChecklist: any) {
    const show = this.dialog.open(CreateEditTrainingProjectChecklistComponent, {
      data: {
        command: command,
        listChecklistItem: this.listChecklistItem
      },
      width: '700px'
    })
    show.afterClosed().subscribe(result => {
      if (result) {
        this.getAllCheckList();
      }
    })


  }
  createProjectChecklist() {
    this.showDialog("create", {});
  }
  delete(item){
    abp.message.confirm(
      "Delete project checklist " + item.name + " ?",
      "",
      (result) => {
        this.projectChecklistService.DeleteByProject(this.projectId , item.id).subscribe((res)=>{
          if(res){
            abp.notify.success("Delete " + item.name + " successfully!");
            this.getAllCheckList();
          }
        })
      }
    )
    
  }


}

