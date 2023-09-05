import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  Component,
  Injector,
  OnInit,
  Output,
  EventEmitter,
  Inject
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  TenantServiceProxy,
  TenantDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-tenant-dialog.component.html'
})
export class EditTenantDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  tenant: TenantDto = new TenantDto();
  id: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _tenantService: TenantServiceProxy,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<EditTenantDialogComponent>,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._tenantService.get(this.data.id).subscribe((result: TenantDto) => {
      this.tenant = result;
    });
  }

  save(): void {
    this.saving = true;

    this._tenantService
      .update(this.tenant)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.dialogRef.close(this.tenant);
      });
  }
}
