import { CreateEditCriteriaAuditComponent } from "./create-edit-criteria-audit/create-edit-criteria-audit.component";
import { Component, Inject, Injector, OnInit } from "@angular/core";
import { catchError, finalize } from "rxjs/operators";
import { PagedListingComponentBase, PagedRequestDto } from "@shared/paged-listing-component-base";
import { NestedTreeControl } from "@angular/cdk/tree";
import { MatTreeNestedDataSource } from "@angular/material/tree";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ProcessCriteria } from "@app/service/model/process-criteria-audit.dto";
import { AuditCriteriaProcessService } from "@app/service/api/audit-criteria-process.service";
import { ViewGuilineComponent } from "./view-guiline/view-guiline.component";
import { forkJoin } from 'rxjs';
import { escape } from "lodash";
import { PERMISSIONS_CONSTANT } from "@app/constant/permission.constant";
import { result } from 'lodash-es';

@Component({
  selector: "app-criteria-management",
  templateUrl: "./criteria-management.component.html",
  styleUrls: ["./criteria-management.component.css"],
})
export class CriteriaManagementComponent
  extends PagedListingComponentBase<ProcessCriteria>
  implements OnInit {

  search = ""

  public Audits_Criteria = PERMISSIONS_CONSTANT.Audits_Criteria
  public Audits_Criteria_Create = PERMISSIONS_CONSTANT.Audits_Criteria_Create
  public Audits_Criteria_Edit = PERMISSIONS_CONSTANT.Audits_Criteria_Edit
  public Audits_Criteria_Delete = PERMISSIONS_CONSTANT.Audits_Criteria_Delete
  public Audits_Criteria_Active = PERMISSIONS_CONSTANT.Audits_Criteria_Active
  public Audits_Criteria_DeActive = PERMISSIONS_CONSTANT.Audits_Criteria_DeActive
  public Audits_Criteria_ChangeApplicable = PERMISSIONS_CONSTANT.Audits_Criteria_ChangeApplicable

  treeControl = new NestedTreeControl<PeriodicElement>((node) => node.children);
  dataSource = new MatTreeNestedDataSource<PeriodicElement>();
  hasChild = (_: number, node: PeriodicElement) =>
    !!node.children && node.children.length > 0;


  showDialog(command: string, node: any) {
    let criteria = {} as ProcessCriteria;
    if (node) {
      criteria = {
        name: node.item.name,
        code: node.item.code,
        isApplicable: node.item.isApplicable,
        isActive: node.item.isActive,
        guidLine: node.item.guidLine,
        qaExample: node.item.qaExample,
        parentId: node.item.parentId,
        level: node.item.level,
        isLeaf: node.item.isLeaf,
        id: node.item.id,
      }
    }

    const show = this.dialog.open(CreateEditCriteriaAuditComponent, {
      panelClass: 'my-dialog',
      data: {
        item: criteria,
        command: command,
        childrens: (node ? node.children : this.dataSource.data)
      },
      width: "60%",
      height: "72%",
      disableClose: true,
    });
    show.afterClosed().subscribe((result) => {
        this.processCriteriaService.getAll().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>{
          this.treeControl.dataNodes=data.result.childrens
          this.dataSource.data = data.result.childrens
          if(command=='edit'){
            this.exp(this.dataSource.data,node.item.parentId)
          }
         else{
            this.exp(this.dataSource.data,result.parentId)
         }
      })
    });
  }


  constructor(injector: Injector, private dialog: MatDialog, private processCriteriaService: AuditCriteriaProcessService) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh()
  }

  exp(data, parentId: number): any {
    data.forEach(node => {
      if (node.children && node.children.find(c => c.item.parentId == parentId)) {
        console.log(node)
        this.treeControl.expand(node);
        this.exp(this.treeControl.dataNodes, node.item.parentId);
      }
      else if (node.children && node.children.find(c => c.children)) {
        this.exp(node.children, parentId);
      }
    });
  }

  searchCriteria() {
    this.processCriteriaService.search(this.search).pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>
      {this.dataSource.data = data.result.childrens,
      this.treeControl.dataNodes=data.result.childrens
      this.treeControl.expandAll()
    }
      )
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.processCriteriaService.getAll().pipe(finalize(() => {
      finishedCallback();
    })).subscribe((data) => {

      this.dataSource.data = data.result.childrens
      this.treeControl.dataNodes=data.result.childrens
      this.treeControl.expandAll()
      this.showPaging(data.result, pageNumber);
    }, () => { })
  }

  protected delete(node: ProcessCriteria): void {
    if (!node.isLeaf) {
      forkJoin([this.processCriteriaService.ValidToDeleteListCriteria(node.id), this.processCriteriaService.GetAllProcessTailoringContain(node.id)])
        .pipe(catchError(this.processCriteriaService.handleError))
        .subscribe(([rsValid, rsTailoring]) => {
          if (rsValid.success) {
            if (rsTailoring.result.length > 0) {
              let message = `<span><strong> Delete <span style="color:#2991BF">${node.code}: ${node.name}</span> will REMOVE these criteria below from tailoring</strong></span><br>`;
              rsTailoring.result.forEach(element => {
                message += `<span class="text-left"> ${element.code}: ${element.name}</span><br>`;
              });
              abp.message.confirm(`<div style="display: flex; flex-direction: column; align-items: stretch; overflow-y: auto; max-height: 500px;">${message}</div>`, "",
                (result: boolean) => {
                  if (result) {
                    forkJoin([this.processCriteriaService.delete(node.id), this.processCriteriaService.RemoveCriteriaFromTailoring(node.id)])
                      .pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
                        abp.notify.success("Delete " + node.name);
                        this.refresh()
                      })
                  }
                }, {isHtml:true}
              );
            }
            else {
              abp.message.confirm(
                "Delete " + node.name + "?",
                "",
                (result: boolean) => {
                  if (result) {
                    this.processCriteriaService.delete(node.id).pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
                      abp.notify.success("Deleted " + node.name);
                      this.refresh()
                    });
                  }
                }
              );
            }
          }
        });
    }
    if (node.isLeaf) {
      forkJoin([this.processCriteriaService.ValidToDeleteLeafCriteria(node.id), this.processCriteriaService.ValidTailoringContain(node.id)])
        .pipe(catchError(this.processCriteriaService.handleError))
        .subscribe(([rsValid, rsTailoring]) => {
          if (rsValid.success) {
            if (rsTailoring.result) {
              let message = `<span><strong> Delete <span style="color:#2991BF">${node.code}: ${node.name}</span> will REMOVE it from tailoring</strong></span><br>`;
              abp.message.confirm(`<div style="display: flex; flex-direction: column; align-items: stretch; overflow-y: auto; max-height: 500px;">${message}</div>`, "",
                (result: boolean) => {
                  if (result) {
                    forkJoin([this.processCriteriaService.delete(node.id), this.processCriteriaService.RemoveCriteriaFromTailoring(node.id)])
                      .pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
                        abp.notify.success("Delete " + node.name);
                        this.processCriteriaService.getAll().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>{
                          this.treeControl.dataNodes=data.result.childrens
                          this.dataSource.data = data.result.childrens
                          this.treeControl.expandAll()
                      })
                      })
                  }
                }, {isHtml:true}
              );
            }
            else {
              abp.message.confirm(
                "Delete " + node.name + "?",
                "",
                (result: boolean) => {
                  if (result) {
                    this.processCriteriaService.delete(node.id).pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
                      abp.notify.success("Deleted " + node.name);
                      this.processCriteriaService.getAll().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>{
                        this.treeControl.dataNodes=data.result.childrens
                        this.dataSource.data = data.result.childrens
                        this.treeControl.expandAll()
                    })
                    })
                  }
                }
              );
            }
          }
        })
    }
  }

  Inactive(node) {
    this.processCriteriaService.GetAllProcessTailoringContain(node.item.id).subscribe(rs => {
      if (rs.result.length > 0) {
        let message = `<span><strong> Deactivate <span style="color:#2991BF">${node.item.code}: ${node.item.name}</span> will REMOVE these criteria below from tailoring</strong></span><br>`;
        rs.result.forEach(element => {
          message += `<span class="text-left"> ${element.code}: ${element.name}</span><br>`;
        });
        abp.message.confirm(`<div style="display: flex; flex-direction: column; align-items: stretch; overflow-y: auto; max-height: 500px;">
        ${message}
        </div>`, "",
          (result: boolean) => {
            if (result) {
              forkJoin([this.processCriteriaService.DeActive(node.item.id), this.processCriteriaService.RemoveCriteriaFromTailoring(node.item.id)])
                .pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
                  abp.notify.success("Deactivate " + node.item.name);
                  this.processCriteriaService.getAll().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>{
                    this.treeControl.dataNodes=data.result.childrens
                    this.dataSource.data = data.result.childrens
                      this.exp(this.dataSource.data,node.item.parentId)
                })
              })
            }
          }, {isHtml:true}
        );
      }
      else {
        abp.message.confirm(
          "Deactivate " + node.item.name + "?",
          "",
          (result: boolean) => {
            if (result) {
              this.processCriteriaService.DeActive(node.item.id).pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
                abp.notify.success("Deactivate " + node.item.name);
                this.processCriteriaService.getAll().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>{
                  this.treeControl.dataNodes=data.result.childrens
                  this.dataSource.data = data.result.childrens
                    this.exp(this.dataSource.data,node.item.parentId)
              })
              });
            }
          }
        );
      }
    })
  }

  Active(node) {
    abp.message.confirm(
      "Activate " + node.item.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.processCriteriaService.Active(node.item.id).pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
            abp.notify.success("Activate " + node.item.name);
            this.processCriteriaService.getAll().pipe(catchError(this.processCriteriaService.handleError)).subscribe(data =>{
              this.treeControl.dataNodes=data.result.childrens
              this.dataSource.data = data.result.childrens
                this.exp(this.dataSource.data,node.item.parentId)
          })
          });
        }
      }
    );
  }

  editCriteria(node: PeriodicElement) {
    this.showDialog("edit", node);
  }
  createCriteria(node?: PeriodicElement) {
    this.showDialog("create", node);
  }

  viewGuiline(node): void {
    const dialogRef = this.dialog.open(ViewGuilineComponent, {
       panelClass: 'my-dialog',
      width: '60%',
      data: node,
    });

    dialogRef.afterClosed().subscribe(result => {

    });
  }
  setIsApplicable(item) {
    this.processCriteriaService.ChangeApplicable(item.id).pipe(catchError(this.processCriteriaService.handleError)).subscribe(() => {
      item.isApplicable = !item.isApplicable;
      abp.notify.success("Change applicable successfully!");
    });
  }
}
export interface PeriodicElement {
  item: ProcessCriteria
  children?: PeriodicElement[];
}

