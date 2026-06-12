import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AccountTypeService } from '@app/service/api/account-type.service';
import { AccountAssetCreatorManagerService } from '@app/service/api/account-asset-creator.service';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { AccountAssetDto } from '@app/service/model/project.dto';

@Component({
  selector: 'app-add-account-asset-dialog',
  templateUrl: './add-account-asset-dialog.component.html',
  styleUrls: ['./add-account-asset-dialog.component.css']
})
export class AddAccountAssetDialogComponent implements OnInit {
  accountTypes: any[] = [];
  creators: any[] = [];
  model: AccountAssetDto = {} as AccountAssetDto;
  saving = false;

  constructor(
    public dialogRef: MatDialogRef<AddAccountAssetDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { projectUserBillId: number; accountResource?: AccountAssetDto },
    private accountTypeService: AccountTypeService,
    private creatorService: AccountAssetCreatorManagerService,
    private projectUserBillService: ProjectUserBillService,
  ) {
    if (data?.accountResource) {
      this.model = { ...data.accountResource };
    } else {
      this.model.projectUserBillId = data?.projectUserBillId;
    }
  }

  ngOnInit(): void {
    this.accountTypeService.getAll().subscribe(res => {
      this.accountTypes = (res && res.result ? res.result : res) || [];
    });

    this.creatorService.getAll().subscribe(res => {
      this.creators = (res && res.result ? res.result : res) || [];
    });
  }

  save(): void {
    this.saving = true;
    const payload = {
      accountResourceId: this.model.accountResourceId || this.model.id,
      projectUserBillId: this.model.projectUserBillId,
      accountTypeId: this.model.accountTypeId,
      accountAssetCreatorId: this.model.accountAssetCreatorId,
      typeLogin: this.model.typeLogin,
      assetName: this.model.assetName
    };

    const request$ = this.model.id || this.model.accountResourceId
      ? this.projectUserBillService.updateAccountAsset(payload)
      : this.projectUserBillService.createAccountAsset(payload);

    request$.subscribe({
      next: () => this.dialogRef.close(true),
      error: () => this.saving = false,
      complete: () => this.saving = false
    });
  }
}
