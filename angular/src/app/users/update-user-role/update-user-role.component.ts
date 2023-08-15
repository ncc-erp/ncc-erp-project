import { catchError } from 'rxjs/operators';
import { UserService } from './../../service/api/user.service';
import { Component, Inject, OnInit } from '@angular/core';
import { RoleDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { forEach as _forEach, includes as _includes, map as _map } from 'lodash-es';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-update-user-role',
  templateUrl: './update-user-role.component.html',
  styleUrls: ['./update-user-role.component.css']
})
export class UpdateUserRoleComponent implements OnInit {
  roles: RoleDto[] = [];
  checkedRolesMap: { [key: string]: boolean } = {};
  user = {} as any
  subscription: Subscription[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<UpdateUserRoleComponent>,
    private _userService: UserServiceProxy, private userInfoService: UserService) { }

  ngOnInit(): void {
    this.user.roleNames = this.data.roleNames
    this.subscription.push(
      this._userService.getRoles().subscribe((result2) => {
        this.roles = result2.items;
        this.setInitialRolesStatus();
      })
    )
  }
  setInitialRolesStatus(): void {
    _map(this.roles, (item) => {
      this.checkedRolesMap[item.normalizedName] = this.isRoleChecked(
        item.normalizedName
      );
    });
  }
  isRoleChecked(normalizedName: string): boolean {
    return _includes(this.user.roleNames, normalizedName);
  }

  onRoleChange(role: RoleDto, $event) {
    this.checkedRolesMap[role.normalizedName] = $event.target.checked;
  }

  getCheckedRoles(): string[] {
    const roles: string[] = [];
    _forEach(this.checkedRolesMap, function (value, key) {
      if (value) {
        roles.push(key);
      }
    });
    return roles;
  }
  save() {
    this.user.userId = this.data.id
    this.user.roleNames = this.getCheckedRoles();
    this.subscription.push(
      this.userInfoService.UpdateUserRole(this.user).subscribe(rs => {
        abp.notify.success("Updated user role")
        this.dialogRef.close(true)
      })
    )
  }
  ngOnDestroy(): void {
    this.subscription.forEach(sub => sub.unsubscribe)
  }
}
