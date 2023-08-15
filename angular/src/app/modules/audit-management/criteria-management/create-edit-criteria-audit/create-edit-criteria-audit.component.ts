
import { AppComponentBase } from "@shared/app-component-base";
import { Component, Inject, Injector, OnInit, Renderer2 } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ProcessCriteria } from "@app/service/model/process-criteria-audit.dto";
import { AuditCriteriaProcessService } from "@app/service/api/audit-criteria-process.service";
import { catchError } from "rxjs/operators";
import { DomSanitizer } from "@angular/platform-browser";
import { FormControl } from "@angular/forms";
import { PERMISSIONS_CONSTANT } from "@app/constant/permission.constant";
import Swal from "sweetalert2";

@Component({
  selector: "app-create-edit-criteria-audit",
  templateUrl: "./create-edit-criteria-audit.component.html",
  styleUrls: ["./create-edit-criteria-audit.component.css"],
})
export class CreateEditCriteriaAuditComponent
  extends AppComponentBase
  implements OnInit
{
  tinyMCE1 = new FormControl('');
  tinyMCE2 = new FormControl('');
  criteriaAudit = {} as ProcessCriteria ;
  listCriteriaAudit = []
  listCriteriaAuditFilter = []
  searchCriteria:string = "";
  parentCurrent:string='';
  nameParent:string=''
  isCreateCriteria:boolean = true;
  isCheckedCreateAnother:boolean=false;
  codeParent:string='';
  codeChild:number;
  code=[];
  maxCode: number
  oldCode = '';
  oldName = '';
  oldGuideline = '';
  oldExample= ''
  


  public Audits_Criteria_ChangeApplicable = PERMISSIONS_CONSTANT.Audits_Criteria_ChangeApplicable

  constructor(private renderer: Renderer2,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<CreateEditCriteriaAuditComponent>,
    private processCriteriaService: AuditCriteriaProcessService,
    private sanitizer: DomSanitizer,
    injector: Injector
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (this.data.command == "edit") {
      this.oldCode = this.data.item.code
      this.oldCode = this.data.item.code
      this.oldName=this.data.item.name
      this.oldGuideline=this.data.item.guidLine
      this.oldExample=this.data.item.qaExample
      this.isCreateCriteria = false;
      if (this.data.item.parentId) {
        this.processCriteriaService.getCriteriaById(this.data.item.parentId).pipe(catchError(this.processCriteriaService.handleError)).subscribe(res => {
          this.parentCurrent = res.result.code
          this.nameParent = res.result.code + ' ' + res.result.name
        })
      }
      else {
        this.parentCurrent = null;
      }
      this.criteriaAudit = this.data.item;
      this.tinyMCE1.setValue(this.data.item.guidLine)
      this.tinyMCE2.setValue(this.data.item.qaExample)
      this.code = this.data.item.code.split('.')
      this.codeChild = Number(this.code.splice(-1, 1))
      this.codeParent = this.code.join('.')
    } else {
      this.isCreateCriteria = true;
      this.parentCurrent = this.data.item.code;
      this.codeParent = this.data.item.code || '';
      
      this.criteriaAudit = { ...this.criteriaAudit, isApplicable: true, parentId: this.data.item.id, name: this.data.name }
    }
    
    this.processCriteriaService.getForDropDown().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data => {
      this.listCriteriaAudit = data.result
      this.listCriteriaAuditFilter = data.result.filter(x => x.isActive == true);
      this.maxCode = this.listCriteriaAudit.filter(res => res.level == 1).map(res => { return Number(res.code) }).sort(function (a, b) { return a - b }).pop()

      if (this.isCreateCriteria && this.parentCurrent) {
        this.codeChild = data.result.find(res => res.id == this.data.item.id).maxValueOfListCode + 1
      }
      if (this.isCreateCriteria && !this.parentCurrent) {
        if (this.data.childrens.length > 0) {
          this.codeChild = this.maxCode + 1
        }
        else {
          this.codeChild = 1
        }
      }
    })
  }

  ngAfterViewInit() {
    if(this.parentCurrent){
    setTimeout(() => {
      var elem = this.renderer.selectRootElement('#criteriaName');
      elem.focus();
    }, 500);
  }
  }
  changeParent(e) {
    this.parentCurrent= e.value
    this.codeParent= e.value
    this.criteriaAudit.parentId=this.listCriteriaAudit.find(res=>res.code==e.value).id
    this.codeChild=this.listCriteriaAudit.find(res=>res.code==e.value).maxValueOfListCode + 1
    setTimeout(() => {
      var elem = this.renderer.selectRootElement('#criteriaName');
      elem.focus();
    }, 500);
  }

  setCheckedCreateAnother(){
  this.isCheckedCreateAnother=!this.isCheckedCreateAnother
  }

  removeParent(){
    this.parentCurrent=null;
    this.criteriaAudit.parentId=null;
    this.codeParent = '';
    this.searchCriteria = '';
    if(this.parentCurrent){
      this.codeChild = undefined;
    }
    else{
      this.codeChild = this.maxCode + 1
    }
  }

  SaveAndClose() {
    const criteriaCreate = {
      code: this.codeParent ? (this.codeParent + '.' + this.codeChild) : this.codeChild,
      name: this.criteriaAudit.name,
      isApplicable: this.criteriaAudit.isApplicable,
      guidLine: this.tinyMCE1.value,
      qaExample: this.tinyMCE2.value,
      id: this.criteriaAudit.id,
      parentId: this.criteriaAudit.parentId
    }
    const criteriaUpdate = {
      code: this.codeParent ? (this.codeParent + '.' + this.codeChild) : this.codeChild,
      isApplicable: this.criteriaAudit.isApplicable,
      guidLine: this.tinyMCE1.value,
      qaExample: this.tinyMCE2.value,
      id: this.criteriaAudit.id,
      name: this.criteriaAudit.name
    }





    if (this.data.command == "create") {
      this.processCriteriaService.create(criteriaCreate).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
        abp.notify.success("Create Successfully!");
        if (!this.isCheckedCreateAnother) {
          this.dialogRef.close(this.criteriaAudit)
        }
        else {
          this.processCriteriaService.getForDropDown().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data => {
            this.listCriteriaAudit = data.result;
            this.listCriteriaAuditFilter = data.result.filter(x => x.isActive == true);
            this.maxCode = this.listCriteriaAudit.filter(res => res.level == 1).map(res => { return Number(res.code) }).sort(function (a, b) { return a - b }).pop()
            if (this.parentCurrent) {
              this.codeChild = data.result.find(res => res.code == this.parentCurrent).maxValueOfListCode + 1
            }
            else {
              this.codeChild = this.maxCode + 1
            }
          })

          this.criteriaAudit.name = '';
          this.criteriaAudit.isApplicable=true;
          this.tinyMCE1.setValue('')
          this.tinyMCE2.setValue('')
        }
      }, () => { this.isLoading = false })
    } else {

      const listAtrCriteriaChange = []
      if(criteriaCreate.name != this.oldName){
        listAtrCriteriaChange.push('Name')
      }
      if(criteriaCreate.guidLine!= this.oldGuideline){
        listAtrCriteriaChange.push('Guideline')
      }
      if(criteriaCreate.qaExample!= this.oldExample){
        listAtrCriteriaChange.push('QaExample')
      }

      if (criteriaUpdate.code != this.oldCode) {
        abp.message.confirm(" Code of all descendants will be updated accordingly ", "", (result: boolean) => {
          if (result) {
            if(listAtrCriteriaChange.length>0){
              const message = listAtrCriteriaChange.join(', ') + ', Code' + ' Update Successfully!'
              this.processCriteriaService.update(criteriaUpdate).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
                abp.notify.success(message);
                this.dialogRef.close(this.criteriaAudit);
              }, () => { this.isLoading = false })
            }
            else{
              this.processCriteriaService.update(criteriaUpdate).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
                abp.notify.success('Code Update Successfully!');
                this.dialogRef.close(this.criteriaAudit);
              }, () => { this.isLoading = false })
          }
        }
          else {
            criteriaUpdate.code = this.oldCode;
            if(listAtrCriteriaChange.length>0){
              const message = listAtrCriteriaChange.join(', ') +  ' Update Successfully!'
              this.processCriteriaService.update(criteriaUpdate).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
                abp.notify.success(message);
                this.dialogRef.close(this.criteriaAudit);
              }, () => { this.isLoading = false })
            }
            else{
              this.dialogRef.close(this.criteriaAudit);
            }
          }
        },{ confirmButtonText: abp.localization.abpWeb("Update")})
      }
      else {
        if(listAtrCriteriaChange.length>0){
          const message = listAtrCriteriaChange.join(', ') +  ' Update Successfully!'
          this.processCriteriaService.update(criteriaUpdate).pipe(catchError(this.processCriteriaService.handleError)).subscribe((res) => {
            abp.notify.success(message);
            this.dialogRef.close(this.criteriaAudit);
          }, () => { this.isLoading = false })
        }
        else{
          this.dialogRef.close(this.criteriaAudit);
        }
      }
    }
  }
  checkCode(e) {
    if (e!=null && e < 1) {
      this.codeChild=1
    }
  }
}

