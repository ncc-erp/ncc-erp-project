import { RetroReviewHistoryByUserComponent } from './plan-user/retro-review-history-by-user/retro-review-history-by-user.component';
import { RetroReviewInternHistoriesDto } from './../../../../../service/model/resource-plan.dto';
import { ResourceManagerService } from './../../../../../service/api/resource-manager.service';
import { AddUserToTempProjectDialogComponent } from './add-user-to-temp-project-dialog/add-user-to-temp-project-dialog.component';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { AddNoteDialogComponent } from './add-note-dialog/add-note-dialog.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { ProjectDetailComponent } from './plan-user/project-detail/project-detail.component';
import { SkillService } from './../../../../../service/api/skill.service';
import { InputFilterDto } from './../../../../../../shared/filter/filter.component';
import { DeliveryResourceRequestService } from './../../../../../service/api/delivery-request-resource.service';
import { availableResourceDto, ReviewInternRetroHisotyDto } from './../../../../../service/model/delivery-management.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { PlanUserComponent } from './plan-user/plan-user.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { result } from 'lodash-es';
import { catchError, finalize, filter } from 'rxjs/operators';
// import { DeliveryResourceRequestService } from './../../../../service/api/delivery-request-resource.service';
// import { availableResourceDto } from './../../../../service/model/delivery-management.dto';
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { SkillDto } from '@app/service/model/list-project.dto';
import { ProjectResourceRequestService } from '@app/service/api/project-resource-request.service';
import { ProjectHistoryByUserComponent } from './plan-user/project-history-by-user/project-history-by-user.component';
import * as moment from 'moment';
import { UpdateUserSkillDialogComponent } from '@app/users/update-user-skill-dialog/update-user-skill-dialog.component';
import { Subscription } from 'rxjs';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ReleaseUserDialogComponent } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/release-user-dialog/release-user-dialog.component';
import { ConfirmPlanDialogComponent } from './plan-user/confirm-plan-dialog/confirm-plan-dialog.component';
import { ConfirmFromPage } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/confirm-popup/confirm-popup.component';
import { BranchService } from '@app/service/api/branch.service';
@Component({
  selector: 'app-plan-resource',
  templateUrl: './plan-resource.component.html',
  styleUrls: ['./plan-resource.component.css'],
})
export class PlanResourceComponent
  extends PagedListingComponentBase<PlanResourceComponent>
  implements OnInit {
  public listSkills: SkillDto[] = [];
  public skill = '';
  public skillsParam = [];
  private subscription: Subscription[] = [];
  public selectedSkillId: number[]
  public isAndCondition: boolean = false

  Resource_TabPool = PERMISSIONS_CONSTANT.Resource_TabPool
  Resource_TabPool_View = PERMISSIONS_CONSTANT.Resource_TabPool_View
  Resource_TabPool_ViewHistory = PERMISSIONS_CONSTANT.Resource_TabPool_ViewHistory
  Resource_TabPool_CreatePlan = PERMISSIONS_CONSTANT.Resource_TabPool_CreatePlan
  Resource_TabPool_EditPlan = PERMISSIONS_CONSTANT.Resource_TabPool_EditPlan
  Resource_TabPool_AddTempProject = PERMISSIONS_CONSTANT.Resource_TabPool_AddTempProject
  Resource_TabPool_ConfirmPickEmployeeFromPoolToProject = PERMISSIONS_CONSTANT.Resource_TabPool_ConfirmPickEmployeeFromPoolToProject
  Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther = PERMISSIONS_CONSTANT.Resource_TabPool_ConfirmMoveEmployeeWorkingOnAProjectToOther
  Resource_TabPool_ConfirmOut = PERMISSIONS_CONSTANT.Resource_TabPool_ConfirmOut
  Resource_TabPool_CancelMyPlan = PERMISSIONS_CONSTANT.Resource_TabPool_CancelMyPlan
  Resource_TabPool_CancelAnyPlan = PERMISSIONS_CONSTANT.Resource_TabPool_CancelAnyPlan
  Resource_TabPool_EditTempProject = PERMISSIONS_CONSTANT.Resource_TabPool_EditTempProject
  Resource_TabPool_Release = PERMISSIONS_CONSTANT.Resource_TabPool_Release
  Resource_TabPool_UpdateSkill = PERMISSIONS_CONSTANT.Resource_TabPool_UpdateSkill
  Resource_TabPool_EditNote = PERMISSIONS_CONSTANT.Resource_TabPool_EditNote
  Resource_TabPool_ProjectDetail = PERMISSIONS_CONSTANT.Resource_TabPool_ProjectDetail
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
  Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport
  Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View
  
  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function, skill?

  ): void {
    this.isLoading = true;
    let requestBody: any = request
    requestBody.skillIds = this.selectedSkillId
    requestBody.isAndCondition = this.isAndCondition
    this.subscription.push(
      this.availableRerourceService
        .GetAllPoolResource(requestBody, this.skill)
        .pipe(
          finalize(() => {
            finishedCallback();
          }),
          catchError(this.availableRerourceService.handleError)
        )
        .subscribe((data) => {
          this.availableResourceList = data.result.items.filter((item) => {
            if (item.userType !== 4) {
              return item;
            }
          });
          this.showPaging(data.result, pageNumber);
          this.isLoading = false;
          let listEmail = this.availableResourceList.map(rs => rs.emailAddress)
          this.GetRetroAndReviewInternHistories(listEmail)
        })
    )
  }
  protected delete(entity: PlanResourceComponent): void { }
  userTypeParam = Object.entries(this.APP_ENUM.UserType).map((item) => {
    return {
      displayName: item[0],
      value: item[1],
    };
  });
  branchParam = Object.entries(this.APP_ENUM.UserBranch).map((item) => {
    return {
      displayName: item[0],
      value: item[1],
    };
  });

  public readonly FILTER_CONFIG: InputFilterDto[] = [
    {
      propertyName: 'fullName',
      comparisions: [0, 6, 7, 8],
      displayName: 'User Name',
    },
    {
      propertyName: 'userType',
      comparisions: [0],
      displayName: 'User Type',
      filterType: 3,
      dropdownData: this.userTypeParam,
    },
  ];

  public availableResourceList: availableResourceDto[] = [];
  private reviewInternRetroHisoties: ReviewInternRetroHisotyDto[] = []

  constructor(
    public injector: Injector,
    private _modalService: BsModalService,
    private availableRerourceService: ResourceManagerService,
    private dialog: MatDialog,
    private skillService: SkillService,
    private projectUserService: ProjectUserService,
    private branchService: BranchService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.pageSizeType = 100;
    this.changePageSize();
    this.getAllSkills();
    this.getAllBranchs();
  }

  getAllBranchs() {
    this.branchService.getAllNotPagging().subscribe((data) => {
      this.branchParam = data.result.map(item => {
        return {
          displayName: item.displayName,
          value: item.id
        }
      })
      this.FILTER_CONFIG.push({ propertyName: 'branchId', comparisions: [0], displayName: "Branch", filterType: 3, dropdownData: this.branchParam },
      )
    })
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

  // public long ProjectId { get; set; }
  // public long UserId { get; set; }
  // public byte PercentUsage { get; set; }
  // public ProjectUserRole ProjectRole { get; set; }
  // public DateTime StartTime { get; set; }
  // public bool IsExpense { get; set; }
  // public string Note { get; set; }
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
  showDialogProjectHistoryUser(user: availableResourceDto) {
    let userInfo = {
      userId: user.userId,
      emailAddress: user.emailAddress,
    };

    const show = this.dialog.open(ProjectHistoryByUserComponent, {
      width: '700px',
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
  planUser(user: any,) {
    this.showDialogPlanUser('plan', user);
  }
  editUserPlan(user: any, projectUser:any) {
    user.userId = projectUser.userId
    user.projectUserId = user.id 
    user.fullName = projectUser.fullName
    this.showDialogPlanUser('edit', user);
  }
  showUserDetail(userId: any) { }

  getAllSkills() {
    this.subscription.push(
      this.skillService.getAll().subscribe((data) => {
        this.listSkills = data.result;
        this.skillsParam = data.result.map((item) => {
          return {
            displayName: item.name,
            value: item.id,
          };
        });
      }))
  }

  skillsCommas(arr) {
    arr = arr.map((item) => {
      return item.name;
    });
    return arr.join(', ');
  }

  projectsCommas(arr) {
    arr = arr.map((item) => {
      return item.projectName;
    });
    return arr.join(', ');
  }

  showProjectDetail(projectId, projectName) {
    const show = this.dialog.open(ProjectDetailComponent, {
      data: {
        projectId: projectId,
        projectName: projectName,
      },
      width: '95vw',
      height: '90vh',
    });
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

  formatDateStartPool(date: string) {
    return moment(date).format('DD/MM/YYYY');
  }

  updateNote(id, fullName) {
    let addOrEditNoteDialog: BsModalRef;
    addOrEditNoteDialog = this._modalService.show(AddNoteDialogComponent, {
      class: 'modal',
      initialState: {
        id: id,
        fullName: fullName,
      },
    });

    addOrEditNoteDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }

  CancelResourcePlan(projectUser, userName: string) {
    abp.message.confirm(
      `Cancel plan to project <strong>${projectUser.projectName}</strong> for user <strong>${userName}</strong>?`,
      "",
      (result: boolean) => {
        if (result) {
          this.subscription.push(
            this.availableRerourceService.CancelPoolResourcePlan(projectUser.id).pipe(catchError(this.availableRerourceService.handleError)).subscribe(rs => {
              this.refresh()
              abp.notify.success("Cancel plan for user")
            })
          )

        }
      }
      , {isHtml:true}
    )
  }
  ngOnDestroy(): void {
    this.subscription.forEach(sub => {
      sub.unsubscribe()
    })
  }
  releaseUser(user, name) {
    user.fullName = name
    let ref = this.dialog.open(ReleaseUserDialogComponent, {
      width: "700px",
      data: {
        user: user,
        action: "Release"
        
      }
    })
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh();
      }
    })
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
          fromPage: ConfirmFromPage.poolResource,
          
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

  cancelResourcePlan(user) {
    abp.message.confirm(
      `Cancel plan for user <strong>${user.fullName}</strong> <strong class = "${user.allocatePercentage > 0 ? 'text-success' : 'text-danger'}">
      ${user.allocatePercentage > 0 ? 'Join project' : 'Out project'}</strong>?`,
      "",
      (result: boolean) => {
        if (result) {
          this.reportService.CancelResourcePlan(user.id).subscribe(rs => {
            abp.notify.success(`Cancel plan for user ${user.fullName}`)
            this.refresh()

          })
        }
      }, {isHtml:true}
     
    )
  }

  saveResourcePlan(projectUser) {
    projectUser.projectId = this.projectId
    this.reportService.EditProjectUserPlan(projectUser).subscribe(rs => {
      abp.notify.success(`Edited plan for user ${projectUser.fullName}`)
      this.refresh()
    })
  }




  editResourcePlan(projectUser) {
    // projectUser.editMode = true
    // this.isEditPlannedResource = true;
    // this.planResourceProcess = true
  }


  addToTempProject(projectUser, plan?) {
    if(plan){
      projectUser.project = plan
    }
    let ref = this.dialog.open(AddUserToTempProjectDialogComponent,
      {
        width: "700px",
        data: projectUser
      })
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh()
      }
    })
  }
  public GetRetroAndReviewInternHistories(emails: string[]) {
    this.isLoading = true
    this.availableRerourceService.GetTimesheetOfRetroReviewInternHistories({ emails: emails} as RetroReviewInternHistoriesDto).subscribe(rs => {
      this.reviewInternRetroHisoties = rs.result
      this.availableResourceList.forEach(x => {
          x.avgPoint = this.reviewInternRetroHisoties.find(s => s.email == x.emailAddress)?.averagePoint
      })
      this.isLoading = false;
    }, error => {this.isLoading = false})
  }

  viewProjectDetail(project){
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
}
