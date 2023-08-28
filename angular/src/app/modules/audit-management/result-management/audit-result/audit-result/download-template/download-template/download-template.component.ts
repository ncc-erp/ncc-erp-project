import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ProjectProcessResultAppService } from '@app/service/api/project-process-result.service';
import * as FileSaver from 'file-saver';
@Component({
  selector: 'app-download-template',
  templateUrl: './download-template.component.html',
  styleUrls: ['./download-template.component.css']
})
export class DownloadTemplateComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<DownloadTemplateComponent>,
    public projectProcessResultAppService: ProjectProcessResultAppService) {
    super(injector);
  }
  public listProject;
  public projectId;
  public searchPRJ = "";
  ngOnInit(): void {
    this.getProjectToImportResult();
  }
  getProjectToImportResult() {
    this.projectProcessResultAppService.getProjectToImportResult().subscribe(res => {
      if (res.success) {
        this.listProject = res.result;
      }
    })
  }
  onSelectChange(event) {
    this.projectId = event.value;

  }
  focusOutProject() {
    this.searchPRJ = "";
  }
  download() {
    this.projectProcessResultAppService.exportProjectProcessResultTemplate(this.projectId).subscribe(res => {
      const file = new Blob([this.s2ab(atob(res.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, res.result.fileName);
      abp.notify.success("Export Template Successfully!");
      this.dialogRef.close();
    });
  }
  s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }
}
