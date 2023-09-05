import { ActivatedRoute } from '@angular/router';
import { ProjectProcessCriteriaAppService } from '@app/service/api/project-process-criteria.service';
import { NestedTreeControl } from '@angular/cdk/tree';
import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { AuditCriteriaProcessService } from '@app/service/api/audit-criteria-process.service';
import { ProcessCriteria } from '@app/service/model/process-criteria-audit.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { CriteriaManagementComponent } from '../criteria-management.component';
import { PopupNoteComponent } from '../tailoring-management/popup-note/popup-note.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import * as FileSaver from 'file-saver';
import { ProjectProcessResultAppService } from '@app/service/api/project-process-result.service';

@Component({
  selector: 'app-taloring-project',
  templateUrl: './taloring-project.component.html',
  styleUrls: ['./taloring-project.component.css']
})
export class TaloringProjectComponent extends PagedListingComponentBase<TaloringProjectComponent> implements OnInit {

  isApplicable = "option2";
  status = "option2";
  projectId:number;
  projectName='';
  projectCode='';
  searchText='';

  Audits_Tailoring_Detail= PERMISSIONS_CONSTANT.Audits_Tailoring_Detail
  Audits_Tailoring_Detail_ViewNote= PERMISSIONS_CONSTANT.Audits_Tailoring_Detail_ViewNote
  Audits_Tailoring_Detail_Update= PERMISSIONS_CONSTANT. Audits_Tailoring_Detail_Update
  Audits_Tailoring_Detail_Detele= PERMISSIONS_CONSTANT. Audits_Tailoring_Detail_Detele
  Audits_Tailoring_Update_Project=PERMISSIONS_CONSTANT.Audits_Tailoring_Update_Project
  Audits_Tailoring_DownLoadTailoringTemplate = PERMISSIONS_CONSTANT.Audits_Tailoring_DownLoadTailoringTemplate

  updateTaloring(){
    if(this.permission.isGranted(this.Audits_Tailoring_Update_Project)){
      this.router.navigate(['/app/tailoring-project-edit'], {
        queryParams: {
          projectId:this.projectId,
          projectCode: this.projectCode,
          projectName:this.projectName
        }
      })
    }
}
  treeControl = new NestedTreeControl<PeriodicElement>((node) => node.children);
  dataSource = new MatTreeNestedDataSource<PeriodicElement>();
  hasChild = (_: number, node: PeriodicElement) =>
    !!node.children && node.children.length > 0;

  constructor(
    public injector: Injector,
    private dialog: MatDialog,
    private processProjectService: ProjectProcessCriteriaAppService,
    private projectProcessResultAppService:ProjectProcessResultAppService,
    route:ActivatedRoute,
    ) {
    super(injector);
    this.projectId = Number(route.snapshot.queryParamMap.get("projectId"));
    this.projectName=route.snapshot.queryParamMap.get('projectName')
    this.projectCode=route.snapshot.queryParamMap.get('projectCode')
  }
  public selectedApplicable = -1;
  public applicableList = [
    {displayName: "Standard", value: 1},
    {displayName: "Modify", value: 2},
  ];
  ngOnInit(): void {
    this.getAll();
  }

  getAll() {
    this.processProjectService.getDetail(this.projectId)
    .pipe(catchError(this.processProjectService.handleError))
    .subscribe(data => {
      this.dataSource.data = data.result.childrens,
      this.treeControl.dataNodes=data.result.childrens
      this.treeControl.expandAll()
    })
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
  }

  protected delete(entity: TaloringProjectComponent): void {
    throw new Error('Method not implemented.');
  }

    searchDetail(){
      this.processProjectService.searchDetail(this.projectId,this.searchText,this.selectedApplicable).pipe(catchError(this.processProjectService.handleError)).subscribe(
        data=> {
          this.dataSource.data=data.result.childrens,
          this.treeControl.dataNodes=data.result.childrens
          this.treeControl.expandAll()
        })
    }

  viewNote(node: PeriodicElement){

    const data = {
      command: 'view',
      node: node.item
    }
    const dialogRef = this.dialog.open(PopupNoteComponent, {
      panelClass: 'my-dialog',
      width:"50%",
      data: data,
    });
    dialogRef.afterClosed().subscribe(result => {
    });
  }
  editNote(node:PeriodicElement) {
    const data = {
      command: 'edit',
      node: node.item
    }
    const dialogRef = this.dialog.open(PopupNoteComponent, {
      panelClass: 'my-dialog',
      width:"50%",
      data: data,
    });
    dialogRef.afterClosed().subscribe(result => {

    });
  }
  deleteCriteria(node) {
    this.processProjectService.deleteProjectProcessCriteria(node.item.projectProcessCriteriaId)
      .pipe(catchError(this.processProjectService.handleError))
      .subscribe(() => {
        abp.notify.success("Delete criteria successful!");
        this.getAll();
    })
  }

  s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }

  downloadResultTemplate() {
    this.projectProcessResultAppService.exportProjectProcessResultTemplate(this.projectId).subscribe(res => {
      const file = new Blob([this.s2ab(atob(res.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, res.result.fileName);
      abp.notify.success("Export Template Successfully!");
      this.dialogRef.close();
    });
  }
}
export interface PeriodicElement {
  item: ProcessCriteria
  children?: PeriodicElement[];
}
