import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectAssetService } from '@app/service/api/project-asset.service';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { AccountAssetDto } from '@app/service/model/project.dto';

@Component({
  selector: 'app-add-account-asset-dialog',
  templateUrl: './add-account-asset-dialog.component.html',
  styleUrls: ['./add-account-asset-dialog.component.css']
})
export class AddAccountAssetDialogComponent implements OnInit {
  public availableAssets: any[] = [];
  public model: AccountAssetDto = {} as AccountAssetDto;
  public saving = false;

  constructor(
    public dialogRef: MatDialogRef<AddAccountAssetDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { projectId: number; projectUserBillId: number; accountAsset?: AccountAssetDto },
    private projectAssetService: ProjectAssetService,
    private projectUserBillService: ProjectUserBillService,
  ) {
    if (data?.accountAsset) {
      this.model = { ...data.accountAsset };
    } else {
      this.model.projectUserBillId = data?.projectUserBillId;
    }
  }

  ngOnInit(): void {
    this.loadProjectAssets();
  }

  private loadProjectAssets(): void {
    if (!this.data.projectId) {
      return;
    }

    this.projectAssetService.GetAllForDropdown(this.data.projectId).subscribe(res => {
      this.availableAssets = (res && res.result ? res.result : res) || [];
    });
  }

  save(): void {
    this.saving = true;
    const accountAssetId = this.model.id ?? 0;

    const payload = {
      id: accountAssetId,
      projectUserBillId: this.model.projectUserBillId,
      projectAssetId: this.model.projectAssetId
    };

    const request$ = accountAssetId > 0
      ? this.projectUserBillService.updateAccountAsset(payload)
      : this.projectUserBillService.createAccountAsset(payload);

    request$.subscribe({
      next: () => this.dialogRef.close(true),
      error: () => this.saving = false,
      complete: () => this.saving = false
    });
  }
}
