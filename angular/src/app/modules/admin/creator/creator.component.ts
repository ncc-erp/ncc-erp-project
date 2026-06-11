import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { InputFilterDto } from '@shared/filter/filter.component';
import { CreatorDto } from './../../../service/model/list-project.dto';
import { CreatorManagerService } from './../../../service/api/creator-manager.service';
import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateCreatorComponent } from './create-update-creator/create-update-creator.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-creator',
  templateUrl: './creator.component.html',
  styleUrls: ['./creator.component.css']
})
export class CreatorComponent extends PagedListingComponentBase<CreatorComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: 'Tên' },
  ];

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.creatorService.getAllPaging(request).pipe(finalize(() => finishedCallback()), catchError(this.creatorService.handleError)).subscribe(data => {
      this.creatorList = data.result.items;
      this.showPaging(data.result, pageNumber);
    });
  }

  protected delete(creator: CreatorComponent): void {
    abp.message.confirm('Delete Creator ' + creator.name + '?', '', (result: boolean) => {
      if (result) {
        this.creatorService.deleteCreator(creator.id).pipe(catchError(this.creatorService.handleError)).subscribe(() => {
          abp.notify.success('Delete Creator ' + creator.name);
          this.refresh();
        });
      }
    });
  }

  public creatorList: CreatorDto[] = [];
  public Admin_Creators_View = PERMISSIONS_CONSTANT.Admin_Creators_View;
  Admin_Creators_Create = PERMISSIONS_CONSTANT.Admin_Creators_Create;
  Admin_Creators_Edit = PERMISSIONS_CONSTANT.Admin_Creators_Edit;
  Admin_Creators_Delete = PERMISSIONS_CONSTANT.Admin_Creators_Delete;

  constructor(private creatorService: CreatorManagerService, injector: Injector, private dialog: MatDialog) {
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
