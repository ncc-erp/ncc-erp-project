import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { InputFilterDto } from '@shared/filter/filter.component';
import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateTypeLoginComponent } from './create-update-type-login/create-update-type-login.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { TypeLoginService } from '@app/service/api/type-login.service';

@Component({
  selector: 'app-type-login',
  templateUrl: './type-login.component.html',
  styleUrls: ['./type-login.component.css']
})
export class TypeLoginComponent extends PagedListingComponentBase<TypeLoginComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: 'Tên' },
  ];

  public typeLoginList: any[] = [];
  public Admin_TypeLogins_View = PERMISSIONS_CONSTANT.Admin_TypeLogins_View;
  Admin_TypeLogins_Create = PERMISSIONS_CONSTANT.Admin_TypeLogins_Create;
  Admin_TypeLogins_Edit = PERMISSIONS_CONSTANT.Admin_TypeLogins_Edit;
  Admin_TypeLogins_Delete = PERMISSIONS_CONSTANT.Admin_TypeLogins_Delete;

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.typeLoginService.getAllPagging(request).pipe(finalize(() => finishedCallback()), catchError(this.typeLoginService.handleError)).subscribe(data => {
      this.typeLoginList = data.result.items;
      this.showPaging(data.result, pageNumber);
    });
  }

  protected delete(item: any): void {
    abp.message.confirm('Delete Type Login ' + item.name + '?', '', (result: boolean) => {
      if (result) {
        this.typeLoginService.delete(item.id).pipe(catchError(this.typeLoginService.handleError)).subscribe(() => {
          abp.notify.success('Delete Type Login ' + item.name);
          this.refresh();
        });
      }
    });
  }

  constructor(private typeLoginService: TypeLoginService, injector: Injector, private dialog: MatDialog) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  public showDialog(command: string, item: any) {
    const model = { name: item.name, id: item.id };
    const dialogRef = this.dialog.open(CreateUpdateTypeLoginComponent, {
      data: { item: model, command },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

  public createTypeLogin() { this.showDialog('create', {}); }
  public editTypeLogin(item: any) { this.showDialog('update', item); }
}
