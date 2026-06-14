import { ProjectAssetService } from '@app/service/api/project-asset.service';
import { ProjectAssetTypeService } from '@app/service/api/project-asset-type.service';
import { Inject, Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';


@Component({
  selector: 'app-create-update-asset',
  templateUrl: './create-update-asset.component.html',
  styleUrls: ['./create-update-asset.component.css']
})
export class CreateUpdateAssetComponent extends AppComponentBase implements OnInit {
  public title: string = "";
  public asset: any = {};
  public projectAssetTypes: any[] = [];
  public isCreateMode = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public projectAssetService: ProjectAssetService,
    public projectAssetTypeService: ProjectAssetTypeService,
    public dialogRef: MatDialogRef<CreateUpdateAssetComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.isCreateMode = this.data.command === 'create';
    this.asset = {
      id: this.data.item?.id,
      projectAssetTypeId: this.data.item?.projectAssetTypeId || null,
      projectAssetTypeIds: this.data.item?.projectAssetTypeIds || [],
      assetName: this.data.item?.assetName || '',
      projectAssetTypeName: this.data.item?.projectAssetTypeName || ''
    };

    this.title = this.asset.assetName || 'New asset';

    this.projectAssetTypeService.getAll().subscribe((res) => {
      this.projectAssetTypes = res.result || res || [];
    });
  }

  private getAllChildAssetTypes(): any[] {
    return this.projectAssetTypes.reduce((result: any[], parent: any) => {
      return result.concat(parent.childrens || []);
    }, []);
  }

  private buildSinglePayload(): any {
    const selectedResource = this.getAllChildAssetTypes().find((resource: any) => resource.id === this.asset.projectAssetTypeId);

    return {
      id: this.asset.id,
      projectAssetTypeId: this.asset.projectAssetTypeId,
      assetName: this.asset.assetName || selectedResource?.name || ''
    };
  }

  private buildCreatePayloads(): any[] {
    const selectedIds = this.asset.projectAssetTypeId ? [this.asset.projectAssetTypeId] : [];

    return selectedIds.map((id: number) => {
      const selectedResource = this.getAllChildAssetTypes().find((resource: any) => resource.id === id);
      return {
        projectAssetTypeId: id,
        assetName: this.asset.assetName || selectedResource?.name || ''
      };
    });
  }

  SaveAndClose() {
    if (this.data.command == "create") {
      const payloads = this.buildCreatePayloads();

      if (!payloads.length) {
        abp.message.warn('Please select a project asset type.');
        return;
      }

      this.projectAssetService.Create(this.data.projectId, payloads).pipe(
        catchError(this.projectAssetService.handleError)
      ).subscribe(() => {
        abp.notify.success('Create assets successfully!');
        this.dialogRef.close(payloads);
      }, () => { this.isLoading = false });
      return;
    }

    const payload = this.buildSinglePayload();

    this.projectAssetService.Edit(this.data.projectId, payload).pipe(
      catchError(this.projectAssetService.handleError)
    ).subscribe(() => {
      abp.notify.success('Update asset successfully!');
      this.dialogRef.close(payload);
    }, () => { this.isLoading = false });
  }
}
