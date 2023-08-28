import { SkillService } from './../../../../../service/api/skill.service';
import { catchError } from 'rxjs/operators';
import { ProjectDto, SkillDto } from './../../../../../service/model/list-project.dto';
import { ListProjectService } from './../../../../../service/api/list-project.service';
import { TrainingRequestDto} from './../../../../../service/model/delivery-management.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector, ChangeDetectorRef, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import {TrainingRequestService} from '@app/service/api/training-request.service'

@Component({
  selector: 'app-create-update-training-request',
  templateUrl: './create-update-training-request.component.html',
  styleUrls: ['./create-update-training-request.component.css']
})
export class CreateUpdateTrainingRequestComponent extends AppComponentBase implements OnInit {

  public isLoading: boolean = false;
  public listProject: ProjectDto[] = [];
  public priorityList: string[] = Object.keys(this.APP_ENUM.Priority)
  public trainingRequestDto = {} as TrainingRequestDto;
  public title: string = ""
  public searchProject: string = ""
  public isAddingSkill: boolean = false
  public listSkill: SkillDto[] = []
  public listSkillDetail: any[] = []
  public filteredSkillList: SkillDto[] = []
  public typeControl: string    //include: request, requestProject

  year: number = new Date().getFullYear();
  month: number = new Date().getMonth();

  constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private listProjectService: ListProjectService,
    private trainingRequestService: TrainingRequestService,
    private skillService: SkillService,
    public dialogRef: MatDialogRef<CreateUpdateTrainingRequestComponent>,
    public ref: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllProject();
    this.typeControl = this.data.typeControl
    if (this.data.command == 'create') {
      this.trainingRequestDto = new TrainingRequestDto()
      //if create from resource request project then command = 'create' & projectId != 0
      //assign projectId in dto = projectId
      if(this.month == 12){
        this.trainingRequestDto.timeNeed = new Date(this.year + 1, 1, 15);
      }else{
        this.trainingRequestDto.timeNeed = new Date(this.year, this.month + 1, 15);
      }
      if (this.data.item.projectId > 0) {
        this.trainingRequestDto.projectId = this.data.item.projectId
      }
    }
    else {
      this.getResourceRequestById(this.data.item.id)
    }
    this.listSkill = this.data.skills
    this.filteredSkillList = this.data.skills

    this.trainingRequestDto.level = this.APP_ENUM.UserLevel.Intern_0
  }

  ngAfterViewChecked(): void {
    //Called after every check of the component's view. Applies to components only.
    //Add 'implements AfterViewChecked' to the class.
    this.ref.detectChanges()
  }

  getResourceRequestById(id: number) {
    this.trainingRequestService.getResourceRequestById(id).subscribe(res => {
      this.trainingRequestDto = res.result
      this.title = res.result.name
    })
  }

  addSkill() {
    this.listSkillDetail.push({ pending: true })
    this.isAddingSkill = true
  }

  SaveAndClose() {
    this.isLoading = true;
    // this.resourceRequestTrainingDto.timeNeed = moment(this.resourceRequestTrainingDto.timeNeed).format("YYYY/MM/DD");
    let request = {
      name: '',
      projectId: this.trainingRequestDto.projectId,
      timeNeed: this.formatDateYMD(this.trainingRequestDto.timeNeed),
      level: this.trainingRequestDto.level,
      priority: this.trainingRequestDto.priority,
      id: this.trainingRequestDto.id,
      skillIds: this.trainingRequestDto.skillIds,
      quantity: this.trainingRequestDto.quantity
    }

    if (this.data.command == "create") {
      request.id = 0
      let createRequest = { ...request, quantity: this.trainingRequestDto.quantity };
      this.trainingRequestService.create(createRequest).pipe(catchError(this.trainingRequestService.handleError)).subscribe((res) => {
        abp.notify.success("Create Successfully!");
        this.dialogRef.close(this.trainingRequestDto);
      }, () => this.isLoading = false)
    } else {
      this.trainingRequestService.update(request).pipe(catchError(this.trainingRequestService.handleError)).subscribe((res) => {
        let updateRequest = { ...request, }
        abp.notify.success("Update Successfully!");
        this.dialogRef.close(res.result);
      }, () => this.isLoading = false)
    }
  }

  getAllProject() {
    this.listProjectService.getMyTrainingProjects().subscribe(data => {
      this.listProject = data.result;
    })
  }

  getSkillDetail() {
    if (this.data.command == "edit") {
      this.trainingRequestService.GetSkillDetail(this.data.item.id).pipe(catchError(this.trainingRequestService.handleError)).subscribe(data => {
        this.listSkillDetail = data.result

        let b = this.listSkillDetail.map(item => item.skillId)
        this.trainingRequestDto.skillIds = b
        console.log(this.trainingRequestDto.skillIds)
      })
    }
  }

  pushSkill(skill) {
    this.listSkillDetail.push(skill)
  }

  removeSkill(skill) {
    if (skill.id) {
      this.trainingRequestService.deleteSkill(skill.id).pipe(catchError(this.skillService.handleError)).subscribe(rs => {
        abp.notify.success("delete Successful")
        this.getSkillDetail()
      })
    }
    else {
      this.listSkillDetail.splice(this.listSkillDetail.indexOf(skill), 1)
    }
    this.isAddingSkill = false
  }

  saveSkill(skill) {
    if (skill.skillId && skill.quantity > 0) {
      skill.resourceRequestId = this.data.item.id
      this.trainingRequestService.createSkill(skill).pipe(catchError(this.trainingRequestService.handleError)).subscribe(rs => {
        abp.notify.success("added new Skill")
        this.getSkillDetail()
      })
    }
    else {
      abp.notify.error("Skill or Quantity not valid")
    }
    this.isAddingSkill = false
  }

  focusOut() {
    this.searchProject = '';
  }

  filterSkills(event) {
    this.filteredSkillList = this.listSkill.filter(
      (skill) => this.l(skill.name.toLowerCase()).includes(
        this.l(event.toLowerCase())
      )
    );
  }

}
