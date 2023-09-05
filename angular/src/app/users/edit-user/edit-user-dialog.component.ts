import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PERMISSIONS_CONSTANT } from './../../constant/permission.constant';
import { SkillService } from './../../service/api/skill.service';
import { UserSkillDto } from './../../service/model/skill.dto';
import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  Inject
} from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { forEach as _forEach, includes as _includes, map as _map } from 'lodash-es';
import { AppComponentBase } from '@shared/app-component-base';
import {
  UserServiceProxy,
  UserDto,
  RoleDto
} from '@shared/service-proxies/service-proxies';
import { BranchService } from '@app/service/api/branch.service';
import { PositionService } from '@app/service/api/position.service';
@Component({
  templateUrl: './edit-user-dialog.component.html',
  styleUrls: ['./edit-user-dialog.component.css']
})
export class EditUserDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  user = new UserDto();
  roles: RoleDto[] = [];
  checkedRolesMap: { [key: string]: boolean } = {};
  action:string
  skillList:UserSkillDto[] = [];
  userLevelList = Object.keys(this.APP_ENUM.UserLevel);
  userBranchList;
  userPositionList;
  userTypeList = Object.keys(this.APP_ENUM.UserType);
  isviewOnlyMe:boolean =false;
  @Output() onSave = new EventEmitter<any>();


  constructor(
    injector: Injector,
    private skillService:SkillService,
    private branchService: BranchService,
    private positionService: PositionService,
    public _userService: UserServiceProxy,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<EditUserDialogComponent>,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllSkill();
    this.getAllPosition();
    this.getAllBranchs();
    this._userService.get(this.data.id).subscribe((result) => {
      this.user = result;
      this.user.userSkills = this.user.userSkills
      this.user.positionId = this.user.positionId
      this._userService.getRoles().subscribe((result2) => {
        this.roles = result2.items;
        this.setInitialRolesStatus();
      });

    })
   

    
  }

  getAllBranchs() {
    this.branchService.getAllNotPagging().subscribe((data) => {
      this.userBranchList = data.result
    })
  }
  getAllSkill(){
    this.skillService.getAll().subscribe(data =>{
      this.skillList = data.result
    })
  }

  getAllPosition() {
    this.positionService.getAllNotPagging().subscribe((data) => {
      this.userPositionList = data.result
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

  save(): void {
   this.user.userSkills =  this.user.userSkills.map(item=>{
      return  {
        userId: this.user.id,
        skillId: item
      };
    })
    var currentSkill =   this.user.userSkills;
    this.saving = true;
    this.user.roleNames = this.getCheckedRoles();

    this._userService
      .update(this.user)
      .pipe(
        finalize(() => {
          this.saving = false;
          this.user.userSkills = currentSkill.map(item=>item.skillId)
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.dialogRef.close(this.user)

      });
  }
}
