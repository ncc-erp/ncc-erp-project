
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ProjectProcessCriteriaAppService } from '@app/service/api/project-process-criteria.service'
import { GetAllPagingProjectProcessCriteriaDto, CreateProjectProcessCriteriaDto } from '@app/service/model/project-process-criteria.dto';
@Component({
  selector: 'app-import-tailoring',
  templateUrl: './import-tailoring.component.html',
  styleUrls: ['./import-tailoring.component.css']
})
export class ImportTailoringComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<ImportTailoringComponent>,
    private projectProcessCriteriaAppService: ProjectProcessCriteriaAppService,) {
    super(injector);
  }
  public listProject: GetAllPagingProjectProcessCriteriaDto[] = [];;
  public searchPRJ = "";
  public file: File;
  public fileName = "";
  public projectId;
  ngOnInit(): void {
    this.getProjectHaveNotBeenTailor();
  }

  getProjectHaveNotBeenTailor() {
    this.projectProcessCriteriaAppService.getProjectHaveNotBeenTailor().subscribe(rs => {
      if (rs.success) {
        this.listProject = rs.result;
      }
    })
  }

  focusOutProject() {
    this.searchPRJ = "";
  }

  public selectFile(event) {
    let file = event.target.files[0];
    this.file = file;
    this.fileName = this.file.name;
  }

  SaveAndClose() {
    this.projectProcessCriteriaAppService.importProjectProcessCriteriaFromExcel(this.file, this.projectId).
      pipe(catchError(this.projectProcessCriteriaAppService.handleError)).subscribe((res) => {
        if (res.success) {
          if (res.result.length < 1) {
            abp.message.success(`Import Tailoring Successfully `, "Import Tailoring From Excel", true);
          }
          else {
            let message = "";
            res.result.forEach(element => {
              message += `<span class="text-left">Row: ${element.row} Error: ${element.reasonFail}</span><br>`;
            });
            message = `<div style="display: flex; flex-direction: column; align-items: stretch; overflow-y: auto; max-height: 500px;">${message}</div>`
            abp.message.warn(message, `Import Tailoring Successfully but`, true);
          }
        }
      }, () => { this.isLoading = false })
      this.dialogRef.close();
  }

  onSelectChange(event) {
    this.projectId = event.value;

  }
}
