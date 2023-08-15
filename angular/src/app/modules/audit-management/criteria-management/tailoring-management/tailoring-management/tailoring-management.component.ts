import { Router } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';
import { ProjectProcessCriteriaAppService } from '@app/service/api/project-process-criteria.service'
import { GetAllProjectProcessCriteriaDto } from '@app/service/model/project-process-criteria.dto'
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { SelectProjectTailoringComponent } from './select-project-tailoring/select-project-tailoring/select-project-tailoring.component';
import { ImportTailoringComponent } from './import-tailoring/import-tailoring/import-tailoring.component';
import * as FileSaver from 'file-saver';
import { ProjectProcessResultAppService } from '@app/service/api/project-process-result.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';

@Component({
  selector: 'app-tailoring-management',
  templateUrl: './tailoring-management.component.html',
  styleUrls: ['./tailoring-management.component.css']
})
export class TailoringManagementComponent extends PagedListingComponentBase<TailoringManagementComponent> implements OnInit {


 public Audits_Tailoring = PERMISSIONS_CONSTANT.Audits_Tailoring
 public Audits_Tailoring_DownLoadTemplate = PERMISSIONS_CONSTANT.Audits_Tailoring_DownLoadTemplate
 public Audits_Tailoring_Import = PERMISSIONS_CONSTANT.Audits_Tailoring_Import
 public Audits_Tailoring_Create = PERMISSIONS_CONSTANT.Audits_Tailoring_Create
 public Audits_Tailoring_Delete = PERMISSIONS_CONSTANT.Audits_Tailoring_Delete
 public Audits_Tailoring_DownLoadTailoringTemplate = PERMISSIONS_CONSTANT.Audits_Tailoring_DownLoadTailoringTemplate
 public Audits_Tailoring_Detail= PERMISSIONS_CONSTANT.Audits_Tailoring_Detail
 public Audits_Tailoring_Update_Project=PERMISSIONS_CONSTANT.Audits_Tailoring_Update_Project

  public isShowAction = this.permission.isGranted(this.Audits_Tailoring_DownLoadTailoringTemplate) ||
    this.permission.isGranted(this.Audits_Tailoring_Delete) ||
    this.permission.isGranted(this.Audits_Tailoring_Update_Project);
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.isLoading = true;
    this.projectProcessCriteriaAppService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    })).subscribe((data) => {
      if (data.success) {
        this.listTailoring = data.result.items;
        this.showPaging(data.result, pageNumber);
        this.isLoading=false
      }

    }, () => { })
  }
  protected delete(entity: TailoringManagementComponent): void {
    throw new Error('Method not implemented.');
  }

  constructor(public injector: Injector,
    router: Router,
    private projectProcessCriteriaAppService: ProjectProcessCriteriaAppService,
    private projectProcessResultAppService:ProjectProcessResultAppService,
    private dialog: MatDialog
  ) {
    super(injector);
  }
  public listTailoring: GetAllProjectProcessCriteriaDto[];
  ngOnInit(): void {
    this.refresh();
  }
  openCreateDialog() {
    const show = this.dialog.open(SelectProjectTailoringComponent, {
      panelClass: 'my-dialog',
      width: "60%",
    })
    show.afterClosed().subscribe(re => {
      this.refresh();
    })
  }
  openImportDialog() {
    const show = this.dialog.open(ImportTailoringComponent, {
      panelClass: 'my-dialog',
      width: "40%",
    })
    show.afterClosed().subscribe(re => {
      this.refresh();
    })
  }
  deleteProject(tailoring) {
    abp.message.confirm(`Are you sure to remove <strong style="color:#2991BF">[${tailoring.projectCode}] ${tailoring.projectName}</strong> from Tailoring?`, "", (result: boolean)=>{
      if (result) {
        this.projectProcessCriteriaAppService.deleteProject(tailoring.projectId).
      pipe(catchError(this.projectProcessCriteriaAppService.handleError)).subscribe((res) => {
        if (res.success) {
          abp.message.success(`Delete <strong style="color:#2991BF">[${tailoring.projectCode}] ${tailoring.projectName}</strong> Tailoring sucessfully!`, "Delete Tailoring", true);
          this.refresh();
        }
      });
      }
    },{isHtml:true})
  }

  navigateTaloringProject(tailoring) {
    if(this.permission.isGranted(this.Audits_Tailoring_Detail)){
      this.router.navigate(['/app/tailoring-project'], {
        queryParams: {
          projectId: tailoring.projectId,
          projectCode: tailoring.projectCode,
          projectName: tailoring.projectName
        }
      })
    }
  }

  updateTaloring(tailoring) {
    if(this.permission.isGranted(this.Audits_Tailoring_Update_Project)){
      this.router.navigate(['/app/tailoring-project-edit'], {
        queryParams: {
          projectId: tailoring.projectId,
          projectCode: tailoring.projectCode,
          projectName: tailoring.projectName
        }
      })
    }

  }
  DownloadTemplate() {
    this.projectProcessCriteriaAppService.downloadTemplate().subscribe(res => {
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
  downloadResultTemplate(projectId) {
    this.projectProcessResultAppService.exportProjectProcessResultTemplate(projectId).subscribe(res => {
      const file = new Blob([this.s2ab(atob(res.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, res.result.fileName);
      abp.notify.success("Export Template Successfully!");
      this.dialogRef.close();
    });
  }
}
