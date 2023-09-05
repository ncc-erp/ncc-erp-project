import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { finalize } from 'rxjs/operators';
import {
  UserServiceProxy,
  ResetPasswordDto
} from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordDialogComponent extends AppComponentBase
  implements OnInit {
  public isLoading = false;
  public resetPasswordDto: ResetPasswordDto;
  id: number;

  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<ResetPasswordDialogComponent>,
  ) {
    super(injector);
  }

  ngOnInit() {
    this.resetPasswordDto = new ResetPasswordDto();
    this.resetPasswordDto.userId = this.data.id;
  }

  randomPassword() {
    let passRandom = Math.random()
      .toString(36)
      .substr(2, 10);

    this.resetPasswordDto.newPassword = passRandom;
  }
  public resetPassword(): void {
    this.isLoading = true;
    this._userService
      .resetPassword(this.resetPasswordDto)
      .subscribe(() => {
        this.notify.success('Password reset successful');
        this.dialogRef.close(this.resetPassword);
        this.isLoading = false;
      });
  }
}
