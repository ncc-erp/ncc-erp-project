
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ProjectProcessCriteriaAppService } from '@app/service/api/project-process-criteria.service'
import { GetAllPagingProjectProcessCriteriaDto, CreateProjectProcessCriteriaDto } from '@app/service/model/project-process-criteria.dto';

@Component({
  selector: 'app-select-project-tailoring',
  templateUrl: './select-project-tailoring.component.html',
  styleUrls: ['./select-project-tailoring.component.css']
})
export class SelectProjectTailoringComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<SelectProjectTailoringComponent>,
    private projectProcessCriteriaAppService: ProjectProcessCriteriaAppService,) {
    super(injector);
  }
  public selectedProjects: number[] = [];
  public tempPmReportProjectList: GetAllPagingProjectProcessCriteriaDto[] = [];
  public searchText = "";
  public projectList: GetAllPagingProjectProcessCriteriaDto[] = [];
  public allSelected = false;
  public createProjectProcessCriteriaDto: CreateProjectProcessCriteriaDto = new CreateProjectProcessCriteriaDto;
  ngOnInit(): void {
    this.getProjectHaveNotBeenTailor();
  }

  SaveAndClose() {
    this.projectProcessCriteriaAppService.addMultiCriteriaToMultiProject(this.createProjectProcessCriteriaDto).pipe(catchError(this.projectProcessCriteriaAppService.handleError)).subscribe((res) => {
      abp.notify.success("Add Project for Quality Audit SuccessFull!");
      this.dialogRef.close();
    }, () => { this.isLoading = false })
  }

  public search() {
    console.log(this.tempPmReportProjectList);

    this.projectList = this.tempPmReportProjectList.filter((item) => {
      return item.projectName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        item.projectCode.toLowerCase().includes(this.searchText.toLowerCase()) ||
        item.pmName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        item.clientName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        item.clientCode.toLowerCase().includes(this.searchText.toLowerCase());
    });
  }

  someChecked() {
    if (!this.projectList.length) {
      return false
    }
    return this.projectList.filter(e => e.selected).length > 0 && !this.allSelected
  }

  public selectAll(selected: boolean) {
    this.allSelected = selected;
    if (this.projectList == null) {
      return;
    }
    this.projectList.forEach(t => (t.selected = selected));
    this.selectedProjects = this.projectList.filter(x => x.selected).map(x => x.projectId);
    this.createProjectProcessCriteriaDto.projectIds = this.selectedProjects;
  }

  public onProjectSelect(project) {
    this.allSelected = this.projectList != null && this.projectList.every(t => t.selected);
    if (!project.selected) {
      this.selectedProjects = this.selectedProjects.filter(e => e != project.projectId)
    }
    else {
      this.selectedProjects.push(project.projectId)
    }
    this.createProjectProcessCriteriaDto.projectIds = this.selectedProjects;
  }

  public getProjectHaveNotBeenTailor() {
    this.projectProcessCriteriaAppService.getProjectHaveNotBeenTailor().subscribe(rs => {
      if (rs.success) {
        this.projectList = rs.result as GetAllPagingProjectProcessCriteriaDto[];
        this.tempPmReportProjectList = rs.result as GetAllPagingProjectProcessCriteriaDto[];
      }
    })
  }
  changeTextProjectType(projectType: string) {
    return projectType === 'TAM' ? 'T&M' : projectType
  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}
