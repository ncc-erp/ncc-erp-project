import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OffboardUserService } from '@app/service/api/offboard-user.service';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';


@Component({
  selector: 'app-offboard-dialog',
  templateUrl: './offboard-dialog.component.html',
  styleUrls: ['./offboard-dialog.component.css']
})
export class OffboardDialogComponent extends AppComponentBase implements OnInit {
  items: any[] = [];
  accountAssetItems: any[] = [];
  projectAssetItems: any[] = [];
  offboardHistoryId: number;
  fullName: string;
  isLoading = false;
  isSaving = false;
  viewOnly = false;

  constructor(
    injector: Injector,
    private _offboardService: OffboardUserService,
    public dialogRef: MatDialogRef<OffboardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    super(injector);
    this.offboardHistoryId = data.offboardHistoryId;
    this.fullName = data.fullName;
    this.viewOnly = data?.viewOnly === true;
  }

  ngOnInit(): void {
    this.load();
  }

  load() {
    this.isLoading = true;
    this._offboardService.GetOffboardChecklist(this.offboardHistoryId)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe((res: any) => {
        this.items = res.result || res || [];
        this.accountAssetItems = this.items.filter(x => x.itemType === 'AccountAsset');
        this.projectAssetItems = this.items.filter(x => x.itemType === 'ProjectAsset');
      });
  }

  save() {
    this.isSaving = true;
    const checkedProjectAssetIds = this.items
      .filter(x => x.itemType === 'ProjectAsset' && x.isChecked)
      .map(x => x.projectAssetId)
      .filter((id): id is number => id !== null && id !== undefined);
    const checkedAccountAssetIds = this.items
      .filter(x => x.itemType === 'AccountAsset' && x.isChecked)
      .map(x => x.accountAssetId)
      .filter((id): id is number => id !== null && id !== undefined);

    const input = {
      offboardHistoryId: this.offboardHistoryId,
      checkedProjectAssetIds,
      checkedAccountAssetIds
    };
    this._offboardService.SaveOffboardChecklist(input)
      .pipe(finalize(() => (this.isSaving = false)))
      .subscribe(() => {
        abp.notify.success('Saved offboard checklist');
        this.dialogRef.close(true);
      });
  }

  close() {
    this.dialogRef.close(false);
  }

}
