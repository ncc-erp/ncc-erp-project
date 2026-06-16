import { ProjectAssetService } from '@app/service/api/project-asset.service';
import { ProjectAssetTypeService } from '@app/service/api/project-asset-type.service';
import { AccountTypeService } from '@app/service/api/account-type.service';
import { AccountAssetCreatorManagerService } from '@app/service/api/account-asset-creator.service';
import { TypeLoginService } from '@app/service/api/type-login.service';
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
  public accountTypes: any[] = [];
  public creators: any[] = [];
  public typeLogins: any[] = [];
  public isCreateMode = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public projectAssetService: ProjectAssetService,
    public projectAssetTypeService: ProjectAssetTypeService,
    public accountTypeService: AccountTypeService,
    public creatorService: AccountAssetCreatorManagerService,
    public typeLoginService: TypeLoginService,
    public dialogRef: MatDialogRef<CreateUpdateAssetComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.isCreateMode = this.data.command === 'create';
    this.asset = {
      id: this.data.item?.id,
      projectAssetTypeId: this.data.item?.projectAssetTypeId || null,
      assetName: this.data.item?.assetName || '',
      accountTypeId: this.data.item?.accountTypeId || null,
      accountAssetCreatorId: this.data.item?.accountAssetCreatorId || null,
      typeLoginId: this.data.item?.typeLoginId || null
    };

    this.title = this.asset.assetName || 'New asset';

    this.projectAssetTypeService.getAll().subscribe((res) => {
      this.projectAssetTypes = res.result || res || [];
    });

    this.accountTypeService.getAll().subscribe((res) => {
      this.accountTypes = res.result || res || [];
    });

    this.creatorService.getAll().subscribe((res) => {
      this.creators = res.result || res || [];
    });

    this.typeLoginService.getAll().subscribe((res) => {
      this.typeLogins = res.result || res || [];
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
      assetName: this.asset.assetName || selectedResource?.name || '',
      accountTypeId: this.asset.accountTypeId,
      accountAssetCreatorId: this.asset.accountAssetCreatorId,
      typeLoginId: this.asset.typeLoginId
    };
  }

  private buildCreatePayloads(): any[] {
    const selectedIds = this.asset.projectAssetTypeId ? [this.asset.projectAssetTypeId] : [];

    return selectedIds.map((id: number) => {
      const selectedResource = this.getAllChildAssetTypes().find((resource: any) => resource.id === id);
      return {
        projectAssetTypeId: id,
        assetName: this.asset.assetName || selectedResource?.name || '',
        accountTypeId: this.asset.accountTypeId,
        accountAssetCreatorId: this.asset.accountAssetCreatorId,
        typeLoginId: this.asset.typeLoginId
      };
    });
  }

  SaveAndClose() {
    if (this.data.command == "create") {
      const payload = this.buildSinglePayload();

      if (!payload.projectAssetTypeId) {
        abp.message.warn('Please select a project asset type.');
        return;
      }

      this.projectAssetService.Create(this.data.projectId, payload).pipe(
        catchError(this.projectAssetService.handleError)
      ).subscribe(() => {
        abp.notify.success('Create assets successfully!');
        this.dialogRef.close(payload);
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
