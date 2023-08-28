import { Component, Inject, Injector, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PmReportRiskService } from '@app/service/api/pm-report-project-ricks.service';
import { APP_ENUMS } from '@shared/AppEnums';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';
import { AddRiskDialogComponent } from '../add-risk-dialog/add-risk-dialog.component';
import * as moment from 'moment';
import { PmReportIssueService } from '@app/service/api/pm-report-issue.service';

@Component({
  selector: 'app-add-edit-issuse',
  templateUrl: './add-edit-issuse.component.html',
  styleUrls: ['./add-edit-issuse.component.css']
})
export class AddEditIssuseComponent extends AppComponentBase implements OnInit  {

  APP_ENUM = APP_ENUMS
  issueForm=new FormGroup({
    description: new FormControl('',[Validators.required, this.emptyLinesValiadtor]),
    impact : new FormControl('',),
    solution : new FormControl(''),
    meetingSolution : new FormControl(''),
  })

  id: number
  currentStatus = this.APP_ENUM.PMReportProjectIssueStatus.InProgress;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector: Injector,
    private reportIssueService: PmReportIssueService,
    public dialogRef: MatDialogRef<AddRiskDialogComponent>) {
    super(injector);

  }

  issueStatusList = [{ value: this.APP_ENUM.PMReportProjectIssueStatus.Done, view: 'Done' }, { value: this.APP_ENUM.PMReportProjectIssueStatus.InProgress, view: 'InProgress' }]
  ngOnInit(): void {
    if (this.data.command == 'edit') {
      this.issueForm.setValue({...this.issueForm.value,description:this.data.issue.description,solution:this.data.issue.solution,impact:this.data.issue.impact,meetingSolution:this.data.issue.meetingSolution}),
      this.id = this.data.issue.id;
      this.currentStatus = this.APP_ENUM.PMReportProjectIssueStatus[this.data.issue.status];
    }
  }
  get controls() {
    return this.issueForm.controls;
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

  SaveAndClose() {

    if (this.data.command == 'create') {
      const issue = {
        createdAt : moment().format("YYYY-MM-DD"),
        description: this.issueForm.value.description,
        solution: this.issueForm.value.solution,
        status: this.currentStatus,
        impact:this.issueForm.value.impact,
        meetingSolution:this.issueForm.value.meetingSolution

      }
      this.reportIssueService.createReportIssue(this.data.projectId, issue).pipe(catchError(this.reportIssueService.handleError)).subscribe(res => {
        abp.notify.success("Create Issue Successfully!")
        this.dialogRef.close(res);
      })
    }
    else {
      const issue = { ...this.data.issue,
        createdAt : moment().format("YYYY-MM-DD"),
        description: this.issueForm.value.description,
        solution: this.issueForm.value.solution,
        status: this.currentStatus,
        pmReportProjectId: this.data.pmReportProjectId,
        impact: this.issueForm.value.impact,
        meetingSolution:this.issueForm.value.meetingSolution,
        id: this.id
      }
      this.reportIssueService.update(issue).pipe(catchError(this.reportIssueService.handleError)).subscribe((res) => {
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
