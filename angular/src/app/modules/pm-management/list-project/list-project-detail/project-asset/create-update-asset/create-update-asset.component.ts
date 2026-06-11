import { ProjectAssetService } from '@app/service/api/project-asset.service';
import { ProjectResourceService } from '@app/service/api/project-resource.service';
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
  public projectResources: any[] = [];
  public isCreateMode = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public projectAssetService: ProjectAssetService,
    public projectResourceService: ProjectResourceService,
    public dialogRef: MatDialogRef<CreateUpdateAssetComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.isCreateMode = this.data.command === 'create';
    this.asset = {
      id: this.data.item?.id,
      projectResourceId: this.data.item?.projectResourceId || null,
      projectResourceIds: this.data.item?.projectResourceIds || [],
      assetName: this.data.item?.assetName || '',
      projectResourceName: this.data.item?.projectResourceName || ''
    };

    this.title = this.asset.assetName || 'New asset';

    this.projectResourceService.getAll().subscribe((res) => {
      this.projectResources = res.result || res || [];
    });
  }

  private getAllChildResources(): any[] {
    return this.projectResources.reduce((result: any[], parent: any) => {
      return result.concat(parent.childrens || []);
    }, []);
  }

  private buildSinglePayload(): any {
    const selectedResource = this.getAllChildResources().find((resource: any) => resource.id === this.asset.projectResourceId);

    return {
      id: this.asset.id,
      projectResourceId: this.asset.projectResourceId,
      assetName: this.asset.assetName || selectedResource?.name || ''
    };
  }

  private buildCreatePayloads(): any[] {
    const selectedIds = Array.from(new Set(this.asset.projectResourceIds || []));

    return selectedIds.map((id: number) => {
      const selectedResource = this.getAllChildResources().find((resource: any) => resource.id === id);
      return {
        projectResourceId: id,
        assetName: this.asset.assetName || selectedResource?.name || ''
      };
    });
  }

  SaveAndClose() {
    if (this.data.command == "create") {
      const payloads = this.buildCreatePayloads();

      if (!payloads.length) {
        abp.message.warn('Please select at least one project resource.');
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
