import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { ProjectProcessResultAppService } from '@app/service/api/project-process-result.service';
import { MatDialog } from '@angular/material/dialog';
import { ImportAuditResultComponent } from './import-audit-result/import-audit-result/import-audit-result.component';
import { DownloadTemplateComponent } from './download-template/download-template/download-template.component'
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ViewQaNoteComponent } from './view-note/view-qa-note/view-qa-note.component';
import { GetClientFilterDto, GetPMFilterDto } from "@app/service/model/project-process-result.dto"

@Component({
  selector: 'app-audit-result',
  templateUrl: './audit-result.component.html',
  styleUrls: ['./audit-result.component.css']
})

export class AuditResultComponent extends PagedListingComponentBase<AuditResultComponent> implements OnInit {

  Audits_Results=PERMISSIONS_CONSTANT.Audits_Results
  Audits_Results_Delete=PERMISSIONS_CONSTANT. Audits_Results_Delete
  Audits_ResultsAudits_Results_DownLoad_Template=PERMISSIONS_CONSTANT.Audits_Results_DownLoad_Template
  Audits_Results_Import_Result=PERMISSIONS_CONSTANT.Audits_Results_Import_Result
  Audits_Results_Edit=PERMISSIONS_CONSTANT.Audits_Results_Edit
  Audits_Results_Detail=PERMISSIONS_CONSTANT.Audits_Results_Detail

  isShowAction = this.permission.isGranted(this.Audits_Results_Edit) ||
    this.permission.isGranted(this.Audits_Results_Edit);
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    let checkFilterPM = false;
    let checkFilterClient = false;
    let checkFilterStatus = false;
    request.filterItems.forEach(item => {
      if (item.propertyName == "pmId") {
        checkFilterPM = true;
        item.value = this.pmId;
      }
      if (item.propertyName == "clientId") {
        checkFilterClient = true;
        item.value = this.clientId;
      }
      if (item.propertyName == "status") {
        checkFilterStatus = true;
        item.value = this.status;
      }
    })
    if (!checkFilterPM) {
      request.filterItems = this.AddFilterItem(request, "pmId", this.pmId);
    }
    if (!checkFilterClient) {
      request.filterItems = this.AddFilterItem(request, "clientId", this.clientId);
    }
    if (!checkFilterStatus) {
      request.filterItems = this.AddFilterItem(request, "status", this.status);
    }
    if (this.pmId == -1) {
      request.filterItems = this.clearFilter(request, "pmId", "");
      checkFilterPM = true;
    }
    if (this.clientId == -1) {
      request.filterItems = this.clearFilter(request, "clientId", "");
      checkFilterClient = true;
    }
    if (this.status == -1) {
      request.filterItems = this.clearFilter(request, "status", "");
    }
    if (request.searchText) {
      this.pmId = -1;
      this.clientId = -1;
      this.status = -1;
    }
    this.projectProcessResultAppService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    })).subscribe((data) => {
      this.listResult = data.result.items;
      this.showPaging(data.result, pageNumber);
      if (!checkFilterPM) {
      request.filterItems = this.clearFilter(request, "pmId", "");
      }
      if (!checkFilterClient) {
        request.filterItems = this.clearFilter(request, "clientId", "");
      }
      if (!checkFilterStatus) {
        request.filterItems = this.clearFilter(request, "status", "");
      }
    }, () => { })
  }
  public searchProject(){
    if (this.searchText != ""){
      this.pmId = -1;
      this.clientId = -1;
    }
    this.getDataPage(1);
  }
  protected delete(entity: AuditResultComponent): void {
    throw new Error('Method not implemented.');
  }

  constructor(public injector: Injector,
    private projectProcessResultAppService: ProjectProcessResultAppService,
    private dialog: MatDialog) {
    super(injector);
  }
  listShowResult = [];
  public pmList: GetPMFilterDto[] = [];
  public clientList: GetClientFilterDto[] = [];
  public searchClient = "";
  public searchPM = "";
  public pmId = -1;
  public clientId = -1;
  public status = -1;
  public auditStatusList: string[] = Object.keys(this.APP_ENUM.AuditStatus);
  showDetail(audit: any, item: any) {
    if (this.permission.isGranted(this.Audits_Results_Detail)) {
      this.router.navigate(['app/auditResultDetail'], {
        queryParams: {
          projectId: item.projectId,
          projectProcessResultId: audit.id,
          projectCode: item.projectCode,
          projectName: item.projectName,
          auditDate: audit.auditDate,
          pmName: audit.pmName
        }
      })
    }

  }

  public listResult;
  ngOnInit(): void {
    this.refresh();
    this.getPM();
    this.getClient();
  }
  getPM() {
    this.projectProcessResultAppService.getPMProcessResultInfors().subscribe(rs => {
      this.pmList = rs.result;
    })
  }
  getClient() {
    this.projectProcessResultAppService.getClientProcessResultInfors().subscribe(rs => {
      this.clientList = rs.result
    });
  }
  openImport() {
    const show = this.dialog.open(ImportAuditResultComponent, {
      panelClass: 'my-dialog',
      width: "60%",
    })
    show.afterClosed().subscribe(re => {
      this.refresh();
    })
  }
  openDownloadTemplate() {
    const show = this.dialog.open(DownloadTemplateComponent, {
      panelClass: 'my-dialog',
      width: "60%",
    })
    show.afterClosed().subscribe(re => {
      this.refresh();
    })
  }
  deleteResult(id) {
    abp.message.confirm("Are you sure to delete this result?", "", (result: boolean) => {
      if (result) {
        this.projectProcessResultAppService.deleteResult(id).subscribe((rs) => {
          if (rs.success) {
            abp.message.success("Delete Result Successful!", "Delete Audit result", true);
            this.isLoading = false;
            this.refresh();
          }
        })
      }
    })
  }
  viewNote(item) {
    const show = this.dialog.open(ViewQaNoteComponent, {
      panelClass: 'my-dialog',
      width: "60%",
      data: {
        item: item,
        action: "view"
      }
    })
    show.afterClosed().subscribe(re => {
      this.isLoading = false;
      this.refresh();
    });
  }
  EditNote(item) {
    const show = this.dialog.open(ViewQaNoteComponent, {
      panelClass: 'my-dialog',
      width: "60%",
      data: {
        item: item,
        action: "edit"
      }
    })
    show.afterClosed().subscribe(re => {
      this.isLoading = false;
      this.refresh();
    });
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
  changeShowResult(projectId){
    if(this.listShowResult.some(res=> res == projectId)){
      this.listShowResult=this.listShowResult.filter(res=> {
        return res !== projectId
      })
    }else{
      this.listShowResult=[...this.listShowResult,projectId]
    }

  }
  checkShowResult(projectId){
    return this.listShowResult.some(res=>res==projectId)
  }
}
