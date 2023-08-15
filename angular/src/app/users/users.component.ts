import { UpdateUserSkillDialogComponent } from './update-user-skill-dialog/update-user-skill-dialog.component';
import { SkillService } from './../service/api/skill.service';
import { DropDownDataDto } from './../../shared/filter/filter.component';
import { result } from 'lodash-es';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { Component, Injector } from '@angular/core';
import { catchError, finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
  UserServiceProxy,
  UserDto,
  UserDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { CreateUserDialogComponent } from './create-user/create-user-dialog.component';
import { EditUserDialogComponent } from './edit-user/edit-user-dialog.component';
import { ResetPasswordDialogComponent } from './reset-password/reset-password.component';
import { UploadAvatarComponent } from './upload-avatar/upload-avatar.component';
import { AppConsts } from '@shared/AppConsts';
import { UserService } from '@app/service/api/user.service';
import { MatDialog } from '@angular/material/dialog';
import { InputFilterDto } from '@shared/filter/filter.component';
import { IUSerProjectHistory } from '@app/service/model/user.inteface';
import * as moment from 'moment';
import { UpdateUserRoleComponent } from './update-user-role/update-user-role.component';

class PagedUsersRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  templateUrl: './users.component.html',
  animations: [appModuleAnimation()],
  styleUrls: ['./users.component.css']

})
export class UsersComponent extends PagedListingComponentBase<UserDto> {
  userLevelParam = Object.entries(this.APP_ENUM.UserLevel).map(item => {
    return {
      displayName: item[0],
      value: item[1]
    }
  })
  userTypeParam = Object.entries(this.APP_ENUM.UserType).map(item => {
    return {
      displayName: item[0],
      value: item[1]
    }
  })
  branchParam = Object.entries(this.APP_ENUM.UserBranch).map(item => {
    return {
      displayName: item[0],
      value: item[1]
    }
  })

  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'fullName', displayName: "Name", comparisions: [0, 6, 7, 8] },
    { propertyName: 'emailAddress', displayName: "emailAddress", comparisions: [0, 6, 7, 8] },
    { propertyName: 'userCode', displayName: "User Code", comparisions: [0, 6, 7, 8] },
    { propertyName: 'lastLoginTime', displayName: "Last Login Time", comparisions: [0, 1, 2, 3, 4], filterType: 1 },
    { propertyName: 'creationTime', displayName: "Creation Time", comparisions: [0, 1, 2, 3, 4], filterType: 1 },
    { propertyName: 'userLevel', comparisions: [0], displayName: "Level", filterType: 3, dropdownData: this.userLevelParam },
    { propertyName: 'userType', comparisions: [0], displayName: "User type", filterType: 3, dropdownData: this.userTypeParam },
    { propertyName: 'branch', comparisions: [0], displayName: "Branch", filterType: 3, dropdownData: this.branchParam },
    { propertyName: 'isActive', comparisions: [0], displayName: "Active", filterType: 2 },


  ];
  users: UserDto[] = [];
  keyword = '';
  isActive: boolean | null;
  skillsParam: DropDownDataDto[] = []
  skill = ""
  advancedFiltersVisible = false;
  public userProjectHistory: IUSerProjectHistory[] = []
  Admin_Users_View = PERMISSIONS_CONSTANT.Admin_Users_View;
  Admin_Users_Create = PERMISSIONS_CONSTANT.Admin_Users_Create;
  Admin_Users_SyncDataFromHrm = PERMISSIONS_CONSTANT.Admin_Users_SyncDataFromHrm;
  Admin_Users_ViewProjectHistory = PERMISSIONS_CONSTANT.Admin_Users_ViewProjectHistory;
  Admin_Users_Edit = PERMISSIONS_CONSTANT.Admin_Users_Edit;
  Admin_Users_UpdateSkill = PERMISSIONS_CONSTANT.Admin_Users_UpdateSkill;
  Admin_Users_UpdateRole = PERMISSIONS_CONSTANT.Admin_Users_UpdateRole;
  Admin_Users_ActiveAndDeactive = PERMISSIONS_CONSTANT.Admin_Users_ActiveAndDeactive;
  Admin_Users_UploadAvatar = PERMISSIONS_CONSTANT.Admin_Users_UploadAvatar;
  Admin_Users_ResetPassword = PERMISSIONS_CONSTANT.Admin_Users_ResetPassword;
  Admin_Users_DeleteFakeUser = PERMISSIONS_CONSTANT.Admin_Users_DeleteFakeUser;

  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
    private _modalService: BsModalService,
    private userInfoService: UserService,
    private dialog: MatDialog,
    private skillService: SkillService
  ) {
    super(injector);
  }

  createUser(): void {
    this.showCreateOrEditUserDialog();
  }

  editUser(user: UserDto): void {
    this.showCreateOrEditUserDialog(user.id);
  }

  public resetPassword(user: UserDto): void {
    this.showResetPasswordUserDialog(user);
  }

  clearFilters(): void {
    this.keyword = '';
    this.isActive = undefined;
    this.getDataPage(1);
  }

  protected list(
    request: PagedUsersRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    let check = false
    request.keyword = this.keyword;
    request.isActive = this.isActive;
    this.isLoading = true
    let requestBody: any = request
    requestBody.skillIds = []
    requestBody.filterItems.forEach(item => {
      if (item.filterType == 4) {
        requestBody.filterItems = this.clearFilter(requestBody, "skill", 0)
        // this.skill = item.value
        requestBody.skillIds.push(item.value)
        check = true
      }
    })


    this.userInfoService
      .getUserPaging(
        requestBody
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: any) => {
        this.users = result.result.items;
        this.showPaging(result.result, pageNumber);
        this.isLoading = false
        if (check == true) {
          request.filterItems.push({ propertyName: 'skill', comparision: 0, value: this.skill, filterType: 4, dropdownData: this.skillsParam })
          // this.skill = ''
        }

      },
        () => {
          this.isLoading = false
        });
  }

  protected delete(user: UserDto): void {
    abp.message.confirm(
      this.l('UserDeleteWarningMessage', user.fullName),
      undefined,
      (result: boolean) => {
        if (result) {
          this._userService.delete(user.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }
  ngOnInit() {
    this.refresh()
    this.getAllSkills()
  }

  private showResetPasswordUserDialog(user?: UserDto): void {
    const showCreate = this.dialog.open(ResetPasswordDialogComponent, {
      data:user,
      width: "700px",
      disableClose: true,
    });
  }
  getAllSkills() {
    this.skillService.getAll().subscribe((data) => {
      this.listSkills = data.result;
      this.skillsParam = data.result.map(item => {
        return {
          displayName: item.name,
          value: item.id
        }
      })
      this.FILTER_CONFIG.push({ propertyName: 'skill', comparisions: [0], displayName: "Skill", filterType: 4, dropdownData: this.skillsParam },
      )
    })

  }

  private showCreateOrEditUserDialog(id?: number): void {
    if (!id) {
      const showCreate = this.dialog.open(CreateUserDialogComponent, {
        width: "700px",
        disableClose: true,
      });
      showCreate.afterClosed().subscribe(res => {
        if (res) {
          this.refresh()
        }
      })
    } else {
      const showEdit = this.dialog.open(EditUserDialogComponent, {
        data: {id: id},
        width: "700px",
        disableClose: true,
      });
      showEdit.afterClosed().subscribe(res => {
        if (res) {
          this.refresh()
        }
      })
    }
  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
  upLoadAvatar(user): void {
    let diaLogRef = this.dialog.open(UploadAvatarComponent, {
      width: '600px',
      data: user.id
    });
    diaLogRef.afterClosed().subscribe(res => {
      if (res) {
        this.userInfoService.uploadImageFile(res, user.id).pipe(catchError(this.userInfoService.handleError)).subscribe(data => {
          if (data) {
            this.notify.success('Upload Avatar Successfully!');
            this.refresh();
            if (this.appSession.user.id == user.id) {
              this.appSession.user.avatarFullPath = data.body.result;
            }
            user.avatarFullPath = AppConsts.remoteServiceBaseUrl + data.body.result;
            this.users.forEach(u => {
              if (u.managerId == user.id) {
                u.managerAvatarPath = user.avatarFullPath;
              }
            });

          } else { this.notify.error('Upload Avatar Failed!'); }
        });
      }
    });
  }
  updateDataHRM() {
    abp.message.confirm("Sync user data from HRM?",
      undefined,
      (result: boolean) => {
        if (result) {
          this.isLoading = true
          this.userInfoService.autoUpdateUserFromHRM().pipe(catchError(this.userInfoService.handleError)).subscribe((res) => {
            abp.notify.success("Updated user list!");
            this.refresh();
            this.isLoading = false
          },
            () => { this.isLoading = false })
        }
      }
    )
  }

  getHistoryProjectsByUserId(user) {
    this.userInfoService.getHistoryProjectsByUserId(user.id).pipe(catchError(this.userInfoService.handleError)).subscribe(data => {
      user.isshowProjectHistory = true
      let userHisTory = '';
      let count = 0;
      let listHistory = data.result;
      listHistory.forEach(project => {
        count++;
        if (count <= 6 || user.showAllHistory) {
          userHisTory +=
            `<div class="mb-1 d-flex pointer ${project.allowcatePercentage > 0 ? 'join-project' : 'out-project'}">
              <div class="col-11 p-0">
                  <p class="mb-0" >
                  <strong>${project.projectName}</strong>
                  <span class="badge ${this.APP_CONST.projectUserRole[project.projectRole]}">
                  ${this.getByEnum(project.projectRole, this.APP_ENUM.ProjectUserRole)}</span>
                  -  <span>${moment(project.startTime).format("DD/MM/YYYY")}</span></p>
              </div>
              <div class="col-1">
                  <span class="badge ${project.allowcatePercentage > 0 ? 'bg-success' : 'bg-secondary'}">${project.allowcatePercentage > 0 ? 'Join' : 'Out'} </span>
              </div>
          </div>
         `

        }
      });
      if (count > 6) {
        user.showMoreHistory = true
      } else {
        user.showMoreHistory = false;
      }
      user.userProjectHistory = userHisTory
    })
  }

  showMoreHistory(user) {
    user.showAllHistory = !user.showAllHistory;
  }

  updateUserActive(user: UserDto, isActive: boolean) {
    this.userInfoService.updateUserActive(user.id, isActive).pipe(catchError(this.userInfoService.handleError)).subscribe(rs => {
      if (isActive) {
        abp.notify.success(`Update user ${user.fullName} to Activate`)
      }
      else {
        abp.notify.success(`Update user ${user.fullName} to Inactivate`)
      }
      this.refresh()
    })
  }

  deleteFakeUser(user: UserDto) {
    abp.message.confirm(
      "Delete fake user " + user.fullName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.userInfoService.deleteFakeUser(user.id).pipe(catchError(this.userInfoService.handleError)).subscribe(rs => {
            abp.notify.success(`deleted fake user ${user.fullName}`)
            this.refresh()
          })
        }
      }
    )

  }
  updateUserSkill(user) {
    let ref = this.dialog.open(UpdateUserSkillDialogComponent, {
      width: "700px",
      data: user
    })
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh()
      }
    })
  }
  updateUserRole(user) {
    let ref = this.dialog.open(UpdateUserRoleComponent, {
      width: "700px",
      data: user
    })
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh()
      }
    })
  }
}
