import { SkillService } from './../../../../../service/api/skill.service';
import { catchError, map, startWith } from 'rxjs/operators';
import { DeliveryResourceRequestService } from './../../../../../service/api/delivery-request-resource.service';
import { result } from 'lodash-es';
import { ProjectDto, SkillDto } from './../../../../../service/model/list-project.dto';
import { ListProjectService } from './../../../../../service/api/list-project.service';
import { RequestResourceDto, ResourceRequestDetailDto } from './../../../../../service/model/delivery-management.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { APP_ENUMS } from './../../../../../../shared/AppEnums';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector, ChangeDetectorRef } from '@angular/core';
import * as moment from 'moment';
import * as _ from 'lodash'
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { Observable } from 'rxjs';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-create-update-resource-request',
  templateUrl: './create-update-resource-request.component.html',
  styleUrls: ['./create-update-resource-request.component.css']
})
export class CreateUpdateResourceRequestComponent extends AppComponentBase implements OnInit {
  public isLoading: boolean = false;
  public listProject: ProjectDto[] = [];
  public priorityList: string[] = Object.keys(this.APP_ENUM.Priority)
  public userLevelList: string[] = Object.keys(this.APP_ENUM.UserLevel)
  public resourceRequestDto = {} as RequestResourceDto;
  public title: string = ""
  public searchProject: string = ""
  public listRequestCode = []
  public isAddingSkill: boolean = false
  public listSkill: SkillDto[] = []
  public listSkillDetail: any[] = []
  public filteredSkillList: any[] = []
  public typeControl: string  //include: request, requestProject

  constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private listProjectService: ListProjectService,
    private resourceRequestService: DeliveryResourceRequestService,
    private skillService: SkillService,
    public dialogRef: MatDialogRef<CreateUpdateResourceRequestComponent>,
    public ref: ChangeDetectorRef
  )
  {
    super(injector);
  }

  filteredOptions: Observable<any[]>;
  myControl = new FormControl('');

  ngOnInit(): void {
    this.listRequestCode = this.data.listRequestCode
    this.getAllProject();
    this.typeControl = this.data.typeControl
    if(this.data.command == 'create'){
      this.resourceRequestDto = new RequestResourceDto()
      if(this.data.item.projectId > 0){
        this.resourceRequestDto.projectId = this.data.item.projectId
      }
    }
    else{
      this.getResourceRequestById(this.data.item.id)
    }
    this.listSkill = this.data.skills
    this.filteredSkillList = this.data.skills
    this.userLevelList = this.data.levels
    this.filteredOptions = this.myControl.valueChanges.pipe(
      startWith(''),
      map(value => {
        const name = typeof value === 'string' ? value : value?.code;
        return name ? this._filter(name as string) : this.listRequestCode.slice();
      }),
    );
  }
  
   _filter(name: string) {
    const filterValue = name.toLowerCase();
    return this.listRequestCode.filter(option => option.code.toLowerCase().includes(filterValue));
  }

  displayFn(data): string {
    return data && data ? data : '';
  }

  ngAfterViewChecked(): void {
    //Called after every check of the component's view. Applies to components only.
    //Add 'implements AfterViewChecked' to the class.
    this.ref.detectChanges()
  }

  getResourceRequestById(id: number){
    this.resourceRequestService.getResourceRequestById(id).subscribe(res => {
      this.resourceRequestDto = res.result
      this.myControl.setValue(this.resourceRequestDto.code)
      this.title = res.result.name
    })
  }

  addSkill() {
    this.listSkillDetail.push({ pending: true })
    this.isAddingSkill = true
  }

  SaveAndClose() {
    this.isLoading = true;
    // this.resourceRequestDto.timeNeed = moment(this.resourceRequestDto.timeNeed).format("YYYY/MM/DD");
    let request = {
      name: '',
      projectId: this.resourceRequestDto.projectId,
      timeNeed: this.formatDateYMD(this.resourceRequestDto.timeNeed),
      level: this.resourceRequestDto.level,
      priority: this.resourceRequestDto.priority,
      id: this.resourceRequestDto.id,
      skillIds: this.resourceRequestDto.skillIds,
      isNewBillAccount: this.resourceRequestDto.isNewBillAccount,
      code: this.myControl.value
    }

    if (this.data.command == "create") {
      request.id = 0
      let createRequest = { ...request, quantity: this.resourceRequestDto.quantity};
      this.resourceRequestService.create(createRequest).pipe(catchError(this.resourceRequestService.handleError)).subscribe((res) => {
        abp.notify.success("Create Successfully!");
        this.dialogRef.close(this.resourceRequestDto);
      }, () => this.isLoading = false)
    } else {
      this.resourceRequestService.update(request).pipe(catchError(this.resourceRequestService.handleError)).subscribe((res) => {
        abp.notify.success("Update Successfully!");
        this.dialogRef.close(res.result);
      }, () => this.isLoading = false)
    }
  }

  getAllProject() {
    this.listProjectService.getMyProjects().subscribe(data => {
      this.listProject = data.result;
    })
  }

  getallskill() {
    this.skillService.getAll().pipe(catchError(this.skillService.handleError)).subscribe(data => {
      this.listSkill = data.result
      this.temp =data.result
      this.getSkillDetail();
    })
  }

  temp
  getSkillDetail() {
    if (this.data.command == "edit") {
      this.resourceRequestService.GetSkillDetail(this.data.item.id).pipe(catchError(this.resourceRequestService.handleError)).subscribe(data => {
        this.listSkillDetail = data.result

        let b = this.listSkillDetail.map(item => item.skillId)
        this.resourceRequestDto.skillIds = b
      })
    }
  }

  pushSkill(skill) {
    this.listSkillDetail.push(skill)
  }

  removeSkill(skill) {
    if (skill.id) {
      this.resourceRequestService.deleteSkill(skill.id).pipe(catchError(this.skillService.handleError)).subscribe(rs => {
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
      this.resourceRequestService.createSkill(skill).pipe(catchError(this.resourceRequestService.handleError)).subscribe(rs => {
        abp.notify.success("added new Skill")
        this.getSkillDetail()
      })
    }
    else {
      abp.notify.error("Skill or Quantity not valid")
    }
    this.isAddingSkill = false

  }

  focusOut(){
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
