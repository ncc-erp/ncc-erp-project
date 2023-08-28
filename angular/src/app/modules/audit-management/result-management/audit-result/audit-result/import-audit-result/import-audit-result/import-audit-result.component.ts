import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ImportFileDto, ImportProcessCriteriaResultDto } from "@app/service/model/project-process-result.dto";
import * as moment from 'moment';
import { ProjectProcessResultAppService } from "@app/service/api/project-process-result.service";

@Component({
  selector: 'app-import-audit-result',
  templateUrl: './import-audit-result.component.html',
  styleUrls: ['./import-audit-result.component.css']
})
export class ImportAuditResultComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<ImportAuditResultComponent>,
    public projectProcessResultAppService: ProjectProcessResultAppService) {
    super(injector);
  }
  public AuditResultList: string[] = Object.keys(this.APP_ENUM.AuditStatus);
  public import: ImportFileDto = new ImportFileDto;
  public searchPRJ = "";
  public fileName = "";
  public listProject;
  public startDate: Date;
  ngOnInit(): void {
    this.startDate = new Date();
    this.onChangeDate();
    this.getProjectToImportResult();
  }
  getProjectToImportResult() {
    this.projectProcessResultAppService.getProjectToImportResult().subscribe(res => {
      if (res.success) {
        this.listProject = res.result;
      }
    })
  }
  public selectFile(event) {
    let file = event.target.files[0];
    this.import.file = file;
    this.fileName = file.name;
    const regex = /\[(.*?)\]/;
    const match = regex.exec(file.name);
    const item = this.listProject.find(x => x.projectCode == match[1]);
    if (item) {
      this.import.projectId = item.projectId;
    }
  }
  onSelectChange(event) {
    this.import.projectId = event.value;

  }
  focusOutProject() {
    this.searchPRJ = "";
  }
  SaveAndClose(projectId) {
    const item = this.listProject.find(x => x.projectId == projectId);
    if (!this.import.note) { this.import.note = "" }
    this.projectProcessResultAppService.importToProjectProcessResult(this.import).
      pipe(catchError(this.projectProcessResultAppService.handleError)).subscribe((res) => {
        if (res.success) {
          const infor = res.result as ImportProcessCriteriaResultDto;
          if (!infor.failedList) {
            let message = `<div style="display: flex; flex-direction: column; align-items: stretch;">
          <span>Total Score: ${infor.auditInfo.score}</span><br>
          <span>Status: <span class="${this.APP_CONST.auditStatus[infor.auditInfo.status]}"> ${this.AuditResultList[infor.auditInfo.status - 1]}
          </span> </span><br>
          </div>`
            abp.message.success(message, `Import audit result for project [${item.projectCode}] ${item.projectName} successfully!`, true);
          }
          else {
            let message = "";
            infor.failedList.forEach(element => {
              message += `<span class="text-left">Row: ${element.row} Error: ${element.reasonFail}</span><br>`;
            });
            message = `<div style="display: flex; flex-direction: column; align-items: stretch; overflow-y: auto; max-height: 500px;">${message}</div>`
            abp.message.error(message, `Import audit result for project [${item.projectCode}] ${item.projectCode} NOT successful!`, true);
          }
        }
      });
      this.dialogRef.close();
  }
  onChangeDate() {
    this.import.auditDate = moment(this.startDate.toISOString()).format('MM-DD-YYYY');
  }
}
