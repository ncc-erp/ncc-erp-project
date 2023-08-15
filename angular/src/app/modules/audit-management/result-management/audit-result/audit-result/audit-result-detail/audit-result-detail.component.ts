import { state } from '@angular/animations';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, Injector } from '@angular/core';
import { GetAllProjectProcessCriteriaDto } from '@app/service/model/project-process-criteria.dto'
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { NestedTreeControl } from "@angular/cdk/tree";
import { MatTreeNestedDataSource } from "@angular/material/tree";
import { ProjectProcessCriteriaResultAppService } from "@app/service/api/project-process-criteria-result.service";
import { GetProjectProcessCriteriaResultDto, GetProcessCriteriaDto, NCSStatus } from "@app/service/model/project-process-criteria-result.dto";
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ThemePalette } from '@angular/material/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ViewAuditResultDetailComponent } from './view-audit-result-detail/view-audit-result-detail.component';
import { EditAuditResultDetailComponent } from './edit-audit-result-detail/edit-audit-result-detail.component';


@Component({
  selector: 'app-audit-result-detail',
  templateUrl: './audit-result-detail.component.html',
  styleUrls: ['./audit-result-detail.component.css']
})
export class AuditResultDetailComponent extends PagedListingComponentBase<AuditResultDetailComponent> implements OnInit {

  styleChip: string = ' color:black; width:150px; justify-content: center;';
  search = '';
  selectedIsApplicable: string = '';
  selectedNCStatus: string = '';

  Audits_Results_Detail=PERMISSIONS_CONSTANT.Audits_Results_Detail
  Audits_Results_Detail_View = PERMISSIONS_CONSTANT.Audits_Results_Detail_View
  Audits_Results_Detail_ViewNote=PERMISSIONS_CONSTANT.Audits_Results_Detail_ViewNote

  availableColors: ChipColor[] = [
    { name: 'Not Selected', color: null, style: 'background-color: #ccc;' + this.styleChip },
    { name: 'Non-Compliance', color: null, style: 'background-color: #EA9999;' + this.styleChip },
    { name: 'Observation', color: null, style: 'background-color: #FFF2CC;' + this.styleChip },
    { name: 'Recommendation', color: null, style: 'background-color: #CFE2F3;' + this.styleChip },
    { name: 'Excellent', color: null, style: 'background-color: #B6D7A8;' + this.styleChip }
  ];

  chipIndex(chip: ChipColor): number {
    return this.availableColors.findIndex(c => c.name === chip.name);
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.projectProcesCriteriaResultAppService.getTreeListPPCR(this.projectProcessResultId, this.projectId)
      .pipe(finalize(() => {
        finishedCallback();
      })).subscribe((data) => {
        this.dataSource.data = data.result.childrens
        this.treeControl.dataNodes=data.result.childrens
        this.treeControl.expandAll()
        this.showPaging(data.result, pageNumber);
        this.totalScore = data.result.totalScore;
        this.statusFinal = data.result.status;
      }, () => { });
  }

  protected delete(entity: AuditResultDetailComponent): void {
    throw new Error('Method not implemented.');
  }

  public projectProcessResultId: any;
  public projectId: any;
  projectCode: string;
  projectName: string;
  auditDate: string;
  pmName: string;
  totalScore: number;
  statusFinal: number;
  public listTailoring: GetAllProjectProcessCriteriaDto[];

  constructor(public injector: Injector,
    private projectProcesCriteriaResultAppService: ProjectProcessCriteriaResultAppService,
    public dialog: MatDialog,
    private route: ActivatedRoute
  ) {
    super(injector);
  }

  public auditStatusList: string[] = Object.keys(this.APP_ENUM.AuditStatus);
  ngOnInit(): void {
    this.projectProcessResultId = this.route.snapshot.queryParamMap.get('projectProcessResultId');
    this.projectId = this.route.snapshot.queryParamMap.get('projectId');
    this.projectCode = this.route.snapshot.queryParamMap.get('projectCode');
    this.projectName = this.route.snapshot.queryParamMap.get('projectName');
    this.auditDate = this.route.snapshot.queryParamMap.get('auditDate');
    this.pmName = this.route.snapshot.queryParamMap.get('pmName');
    this.refresh();
  }

  private readonly NC_STATUS = {
    ALL: '',
    EXCELLENT: '4',
    RECOMMENDATION: '3',
    OBSERVATION: '2',
    NC: '1',
  };

  private readonly APPLICABLE_STATUS = {
    ALL: '',
    TRUE: 'true',
    FALSE: 'false',
  };

  searchPPCR() {
    const { projectProcessResultId, projectId, search, status } = this;
    this.projectProcesCriteriaResultAppService.search(projectProcessResultId, projectId, search, status)
      .pipe(catchError(this.projectProcesCriteriaResultAppService.handleError))
      .subscribe(({ result: { childrens } }) => {
        this.dataSource.data = childrens;
        this.treeControl.dataNodes= childrens
        this.treeControl.expandAll()
      });
  }

  get status() {
    return this.NC_STATUS[this.selectedNCStatus.toUpperCase()];
  }

  viewPPCRnode(node, contentType?: string) {
    const dialogConfig: MatDialogConfig<any> = {
      panelClass: 'my-dialog',
      width: "60%",
      data: {
        node: node,
        contentType: contentType
      }
    };

    const dialogRef = this.dialog.open(ViewAuditResultDetailComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      //console.log(`Dialog result: ${result}`);
    });
  }



  editProjectProcessCriteria(node) {
    const dialogRef = this.dialog.open(EditAuditResultDetailComponent, {
      panelClass: 'my-dialog',
      data: node
    });

    dialogRef.afterClosed().subscribe(result => {
      //console.log(`Dialog result: ${result}`);
    });
  }

  treeControl = new NestedTreeControl<PeriodicElement>((node) => node.children);
  dataSource = new MatTreeNestedDataSource<PeriodicElement>();
  hasChild = (_: number, node: PeriodicElement) =>
    !!node.children && node.children.length > 0;

}
export interface PeriodicElement {
  item: GetProjectProcessCriteriaResultDto
  children?: PeriodicElement[];
}

export interface ChipColor {
  name: string;
  color: ThemePalette;
  style: string;
}
