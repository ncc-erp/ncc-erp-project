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
  offboardHistoryId: number;
  fullName: string;
  isLoading = false;
  isSaving = false;

  constructor(
    injector: Injector,
    private _offboardService: OffboardUserService,
    public dialogRef: MatDialogRef<OffboardDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    super(injector);
    this.offboardHistoryId = data.offboardHistoryId;
    this.fullName = data.fullName;
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
      });
  }

  save() {
    this.isSaving = true;
    const checked = this.items.filter(x => x.isChecked).map(x => x.projectAssetId);
    const input = { offboardHistoryId: this.offboardHistoryId, checkedProjectAssetIds: checked };
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
