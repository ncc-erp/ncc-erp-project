import { CreateEditCriteriaComponent } from './create-edit-criteria/create-edit-criteria.component';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { result } from 'lodash-es';
import { catchError,finalize } from 'rxjs/operators';
import { ProjectCriteriaDto } from './../../../../service/model/criteria-category.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { CriteriaService } from '@app/service/api/criteria.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { MatMenuTrigger } from '@angular/material/menu';

@Component({
  selector: 'app-criteria',
  templateUrl: './criteria.component.html',
  styleUrls: ['./criteria.component.css']
})
export class CriteriaComponent extends PagedListingComponentBase<CriteriaComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.criteriaService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    })).subscribe((data) => {
      this.criteriaList = data.result.items;
      this.showPaging(data.result, pageNumber);
    }, () => { })
  }
  delete(item): void {
    if (item.isExist == true) { abp.message.error("This criterion has been used!"); }
    else {
      abp.message.confirm(
        "Delete project criterion " + item.name + "?",
        "",
        (result: boolean) => {
          if (result) {
            this.criteriaService.deleteCriteria(item.id).pipe(catchError(this.criteriaService.handleError)).subscribe((res) => {
              abp.notify.success("Delete " + item.name + " successfully!");
              this.refresh();
            })
          }
        }
      )
    }
  }
  deActive(item): void {
    abp.message.confirm(
      "DeActivate project criterion " + item.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.criteriaService.deActiveCriteria(item.id).pipe(catchError(this.criteriaService.handleError)).subscribe((res) => {
            abp.notify.success("Deactivate " + item.name + " successfully!");
            this.refresh();
          })
        }
      }
    )
  }
  active(item): void {
    abp.message.confirm(
      "Activate project criterion " + item.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.criteriaService.activeCriteria(item.id).pipe(catchError(this.criteriaService.handleError)).subscribe((res) => {
            abp.notify.success("Activate " + item.name + " successfully!");
            this.refresh();
          })
        }
      }
    )
  }

  Admin_Criteria_View = PERMISSIONS_CONSTANT.Admin_Criteria_View;
  Admin_Criteria_Create = PERMISSIONS_CONSTANT.Admin_Criteria_Create;
  Admin_Criteria_Edit = PERMISSIONS_CONSTANT.Admin_Criteria_Edit;
  Admin_Criteria_Delete = PERMISSIONS_CONSTANT.Admin_Criteria_Delete;
  Admin_Criteria_Active_DeActive = PERMISSIONS_CONSTANT.Admin_Criteria_Active_DeActive;
  public isShowMenu: boolean;
  menu: MatMenuTrigger;
  contextMenuPosition = { x: '0px', y: '0px' };
  public criteriaList: ProjectCriteriaDto[] = [];
  constructor(public injector: Injector,
    private criteriaService: CriteriaService,
    private route: ActivatedRoute,
    private dialog: MatDialog) {
    super(injector)
    this.isShowMenu =
      this.permission.isGranted(this.Admin_Criteria_Edit) ||
      this.permission.isGranted(this.Admin_Criteria_Delete) ||
      this.permission.isGranted(this.Admin_Criteria_Active_DeActive);
  }

  ngOnInit(): void {
    this.refresh();
  }
  public showDialog(command: string, ProjectCriteria) {
    let item = {
      name: ProjectCriteria.name,
      guideline: ProjectCriteria.guideline,
      id: ProjectCriteria.id,
      isActive: ProjectCriteria.isActive

    }
    const show = this.dialog.open(CreateEditCriteriaComponent, {
      data: {
        item: item,
        command: command,

      },
      width: "60%",
      height: "90%",

    })
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    })
  }

  public create() {
    this.showDialog("create", {})
  }
  public edit(item) {
    this.showDialog("edit", item);
  }

  showActions(e , item){
    e.preventDefault();
    this.contextMenuPosition.x = e.clientX + 'px';
    this.contextMenuPosition.y = e.clientY + 'px';
    this.menu.openMenu();
  }

}
