import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AccountTypeService } from '@app/service/api/account-type.service';
import { CreatorManagerService } from '@app/service/api/creator-manager.service';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { AccountResourceDto } from '@app/service/model/project.dto';

@Component({
  selector: 'app-add-account-resource-dialog',
  templateUrl: './add-account-resource-dialog.component.html',
  styleUrls: ['./add-account-resource-dialog.component.css']
})
export class AddAccountResourceDialogComponent implements OnInit {
  accountTypes: any[] = [];
  creators: any[] = [];
  model: AccountResourceDto = {} as AccountResourceDto;
  saving = false;

  constructor(
    public dialogRef: MatDialogRef<AddAccountResourceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { projectUserBillId: number; accountResource?: AccountResourceDto },
    private accountTypeService: AccountTypeService,
    private creatorService: CreatorManagerService,
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
      creatorId: this.model.creatorId,
      typeLogin: this.model.typeLogin,
      assetName: this.model.assetName
    };

    const request$ = this.model.id || this.model.accountResourceId
      ? this.projectUserBillService.updateAccountResource(payload)
      : this.projectUserBillService.createAccountResource(payload);

    request$.subscribe({
      next: () => this.dialogRef.close(true),
      error: () => this.saving = false,
      complete: () => this.saving = false
    });
  }
}
