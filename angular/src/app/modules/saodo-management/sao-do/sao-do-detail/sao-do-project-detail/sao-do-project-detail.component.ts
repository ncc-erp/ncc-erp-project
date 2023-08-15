import { projectUserDto } from './../../../../../service/model/project.dto';
import { AuditResultService } from './../../../../../service/api/auditresult.service';
import { catchError } from 'rxjs/operators';
import { AuditResultPeopleService } from './../../../../../service/api/audit-result-people.service';

import { ProjectUserService } from './../../../../../service/api/project-user.service';
import { projectChecklistDto } from './../../../../../service/model/checklist.dto';
import { ProjectChecklistService } from './../../../../../service/api/project-checklist.service';
import { AppComponentBase } from '@shared/app-component-base';
import { PERMISSIONS_CONSTANT } from './../../../../../constant/permission.constant';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';
import { SaodoProjectUserDto } from '@app/service/model/saodo.dto';

@Component({
  selector: 'app-sao-do-project-detail',
  templateUrl: './sao-do-project-detail.component.html',
  styleUrls: ['./sao-do-project-detail.component.css']
})
export class SaoDoProjectDetailComponent extends AppComponentBase implements OnInit {
  public examinationName='';
  public projectName='';
  public projectId:any;
  public auditSessionId='';
  public projectUserList=[];
  public isEditing:boolean=false;
  public isEditingNote:boolean=false;
  public projectUser={} as SaodoProjectUserDto;
  public note='';
  public auditResultId='';
  public listCheckList:projectChecklistDto[]=[];
  // SaoDo_AuditResult_Create=PERMISSIONS_CONSTANT.SaoDo_AuditResult_Create;
  // SaoDo_AuditResult_Delete=PERMISSIONS_CONSTANT.SaoDo_AuditResult_Delete;
  // SaoDo_AuditResult_GetNote=PERMISSIONS_CONSTANT.SaoDo_AuditResult_GetNote;
  // SaoDo_AuditResult_Update=PERMISSIONS_CONSTANT.SaoDo_AuditResult_Update;
  // SaoDo_AuditResult_UpdateNote=PERMISSIONS_CONSTANT.SaoDo_AuditResult_UpdateNote;

  constructor(private route: ActivatedRoute , injector:Injector,
    private projectChecklistService:ProjectChecklistService,
    private projectUserService:ProjectUserService,
    private auditProjectResultService: AuditResultPeopleService,
    private auditResultService: AuditResultService) {super(injector) }
  ngOnInit(): void {
    this.examinationName=this.route.snapshot.queryParamMap.get('examinationName');
    this.projectName=this.route.snapshot.queryParamMap.get('projectName');
    this.projectId=Number(this.route.snapshot.queryParamMap.get('projectId'));
    this.auditSessionId=this.route.snapshot.queryParamMap.get('saodoId');
    this.auditResultId=this.route.snapshot.queryParamMap.get('id');
    this.getAllCheckList();
    this.getAllProjectUser();
    this.getNote();

  }
  getAllCheckList(){
    this.projectChecklistService.GetCheckListItemByProject(this.projectId,this.auditSessionId).subscribe(data=>{
      this.listCheckList=data.result;

    })
    this.isEditing=false;
  }
  getAllProjectUser(){
    this.projectUserService.getAllProjectUserInProject(this.projectId).subscribe(data=>{
      this.projectUserList=data.result;

    })
  }
  public onChange(ngvipham, ngliendoi){
    this.projectUser.userId = ngvipham
    this.projectUser.curatorId = ngliendoi

  }
  submit(item:any){
    let requestBody = {
      checkListItemId: item.id,
      userId: this.projectUser.userId,
      curatorId: this.projectUser.curatorId,
      auditResultId:this.auditResultId
    }
    this.auditProjectResultService.create(requestBody).pipe(catchError(this.auditProjectResultService.handleError)).subscribe((res) => {
      this.projectUser={};

      abp.notify.success("Created successfully");
      this.isEditing=false;
      this.getAllCheckList();
    });
  }
  editPeople(item){
    item.createMode=true;
    this.isEditing=true;

  }

  save(id,form){
    let requestBody = {
      checkListItemId: id,
      userId: form.userId,
      curatorId: form.curatorId,
      id: form.id,
      auditResultId:this.auditResultId
    }
    delete form.createMode
    this.auditProjectResultService.update(requestBody).pipe(catchError(this.auditProjectResultService.handleError)).subscribe((res) => {
      abp.notify.success("Created successfully");
      this.getAllCheckList();
      this.isEditing=false;
    },()=>{form.createMode=true});
  }

  deletePeople(form:any){
    abp.message.confirm(
      "Delete Audit ",
      "",
      (result: boolean) => {
        if (result) {
          this.auditProjectResultService.delete(form.id).pipe(catchError(this.auditProjectResultService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Audit ");
            this.getAllCheckList();

          });
        }
      }
    );

  }
  getNote(){
    this.auditResultService.getById(this.auditResultId).subscribe(data=>{
      this.note=data.result;
    })
  }
  saveNote(){
    this.isEditingNote=true;
    this.auditResultService.updateNote(this.auditResultId,this.note).subscribe(data=>{
      // this.note=data.result;
      abp.notify.success("Update Successfully!");
    })
  }
  cancel(){
    this.getAllCheckList();
    this.isEditing=false;

  }
  cancelNote(){
    this.isEditingNote=false;
    this.getNote();
    this.getAllCheckList();
  }



}
