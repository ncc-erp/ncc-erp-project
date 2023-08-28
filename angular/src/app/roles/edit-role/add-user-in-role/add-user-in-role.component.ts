import { RoleServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-add-user-in-role',
  templateUrl: './add-user-in-role.component.html',
  styleUrls: ['./add-user-in-role.component.css']
})
export class AddUserInRoleComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AddUserInRoleComponent>,
    private _roleService: RoleServiceProxy,
    public injector: Injector,) {
    super(injector)

  }
  public searchUser: string = ''
  public userId: number;
  public listUsersNotInRole: any[] = []

  ngOnInit(): void {
    this.getAllUserNotInRole();
  }
  getAllUserNotInRole() {
    this._roleService.getAllUserNotInRole(this.data.roleId).subscribe(res => {
      this.listUsersNotInRole = res.result
    })
  }
  SaveAndClose() {
    this.isLoading = true
    let request = {
      userId: this.userId,
      roleId: this.data.roleId
    }
    this._roleService.addUserIntoRole(request).subscribe((res) => {
      if (res.success) {
        abp.notify.success(res.result)
        this.dialogRef.close(this.userId);
      }
      else {
        abp.notify.error(res.result)
      }
    }, () => this.isLoading = false)
  }
}




