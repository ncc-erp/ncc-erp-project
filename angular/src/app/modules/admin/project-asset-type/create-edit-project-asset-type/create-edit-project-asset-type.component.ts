import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectAssetTypeService } from '@app/service/api/project-asset-type.service';
import { ProjectAssetTypeDto } from '@app/service/model/list-project.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-create-edit-project-asset-type',
  templateUrl: './create-edit-project-asset-type.component.html',
  styleUrls: ['./create-edit-project-asset-type.component.css']
})
export class CreateEditProjectAssetTypeComponent extends AppComponentBase implements OnInit {
  public title = '';
  public item = {} as ProjectAssetTypeDto;
  public parentList: ProjectAssetTypeDto[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateEditProjectAssetTypeComponent>,
    public projectAssetTypeService: ProjectAssetTypeService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.item = { ...this.data.item };
    this.parentList = this.data.parentList || [];
    this.title = this.data.command === 'create' ? 'Create Project Asset Type' : 'Edit Project Asset Type';
  }

  public save(): void {
    const payload = {
      id: this.item.id,
      name: this.item.name,
      note: this.item.note,
      parentId: this.item.parentId || null
    } as ProjectAssetTypeDto;

    const request = this.data.command === 'create'
      ? this.projectAssetTypeService.create(payload)
      : this.projectAssetTypeService.update(payload);

    request.pipe(catchError(this.projectAssetTypeService.handleError)).subscribe(() => {
      abp.notify.success(this.data.command === 'create' ? 'Create Project Asset Type Successfully!' : 'Update Project Asset Type Successfully!');
      this.dialogRef.close(true);
    }, () => {
      this.isLoading = false;
    });
  }
}
