import { MatDialogRef } from '@angular/material/dialog';
import { UserSkillDto } from './../../service/model/skill.dto';
import { SkillService } from './../../service/api/skill.service';
import {
  Component,
  Injector,
  OnInit,

} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { forEach as _forEach, map as _map } from 'lodash-es';
import { AppComponentBase } from '@shared/app-component-base';
import {
  UserServiceProxy,
  CreateUserDto,
  RoleDto
} from '@shared/service-proxies/service-proxies';
import { AbpValidationError } from '@shared/components/validation/abp-validation.api';
import { BranchService } from '@app/service/api/branch.service';
import { PositionService } from '@app/service/api/position.service';
@Component({
  templateUrl: './create-user-dialog.component.html'
})
export class CreateUserDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  user = new CreateUserDto();
  roles: RoleDto[] = [];
  skillList:UserSkillDto[] = [];
  checkedRolesMap: { [key: string]: boolean } = {};
  defaultRoleCheckedStatus = false;
  userLevelList = Object.keys(this.APP_ENUM.UserLevel);
  userBranchList;
  userPositionList;
  userTypeList = Object.keys(this.APP_ENUM.UserType);
  passwordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'pattern',
      localizationKey:
        'PasswordsMustBeAtLeast8CharactersContainLowercaseUppercaseNumber',
    },
  ];
  confirmPasswordValidationErrors: Partial<AbpValidationError>[] = [
    {
      name: 'validateEqual',
      localizationKey: 'PasswordsDoNotMatch',
    },
  ];



  constructor(
    injector: Injector,
    public _userService: UserServiceProxy,
    private skillService:SkillService,
    private branchService: BranchService,
    private positionService: PositionService,
    public dialogRef: MatDialogRef<CreateUserDialogComponent>,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.user.userName=""
    this.user.password=""
    this.user.isActive = true;
    this.getAllSkill();
    this.getAllPosition();
    this.getAllBranchs();
    this._userService.getRoles().subscribe((result) => {
      this.roles = result.items;
      this.setInitialRolesStatus();
    });
  }


  getAllBranchs() {
    this.branchService.getAllNotPagging().subscribe((data) => {
      this.userBranchList = data.result
    })
  }
  getAllPosition() {
    this.positionService.getAllNotPagging().subscribe((data) => {
      this.userPositionList = data.result
    })
  }
  getAllSkill(){
    this.skillService.getAll().subscribe(data =>{
      this.skillList =data.result
    })
  }
  setInitialRolesStatus(): void {
    _map(this.roles, (item) => {
      this.checkedRolesMap[item.normalizedName] = this.isRoleChecked(
        item.normalizedName
      );
    });
  }

  isRoleChecked(normalizedName: string): boolean {
    // just return default role checked status
    // it's better to use a setting
    return this.defaultRoleCheckedStatus;
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

  save(): void {
    this.saving = true;
    this.user.isActive = true
    this.user.roleNames = this.getCheckedRoles();

    this._userService
      .create(this.user)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.dialogRef.close(this.user)
      });
  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}
