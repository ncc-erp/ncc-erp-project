import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { InputFilterDto } from '@shared/filter/filter.component';
import { AccountAssetCreatorDto } from '../../../service/model/list-project.dto';
import { AccountAssetCreatorManagerService } from '../../../service/api/account-asset-creator.service';
import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateCreatorComponent } from './create-update-creator/create-update-creator.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-account-asset-creator',
  templateUrl: './account-asset-creator.component.html',
  styleUrls: ['./account-asset-creator.component.css']
})
export class AccountAssetCreatorComponent extends PagedListingComponentBase<AccountAssetCreatorComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: 'Tên' },
  ];

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.creatorService.getAllPaging(request).pipe(finalize(() => finishedCallback()), catchError(this.creatorService.handleError)).subscribe(data => {
      this.creatorList = data.result.items;
      this.showPaging(data.result, pageNumber);
    });
  }

  protected delete(creator: AccountAssetCreatorComponent): void {
    abp.message.confirm('Delete Creator ' + creator.name + '?', '', (result: boolean) => {
      if (result) {
        this.creatorService.deleteCreator(creator.id).pipe(catchError(this.creatorService.handleError)).subscribe(() => {
          abp.notify.success('Delete Creator ' + creator.name);
          this.refresh();
        });
      }
    });
  }

  public creatorList: AccountAssetCreatorDto[] = [];
  public Admin_AccountAssetCreators_View = PERMISSIONS_CONSTANT.Admin_AccountAssetCreators_View;
  Admin_AccountAssetCreators_Create = PERMISSIONS_CONSTANT.Admin_AccountAssetCreators_Create;
  Admin_AccountAssetCreators_Edit = PERMISSIONS_CONSTANT.Admin_AccountAssetCreators_Edit;
  Admin_AccountAssetCreators_Delete = PERMISSIONS_CONSTANT.Admin_AccountAssetCreators_Delete;

  constructor(private creatorService: AccountAssetCreatorManagerService, injector: Injector, private dialog: MatDialog) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  public showDialog(command: string, item: any) {
    const creator = { name: item.name, id: item.id };
    const dialogRef = this.dialog.open(CreateUpdateCreatorComponent, {
      data: { item: creator, command },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

  public createCreator() { this.showDialog('create', {}); }
  public editCreator(item: any) { this.showDialog('update', item); }
}
