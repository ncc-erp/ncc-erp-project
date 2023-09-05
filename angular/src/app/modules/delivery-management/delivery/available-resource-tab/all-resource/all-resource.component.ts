
import { ResourceManagerService } from './../../../../../service/api/resource-manager.service';
import { UserService } from './../../../../../service/api/user.service';
import { MatDialog } from '@angular/material/dialog';
import { SkillService } from './../../../../../service/api/skill.service';
import { DeliveryResourceRequestService } from './../../../../../service/api/delivery-request-resource.service';
import { availableResourceDto } from './../../../../../service/model/delivery-management.dto';
import { InputFilterDto } from './../../../../../../shared/filter/filter.component';
import { PlanResourceComponent } from './../plan-resource/plan-resource.component';
import { catchError, finalize, filter } from 'rxjs/operators';
import { PagedRequestDto } from './../../../../../../shared/paged-listing-component-base';
import { SkillDto } from './../../../../../service/model/list-project.dto';
import { PagedListingComponentBase } from '@shared/paged-listing-component-base';
import { PlanUserComponent } from './../plan-resource/plan-user/plan-user.component';
import { ProjectDetailComponent } from './../plan-resource/plan-user/project-detail/project-detail.component';
import { Component, OnInit, Injector, inject } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { UpdateUserSkillDialogComponent } from '@app/users/update-user-skill-dialog/update-user-skill-dialog.component';
import * as moment from 'moment';
import { Subscription } from 'rxjs';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { ConfirmPlanDialogComponent } from '../plan-resource/plan-user/confirm-plan-dialog/confirm-plan-dialog.component';
import { ConfirmFromPage } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/confirm-popup/confirm-popup.component';
import { BranchService } from '@app/service/api/branch.service';
import { PositionService } from '@app/service/api/position.service';
import { BranchDto } from '@app/service/model/branch.dto';
import { PositionDto } from '@app/service/model/position.dto';
import { RetroReviewHistoryByUserComponent } from "./../plan-resource/plan-user/retro-review-history-by-user/retro-review-history-by-user.component";
import { ProjectHistoryByUserComponent } from "./../plan-resource/plan-user/project-history-by-user/project-history-by-user.component";
import { result } from 'lodash-es';

@Component({
  selector: 'app-all-resource',
  templateUrl: './all-resource.component.html',
  styleUrls: ['./all-resource.component.css']
})
export class AllResourceComponent extends PagedListingComponentBase<any> implements OnInit {

  subscription: Subscription[] = [];
  public listSkills: SkillDto[] = [];
  public listBranchs: BranchDto[] = [];
  public listPositions: PositionDto[] = [];
  public skill = '';
  public skillsParam = [];
  public selectedSkillId: number[];
  public selectedBranchIds: number[] = [];
  public selectedUserTypes: number[] = [];
  public selectedPositions: number[] = [];
  public isAndCondition: boolean = false;
  public selectedIsPlanned: number;
  public listPlans: string[] = [];

  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View
  Resource_TabAllResource_View = PERMISSIONS_CONSTANT.Resource_TabAllResource_View
  Resource_TabAllResource_ViewHistory = PERMISSIONS_CONSTANT.Resource_TabAllResource_ViewHistory
  Resource_TabAllResource_CreatePlan = PERMISSIONS_CONSTANT.Resource_TabAllResource_CreatePlan
  Resource_TabAllResource_EditPlan = PERMISSIONS_CONSTANT.Resource_TabAllResource_EditPlan
  Resource_TabAllResource_ConfirmPickEmployeeFromPoolToProject = PERMISSIONS_CONSTANT.Resource_TabAllResource_ConfirmPickEmployeeFromPoolToProject
  Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = PERMISSIONS_CONSTANT.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther
  Resource_TabAllResource_ConfirmOut = PERMISSIONS_CONSTANT.Resource_TabAllResource_ConfirmOut
  Resource_TabAllResource_CancelMyPlan = PERMISSIONS_CONSTANT.Resource_TabAllResource_CancelMyPlan
  Resource_TabAllResource_CancelAnyPlan = PERMISSIONS_CONSTANT.Resource_TabAllResource_CancelAnyPlan
  Resource_TabAllResource_UpdateSkill = PERMISSIONS_CONSTANT.Resource_TabAllResource_UpdateSkill
  Resource_TabAllResource_ProjectDetail = PERMISSIONS_CONSTANT.Resource_TabAllResource_ProjectDetail

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function, skill?): void {
    this.isLoading = true;
    const requestBody: any = {
      ...request,
      skillIds: this.selectedSkillId,
      isAndCondition: this.isAndCondition,
      branchIds: this.selectedBranchIds,
      userTypes: this.selectedUserTypes,
      positionIds: this.selectedPositions,
      planStatus: this.selectedIsPlanned || PlanStatus.AllPlan
    };

    this.subscription.push(
      this.availableRerourceService.GetAllResource(requestBody)
        .pipe(
          catchError(this.availableRerourceService.handleError)
        )
        .subscribe(data => {
          this.availableResourceList = data.result.items;
          this.showPaging(data.result, pageNumber);
          this.isLoading = false;
        })
    );
  }


  protected delete(entity: PlanResourceComponent): void {
  }

  userTypeParam = Object.entries(this.APP_ENUM.UserTypeTabAllResource).map((item) => {
    return {
      displayName: item[0],
      value: item[1],
    };
  });


  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'fullName', comparisions: [0, 6, 7, 8], displayName: "User Name" },
    { propertyName: 'used', comparisions: [0, 1, 2, 3, 4], displayName: "Used" },
    { propertyName: 'userType', comparisions: [0], displayName: "User Type", filterType: 3, dropdownData: this.userTypeParam },
  ];

  public availableResourceList: availableResourceDto[] = [];

  constructor(public injector: Injector,
    private availableRerourceService: ResourceManagerService,
    private dialog: MatDialog,
    private skillService: SkillService,
    private _modalService: BsModalService,
    private userInfoService: UserService,
    private projectUserService: ProjectUserService,
    private branchService: BranchService,
    private positionService: PositionService,
  ) { super(injector) }

  ngOnInit(): void {
    this.pageSizeType = 100
    this.changePageSize();
    this.getAllSkills();
    this.getAllPositions();
    this.getAllBranchs();
    this.userTypeParam.forEach(item => {
      if (item.value != 4 && item.value != 5) {
        this.selectedUserTypes.push(item.value);
      }
    });

    this.selectedIsPlanned = 1;
  }
  showDialogPlanUser(command: string, user: any) {
    let item = {
      userId: user.userId,
      fullName: user.fullName,
      projectId: user.projectId,
      projectRole: user.projectRole,
      startTime: user.startTime,
      allocatePercentage: user.allocatePercentage,
      isPool: user.isPool,
      projectUserId: user.projectUserId
    };

    const show = this.dialog.open(PlanUserComponent, {
      width: '700px',
      disableClose: true,
      data: {
        item: item,
        command: command,
      },
    });
    show.afterClosed().subscribe((result) => {
      if (result) {
        this.refresh();
      }
    });
  }

  public isAllowCancelPlan(creatorUserId: number) {
    if (this.permission.isGranted(this.DeliveryManagement_ResourceRequest_CancelMyPlanOnly)) {
      if (this.permission.isGranted(this.DeliveryManagement_ResourceRequest_CancelAnyPlanResource)) {
        return true
      }
      else if (creatorUserId === this.appSession.userId) {
        return true
      }
      else {
        return false
      }
    }
  }
  planUser(user: any) {
    this.showDialogPlanUser("plan", user);
  }
  showUserDetail(userId: any) {

  }

  getAllSkills() {
    this.subscription.push(
      this.skillService.getAll().subscribe((data) => {
        this.listSkills = data.result;
        this.skillsParam = data.result.map(item => {
          return {
            displayName: item.name,
            value: item.id
          }
        })
      })
    )
  }

  onChangeBranchEvent(event?): void {
    this.selectedBranchIds = event.value;
    this.getDataPage(1);
    //this.refresh();
  }

  onChangeUserTypeEvent(event?): void {
    this.selectedUserTypes = event.value;
    this.getDataPage(1);
    //this.refresh();
  }

  planStatusList = [
    { value: PlanStatus.AllPlan, displayName: 'Has plans' },
    { value: PlanStatus.PlanningJoin, displayName: 'Planning join' },
    { value: PlanStatus.PlanningOut, displayName: 'Planning out' },
    { value: PlanStatus.NoPlan, displayName: 'No plan' }
  ];

  applyPlanFilter() {
    this.selectedIsPlanned = PlanStatus.All;
    this.isFilterSelected = false;
    this.getDataPage(1);
}

  getAllBranchs() {
    this.branchService.getAllNotPagging().subscribe((data) => {
      this.listBranchs = data.result
      this.selectedBranchIds = data.result.map(item => item.id)
      this.refresh();
    })
  }

  onChangePositionsEvent(event?): void {
    this.selectedPositions = event.value;
    this.getDataPage(1);
    //this.refresh();
  }

  getAllPositions() {
    this.positionService.getAllNotPagging().subscribe((data) => {
      this.listPositions = data.result
      this.selectedPositions = data.result.map(item => item.id)
      this.refresh();
    })
  }

  skillsCommas(arr) {
    arr = arr.map((item) => {
      return item.name;
    })
    return arr.join(', ')
  }
  projectsCommas(arr) {
    arr = arr.map((item) => {
      return item.projectName;
    })
    return arr.join(', ')
  }

  showProjectDetail(projectId, projectName) {
    const show = this.dialog.open(ProjectDetailComponent, {
      data: {
        projectId: projectId,
        projectName: projectName,
      },
      width: '95vw',
      height: '90vh',
    })
  }
  updateUserSkill(user) {
    let ref = this.dialog.open(UpdateUserSkillDialogComponent, {
      width: "700px",
      data: {
        userSkills: user.userSkills,
        id: user.userId,
        fullName: user.fullName
      }

    });
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh()
      }
    })
  }


  CancelResourcePlan(projectUser, userName: string) {
    abp.message.confirm(
      `Cancel plan to project [${projectUser.projectName}] for user [${userName}]?`,
      "",
      (result: boolean) => {
        if (result) {
          this.subscription.push(
            this.availableRerourceService.CancelAllResourcePlan(projectUser.id).pipe(catchError(this.availableRerourceService.handleError)).subscribe(rs => {
              this.refresh()
              abp.notify.success("Cancel plan for user")
            })
          )
        }
      }
    )
  }

  // getHistoryProjectsByUserId(user) {
  //   this.subscription.push(
  //     this.userInfoService.getHistoryProjectsByUserId(user.userId).pipe(catchError(this.userInfoService.handleError)).subscribe(data => {
  //       user.isshowProjectHistory = true;
  //       let userHisTory = '';
  //       let count = 0;
  //       let listHistory = data.result;
  //       listHistory.forEach(project => {
  //         count++;
  //         if (count <= 6 || user.showAllHistory) {
  //           userHisTory +=
  //             `<div class="mb-1 d-flex pointer ${project.allowcatePercentage > 0 ? 'join-project' : 'out-project'}">
  //               <div class="col-11 p-0">
  //                   <p class="mb-0" >
  //                   <strong
  //                   *ngIf="permission.isGranted(Resource_TabAllResource_ProjectDetail)"
  //                   (click)="${this.viewProjectDetail(project)}">
  //                   ${project.projectName}
  //                   </strong>
  //                   <span class="badge ${this.APP_CONST.projectUserRole[project.projectRole]}">
  //                   ${this.getByEnum(project.projectRole, this.APP_ENUM.ProjectUserRole)}</span>
  //                   -  <span>${moment(project.startTime).format("DD/MM/YYYY")}</span></p>
  //               </div>
  //               <div class="col-1">
  //                   <span class="badge ${project.allowcatePercentage > 0 ? 'bg-success' : 'bg-secondary'}">${project.allowcatePercentage > 0 ? 'Join' : 'Out'} </span>
  //               </div>
  //           </div>
  //          `;
  //         }
  //       });

  //       if (count > 6) {
  //         user.showMoreHistory = true;
  //       } else {
  //         user.showMoreHistory = false;
  //       }

  //       user.userProjectHistory = userHisTory;

  //     })
  //   );
  // }

  getHistoryProjectsByUserId(user) {
    this.subscription.push(
      this.userInfoService.getHistoryProjectsByUserId(user.userId).pipe(catchError(this.userInfoService.handleError)).subscribe(data => {
        user.isshowProjectHistory = true;
        let count = 0;
        let listHistory = data.result;
        user.userProjectHistory = listHistory.map(project => {
          count++;
          if (count <= 6 || user.showAllHistory) {
            return {
              projectName: project.projectName,
              projectRole: project.projectRole,
              startTime: moment(project.startTime).format("DD/MM/YYYY"),
              allowcatePercentage: project.allowcatePercentage > 0,
              projectId: project.projectId,
              projectType: project.projectType
            };
          }
        }).filter(project => project !== undefined);

        if (count > 6) {
          user.showMoreHistory = true;
        } else {
          user.showMoreHistory = false;
        }
      })
    );
  }



  showMoreHistory(user) {
    user.showAllHistory = !user.showAllHistory;
  }
  ngOnDestroy(): void {
    this.subscription.forEach(sub => sub.unsubscribe())
  }

  confirm(plan, userId, userName) {
    // if (user.allocatePercentage <= 0) {
    //   let ref = this.dialog.open(ReleaseUserDialogComponent, {
    //     width: "700px",
    //     data: {
    //       user: user,
    //       type: "confirmOut",
    //     },
    //   })
    //   ref.afterClosed().subscribe(rs => {
    //     if (rs) {
    //       this.refresh()
    //     }
    //   })
    // }
    // else if (user.allocatePercentage > 0) {

    plan.userId = userId
    plan.fullName = userName
    this.projectUserService.GetAllWorkingProjectByUserId(userId).subscribe(data => {
      let ref = this.dialog.open(ConfirmPlanDialogComponent, {
        width: '580px',
        data: {
          workingProject: data.result,
          user: plan,
          fromPage: ConfirmFromPage.allResource
        }
      })

      ref.afterClosed().subscribe(rs => {
        if (rs) {
          this.refresh()
        }
      })
    })
    // }
  }
  editUserPlan(user: any, projectUser: any) {
    user.userId = projectUser.userId
    user.projectUserId = user.id
    user.fullName = projectUser.fullName
    this.showDialogPlanUser('edit', user);
  }

  viewProjectDetail(project) {

    let routingToUrl: string = ''

    if( project.projectType == 5 ){
      routingToUrl = (this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport)
      && this.permission.isGranted(this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View))
     ? "/app/training-project-detail/training-weekly-report" : "/app/training-project-detail/training-project-general"
    } 

    else if ( project.projectType == 3){
      routingToUrl= (this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport)
     && this.permission.isGranted(this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View))
    ? "/app/product-project-detail/product-weekly-report" : "/app/product-project-detail/product-project-general"
    }

    else {
      routingToUrl = (this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport)
      && this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View))
     ? "/app/list-project-detail/weeklyreport" : "/app/list-project-detail/list-project-general"
    }
    const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], {
      queryParams: {
        id: project.projectId,
        type: project.projectType,
        projectName: project.projectName,
        projectCode: project.projectCode
      }
    }));
    window.open(url, '_blank');
  }

  showDialogProjectHistoryUser(user: availableResourceDto) {
    let userInfo = {
      userId: user.userId,
      emailAddress: user.emailAddress,
    };

    const show = this.dialog.open(ProjectHistoryByUserComponent, {
      width: '800px',
      disableClose: true,
      data: {
        item: userInfo,
      },
    });
    show.afterClosed().subscribe((result) => {
      if (result) {
        this.refresh();
      }
    });
  }
  projectHistorUser(user: availableResourceDto) {
    this.showDialogProjectHistoryUser(user);
  }
  showDialogRetroReviewHistoryUser(user: availableResourceDto) {
    let userInfo = {
      userId: user.userId,
      emailAddress: user.emailAddress,
    };

    const show = this.dialog.open(RetroReviewHistoryByUserComponent, {
      width: '1200px',
      disableClose: true,
      data: {
        item: userInfo,
      },
    });
    show.afterClosed().subscribe((result) => {
      if (result) {
        this.refresh();
      }
    });
  }
  RetroReviewHistoryUser(user: availableResourceDto) {
    this.showDialogRetroReviewHistoryUser(user);
  }

}
export enum PlanStatus {
  All = 1,
  AllPlan = 2,
  PlanningJoin = 3,
  PlanningOut = 4,
  NoPlan = 5
}
