import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ProjectAssetService } from '@app/service/api/project-asset.service';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { CreateUpdateAssetComponent } from '@app/modules/pm-management/list-project/list-project-detail/project-asset/create-update-asset/create-update-asset.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';


@Component({
  selector: 'app-product-project-asset',
  templateUrl: './product-project-asset.component.html',
  styleUrls: ['./product-project-asset.component.css']
})
export class ProductProjectAssetComponent extends PagedListingComponentBase<ProductProjectAssetComponent> implements OnInit {
  Projects_ProductProjects_ProjectDetail_TabProjectAsset_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectAsset_View;
  Projects_ProductProjects_ProjectDetail_TabProjectAsset_Create = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectAsset_Create;
  Projects_ProductProjects_ProjectDetail_TabProjectAsset_Edit = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectAsset_Edit;
  Projects_ProductProjects_ProjectDetail_TabProjectAsset_Delete = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabProjectAsset_Delete;

  projectId: number;
  public assetList: any[] = [];

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.projectAssetService.GetAllAssetsByProjectId(this.projectId).pipe(
      finalize(() => {
        finishedCallback();
      }),
      catchError(this.projectAssetService.handleError)
    ).subscribe(data => {
      this.assetList = data.result || [];
      this.totalItems = this.assetList.length;
      this.showPaging(data.result, pageNumber);
    })
  }

  protected delete(asset: any): void {
    abp.message.confirm(
      "Delete Asset " + asset.assetName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.projectAssetService.Delete(asset.id).pipe(
            catchError(this.projectAssetService.handleError)
          ).subscribe((res) => {
            abp.notify.success("Delete asset " + asset.assetName + " successfully!");
            this.refresh();
          })
        }
      }
    )
  }

  constructor(
    private route: ActivatedRoute,
    private projectAssetService: ProjectAssetService,
    injector: Injector,
    private dialog: MatDialog
  ) {
    super(injector);
  }
  
  ngOnInit(): void {
    this.projectId = parseInt(this.route.snapshot.queryParamMap.get('id'));
    this.refresh();
  }

  public showDialog(command: string, asset: any) {
    const assetItem = {
      id: asset.id,
      projectAssetTypeId: asset.projectAssetTypeId || asset.projectAssetType?.id || asset.projectResourceId || asset.projectResource?.id,
      projectAssetTypeIds: asset.projectAssetTypeIds || [],
      projectAssetTypeName: asset.projectAssetTypeName || asset.projectAssetType?.name || asset.projectResourceName || asset.projectResource?.name || '',
      assetName: asset.assetName
    };
    const show = this.dialog.open(CreateUpdateAssetComponent, {
      data: {
        item: assetItem,
        command: command,
        projectId: this.projectId
      },
      width: "700px"
    })
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    })
  }

  public createAsset() {
    this.showDialog("create", {});
  }

  public editAsset(asset: any) {
    this.showDialog("update", asset);
  }
}
