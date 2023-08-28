import { Component, Inject, Injector, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PmReportRiskService } from '@app/service/api/pm-report-project-ricks.service';
import { AppComponentBase } from '@shared/app-component-base';
import { APP_ENUMS } from '@shared/AppEnums';
import { catchError } from 'rxjs/operators';
@Component({
  selector: 'app-add-risk-dialog',
  templateUrl: './add-risk-dialog.component.html',
  styleUrls: ['./add-risk-dialog.component.css']
})
export class AddRiskDialogComponent extends AppComponentBase implements OnInit {
  APP_ENUM = APP_ENUMS
  riskForm=new FormGroup({
    risk : new FormControl('',[Validators.required, this.emptyLinesValiadtor]),
    impact : new FormControl('',[Validators.required, this.emptyLinesValiadtor]),
    solution : new FormControl('',[Validators.required, this.emptyLinesValiadtor]),
  })

  id: number
  currentStatus = this.APP_ENUM.PMReportProjectRiskStatus.InProgress;
  currentPriority = this.APP_ENUM.Priority.Medium;
  public priority = [
    { value: this.APP_ENUM.Priority.Low, viewValue: 'Low' },
    { value: this.APP_ENUM.Priority.Medium, viewValue: 'Medium' },
    { value: this.APP_ENUM.Priority.High, viewValue: 'High' },
    { value: this.APP_ENUM.Priority.Critical, viewValue: 'Critical' }]


  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector: Injector,
    public pmReportRiskService: PmReportRiskService,
    public dialogRef: MatDialogRef<AddRiskDialogComponent>) {
    super(injector);

  }

  rickStatusList = [{ value: this.APP_ENUM.PMReportProjectRiskStatus.Done, view: 'Done' }, { value: this.APP_ENUM.PMReportProjectRiskStatus.InProgress, view: 'InProgress' }]
  ngOnInit(): void {
    if (this.data.command == 'edit') {
      this.riskForm.setValue({...this.riskForm.value,risk:this.data.risk.risk,solution:this.data.risk.solution,impact:this.data.risk.impact}),
      this.id = this.data.risk.id;
      this.currentStatus = this.data.risk.status;
      this.currentPriority = this.data.risk.priority
    }
  }
  get controls() {
    return this.riskForm.controls;
  }
  emptyLinesValiadtor(control:FormControl):{[s:string]:boolean} | null {
    let content = control.value.replaceAll(/&nbsp;/gm,'')
    .trim().split(" ").join("").replaceAll('<p></p>','').trim();
    if(content == "" || content == null) {
      return  {'emptyLines': true}
    }
    return null;
    }
  changeStatus(e) {
    this.currentStatus = e.value
  }
  changePriority(e) {
    this.currentPriority = e.value
  }
  SaveAndClose() {

    if (this.data.command == 'create') {
      const risk = {
        risk: this.riskForm.value.risk,
        solution: this.riskForm.value.solution,
        status: this.currentStatus,
        pmReportProjectId: this.data.pmReportProjectId,
        impact:this.riskForm.value.impact,
        priority: this.currentPriority
      }
      this.pmReportRiskService.createReportRisk(this.data.projectId, risk).pipe(catchError(this.pmReportRiskService.handleError)).subscribe(res => {
        abp.notify.success("Create Risk Successfully!")
        this.dialogRef.close(res);
      })
    }
    else {
      const risk = {
        risk: this.riskForm.value.risk,
        solution: this.riskForm.value.solution,
        status: this.currentStatus,
        pmReportProjectId: this.data.pmReportProjectId,
        impact: this.riskForm.value.impact,
        priority: this.currentPriority,
        id: this.id
      }
      this.pmReportRiskService.UpdateReportRisk(risk).pipe(catchError(this.pmReportRiskService.handleError)).subscribe((res) => {
        if (res) {
          abp.notify.success("Update Successfully!")
          this.dialogRef.close(res);
        }

      })
    }
  }
  // validateContent() :boolean{
  //   return this.convertToPlainText(this.risk.value)!="" && this.convertToPlainText(this.solution.value) !="" && this.convertToPlainText(this.impact.value) !=""
  // }
  convertToPlainText(html: string): string {
    const parser = new DOMParser();
    const doc = parser.parseFromString(html, 'text/html');
    const plainText = doc.body.textContent || '';
    return plainText.trim();
  }
}
