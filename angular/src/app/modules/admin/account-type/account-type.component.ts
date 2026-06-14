import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { InputFilterDto } from '@shared/filter/filter.component';
import { AccountTypeDto } from './../../../service/model/list-project.dto';
import { AccountTypeService } from './../../../service/api/account-type.service';
import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateAccountTypeComponent } from './create-update-account-type/create-update-account-type.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-account-type',
  templateUrl: './account-type.component.html',
  styleUrls: ['./account-type.component.css']
})
export class AccountTypeComponent extends PagedListingComponentBase<AccountTypeComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: 'Tên' },
  ];

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.accountTypeService.getAllPaging(request).pipe(finalize(() => finishedCallback()), catchError(this.accountTypeService.handleError)).subscribe(data => {
      this.accountTypeList = data.result.items;
      this.showPaging(data.result, pageNumber);
    });
  }

  protected delete(accountType: AccountTypeComponent): void {
    abp.message.confirm('Delete AccountType ' + accountType.name + '?', '', (result: boolean) => {
      if (result) {
        this.accountTypeService.deleteAccountType(accountType.id).pipe(catchError(this.accountTypeService.handleError)).subscribe(() => {
          abp.notify.success('Delete AccountType ' + accountType.name);
          this.refresh();
        });
      }
    });
  }

  public accountTypeList: AccountTypeDto[] = [];
  public Admin_AccountTypes_View = PERMISSIONS_CONSTANT.Admin_AccountTypes_View;
  Admin_AccountTypes_Create = PERMISSIONS_CONSTANT.Admin_AccountTypes_Create;
  Admin_AccountTypes_Edit = PERMISSIONS_CONSTANT.Admin_AccountTypes_Edit;
  Admin_AccountTypes_Delete = PERMISSIONS_CONSTANT.Admin_AccountTypes_Delete;

  constructor(private accountTypeService: AccountTypeService, injector: Injector, private dialog: MatDialog) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  public showDialog(command: string, item: any) {
    const accountType = { name: item.name, id: item.id };
    const dialogRef = this.dialog.open(CreateUpdateAccountTypeComponent, {
      data: { item: accountType, command },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

  public createAccountType() { this.showDialog('create', {}); }
  public editAccountType(item: any) { this.showDialog('update', item); }
}
