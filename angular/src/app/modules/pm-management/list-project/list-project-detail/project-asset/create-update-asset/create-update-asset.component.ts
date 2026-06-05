import { ProjectAssetService } from '@app/service/api/project-asset.service';
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

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public projectAssetService: ProjectAssetService,
    public dialogRef: MatDialogRef<CreateUpdateAssetComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.asset = this.data.item;
    this.title = this.asset.assetName;
  }

  SaveAndClose() {
    if (this.data.command == "create") {
      this.projectAssetService.Create(this.data.projectId, this.asset).pipe(
        catchError(this.projectAssetService.handleError)
      ).subscribe((res) => {
        abp.notify.success("Create asset successfully!");
        this.dialogRef.close(this.asset);
      }, () => { this.isLoading = false })
    }
    else {
      this.projectAssetService.Edit(this.data.projectId, this.asset).pipe(
        catchError(this.projectAssetService.handleError)
      ).subscribe((res) => {
        abp.notify.success("Update asset successfully!");
        this.dialogRef.close(this.asset);
      }, () => { this.isLoading = false })
    }
  }
}
