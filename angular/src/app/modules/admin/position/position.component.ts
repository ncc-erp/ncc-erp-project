import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { MatDialog } from '@angular/material/dialog';
import { result } from 'lodash-es';
import { finalize, catchError } from 'rxjs/operators';
import { BranchDto } from '../../../service/model/branch.dto';
import { BranchService } from '../../../service/api/branch.service';
import { PositionService } from '../../../service/api/position.service';
import { Component, OnInit, inject, Injector } from '@angular/core';
import { isNgTemplate } from '@angular/compiler';
import { InputFilterDto } from '@shared/filter/filter.component';
import { CreateUpdatePositionComponent } from './create-update-position/create-update-position.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { PositionDto } from '@app/service/model/position.dto';

@Component({
  selector: 'app-position',
  templateUrl: './position.component.html',
  styleUrls: ['./position.component.css']
})
export class PositionComponent extends PagedListingComponentBase<PositionComponent> implements OnInit {
  [x: string]: any;

  title:string =""
  public position = {} as PositionDto;
  invoiceDateSettingList: [];
  paymentDueByList = [];

  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: "Tên", },

  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.positionService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.positionService.handleError)).subscribe(data => {
      this.positionList = data.result.items;
      this.showPaging(data.result, pageNumber)
    })
  }

  protected delete(position: PositionComponent): void {
    abp.message.confirm(
      "Delete position " + position.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.positionService.deletePosition(position.id).pipe(catchError(this.positionService.handleError)).subscribe((res) => {
            abp.notify.success("Delele position " + position.name);
            this.refresh()
          })
        }
      }
    )
  }

  public positionList: PositionDto[] = [];
  Admin_Positions_View = PERMISSIONS_CONSTANT.Admin_Positions_View;
  Admin_Positions_Create = PERMISSIONS_CONSTANT.Admin_Positions_Create;
  Admin_Positions_Edit = PERMISSIONS_CONSTANT.Admin_Positions_Edit;
  Admin_Positions_Delete = PERMISSIONS_CONSTANT.Admin_Positions_Delete;

  constructor(private positionService: PositionService,
    injector: Injector,
    private dialog: MatDialog) { super(injector) }

  ngOnInit(): void {
    this.refresh();
  }
  public showDialog(command: string, position: any) {
    let item = {
      name: position.name,
      code: position.code,
      shortName: position.shortName,
      color: position.color,
      id: position.id,
    }
    const show = this.dialog.open(CreateUpdatePositionComponent, {
      data: {
        item: item,
        command: command
      },
      width: "700px"
    })
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    })

  }
  public createPosition() {
    this.showDialog("create", {});
  }
  public editPosition(position) {
    this.showDialog("update", position);

  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}
