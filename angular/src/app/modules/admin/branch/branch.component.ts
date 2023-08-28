import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { CreateUpdateBranchComponent } from './create-update-branch/create-update-branch.component';
import { MatDialog } from '@angular/material/dialog';
import { result } from 'lodash-es';
import { finalize, catchError } from 'rxjs/operators';
import { BranchDto } from '../../../service/model/branch.dto'; 
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { BranchService } from '../../../service/api/branch.service';
import { Component, OnInit, inject, Injector } from '@angular/core';
import { isNgTemplate } from '@angular/compiler';
import { InputFilterDto } from '@shared/filter/filter.component';

@Component({
  selector: 'app-branch',
  templateUrl: './branch.component.html',
  styleUrls: ['./branch.component.css']
})

export class BranchComponent extends PagedListingComponentBase<BranchComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: "Tên", },

  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.branchService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.branchService.handleError)).subscribe(data => {
      this.branchList = data.result.items;
      this.showPaging(data.result, pageNumber)
    })
  }

  protected delete(branch: BranchComponent): void {
    abp.message.confirm(
      "Delete branch " + branch.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.branchService.deleteBranch(branch.id).pipe(catchError(this.branchService.handleError)).subscribe((res) => {
            abp.notify.success("Delele branch " + branch.name);
            this.refresh()
          })
        }
      }
    )
  }

  public branchList: BranchDto[] = [];
  Admin_Branchs_View = PERMISSIONS_CONSTANT.Admin_Branchs_View;
  Admin_Branchs_Create = PERMISSIONS_CONSTANT.Admin_Branchs_Create;
  Admin_Branchs_Edit = PERMISSIONS_CONSTANT.Admin_Branchs_Edit;
  Admin_Branchs_Delete = PERMISSIONS_CONSTANT.Admin_Branchs_Delete;

  constructor(private branchService: BranchService,
    injector: Injector,
    private dialog: MatDialog) { super(injector) }

  ngOnInit(): void {
    this.refresh();
  }
  public showDialog(command: string, branch: any) {
    let item = {
      name: branch.name,
      code: branch.code,
      displayName: branch.displayName,
      color: branch.color,
      id: branch.id,
    }
    const show = this.dialog.open(CreateUpdateBranchComponent, {
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
  public createBranch() {
    this.showDialog("create", {});
  }
  public editBranch(branch) {
    this.showDialog("update", branch);

  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}
