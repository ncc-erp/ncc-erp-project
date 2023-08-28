import { MatDialog } from '@angular/material/dialog';
import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from '@shared/paged-listing-component-base';
import {
  TenantServiceProxy,
  TenantDto,
  TenantDtoPagedResultDto,
} from '@shared/service-proxies/service-proxies';
import { CreateTenantDialogComponent } from './create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './edit-tenant/edit-tenant-dialog.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';

class PagedTenantsRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  templateUrl: './tenants.component.html',
  animations: [appModuleAnimation()],
  styleUrls: ['./tenants.component.css']
})
export class TenantsComponent extends PagedListingComponentBase<TenantDto> {
  Admin_Tenants_Create = PERMISSIONS_CONSTANT.Admin_Tenants_Create;
  Admin_Tenants_Edit = PERMISSIONS_CONSTANT.Admin_Tenants_Edit;
  Admin_Tenants_Delete = PERMISSIONS_CONSTANT.Admin_Tenants_Delete;

  tenants: TenantDto[] = [];
  keyword = '';
  isActive: boolean | null;
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _tenantService: TenantServiceProxy,
    private dialog: MatDialog,
  ) {
    super(injector);
  }

  list(
    request: PagedTenantsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = this.isActive;

    this._tenantService
      .getAll(
        request.keyword,
        request.isActive,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: TenantDtoPagedResultDto) => {
        this.tenants = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(tenant: TenantDto): void {
    abp.message.confirm(
      this.l('TenantDeleteWarningMessage', tenant.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._tenantService
            .delete(tenant.id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l('SuccessfullyDeleted'));
                this.refresh();
              })
            )
            .subscribe(() => {});
        }
      }
    );
  }

  createTenant(): void {
    this.showCreateOrEditTenantDialog();
  }

  editTenant(tenant: TenantDto): void {
    this.showCreateOrEditTenantDialog(tenant.id);
  }

  showCreateOrEditTenantDialog(id?: number): void {
    if (!id) {
      const showCreate = this.dialog.open(CreateTenantDialogComponent, {
        width: "700px",
        disableClose: true,
      });
      showCreate.afterClosed().subscribe(res => {
        if (res) {
          this.refresh()
        }
      })
    } else {
      const showEdit = this.dialog.open(EditTenantDialogComponent, {
        data: {id: id},
        width: "700px",
        disableClose: true,
      });
      showEdit.afterClosed().subscribe(res => {
        if (res) {
          this.refresh()
        }
      })
    }
  }

  clearFilters(): void {
    this.keyword = '';
    this.isActive = undefined;
    this.getDataPage(1);
  }
}
