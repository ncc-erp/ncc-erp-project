import { ResourceManagerService } from './../../../../../service/api/resource-manager.service';
import { PagedRequestDto } from './../../../../../../shared/paged-listing-component-base';
import { SkillDto } from './../../../../../service/model/list-project.dto';
import { PlanResourceComponent } from './../plan-resource/plan-resource.component';
import { InputFilterDto } from './../../../../../../shared/filter/filter.component';
import { ProjectHistoryByUserComponent } from './../plan-resource/plan-user/project-history-by-user/project-history-by-user.component';
import { availableResourceDto } from './../../../../../service/model/delivery-management.dto';
import { ProjectDetailComponent } from './../plan-resource/plan-user/project-detail/project-detail.component';
import { UpdateUserSkillDialogComponent } from './../../../../../users/update-user-skill-dialog/update-user-skill-dialog.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, Injector, OnInit } from '@angular/core';
import { PlanUserComponent } from '../plan-resource/plan-user/plan-user.component';
import { DeliveryResourceRequestService } from '../../../../../service/api/delivery-request-resource.service';
import { MatDialog } from '@angular/material/dialog';
import { catchError, finalize } from 'rxjs/operators';
import { PagedListingComponentBase } from '../../../../../../shared/paged-listing-component-base';
import { AddNoteDialogComponent } from '../plan-resource/add-note-dialog/add-note-dialog.component';
import { SkillService } from '../../../../../service/api/skill.service';
import * as moment from 'moment';
import { UserService } from '@app/service/api/user.service';
import { Subscription } from 'rxjs';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ConfirmPlanDialogComponent } from '../plan-resource/plan-user/confirm-plan-dialog/confirm-plan-dialog.component';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { ConfirmFromPage } from '@app/modules/pm-management/list-project/list-project-detail/resource-management/confirm-popup/confirm-popup.component';
import { BranchService } from '@app/service/api/branch.service';
@Component({
  selector: 'app-vendor',
  templateUrl: './vendor.component.html',
  styleUrls: ['./vendor.component.css']
})
export class VendorComponent extends PagedListingComponentBase<PlanResourceComponent> implements OnInit {
 
  subscription: Subscription[] = [];
  public listSkills: SkillDto[] = [];
  public skill = '';
  public skillsParam = [];
  public selectedSkillId:number[]
  public isAndCondition:boolean =false;
  Resource_TabVendor_View = PERMISSIONS_CONSTANT.Resource_TabVendor_View
  Resource_TabVendor_ViewHistory = PERMISSIONS_CONSTANT.Resource_TabVendor_ViewHistory
  Resource_TabVendor_CreatePlan = PERMISSIONS_CONSTANT.Resource_TabVendor_CreatePlan
  Resource_TabVendor_EditPlan = PERMISSIONS_CONSTANT.Resource_TabVendor_EditPlan
  Resource_TabVendor_ConfirmPickEmployeeFromPoolToProject = PERMISSIONS_CONSTANT.Resource_TabVendor_ConfirmPickEmployeeFromPoolToProject
  Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther = PERMISSIONS_CONSTANT.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther
  Resource_TabVendor_ConfirmOut = PERMISSIONS_CONSTANT.Resource_TabVendor_ConfirmOut
  Resource_TabVendor_CancelMyPlan = PERMISSIONS_CONSTANT.Resource_TabVendor_CancelMyPlan
  Resource_TabVendor_CancelAnyPlan = PERMISSIONS_CONSTANT.Resource_TabVendor_CancelAnyPlan
  Resource_TabVendor_UpdateSkill = PERMISSIONS_CONSTANT.Resource_TabVendor_UpdateSkill
  Resource_TabVendor_ProjectDetail = PERMISSIONS_CONSTANT.Resource_TabVendor_ProjectDetail

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function, skill?): void {
    this.isLoading = true;
    let requestBody:any = request
    requestBody.skillIds = this.selectedSkillId
    requestBody.isAndCondition = this.isAndCondition
    this.subscription.push(
      this.availableRerourceService.GetVendorResource(requestBody).pipe(finalize(() => {
        finishedCallback();
      }), catchError(this.availableRerourceService.handleError)).subscribe(data => {
        this.availableResourceList = data.result.items.filter((item) => {
          if (item.userType !== 4) {
            return item;
          }
        });
        this.showPaging(data.result, pageNumber);
        this.isLoading = false;
      })
    )

  }
  protected delete(entity: PlanResourceComponent): void {

  }
  userTypeParam = Object.entries(this.APP_ENUM.UserType).map((item) => {
    return {
      displayName: item[0],
      value: item[1]
    }

  })
  branchParam = Object.entries(this.APP_ENUM.UserBranch).map((item) => {
    return {
      displayName: item[0],
      value: item[1]
    }
  })

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
    private branchService: BranchService


  ) { super(injector) }

  ngOnInit(): void {
    this.pageSizeType = 100
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
      if(this.permission.isGranted(this.DeliveryManagement_ResourceRequest_CancelAnyPlanResource)){
        return true
      }
      else if (creatorUserId === this.appSession.userId) {
        return true
      }
      else{
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
            this.availableRerourceService.CancelVendorResourcPlan(projectUser.id).pipe(catchError(this.availableRerourceService.handleError)).subscribe(rs => {
              this.refresh()
              abp.notify.success("Cancel plan for user")
            })
          )
        }
      }
    )
  }

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
          fromPage: ConfirmFromPage.vendor,
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
  editUserPlan(user: any, projectUser:any) {
    user.userId = projectUser.userId
    user.projectUserId = user.id 
    user.fullName = projectUser.fullName
    this.showDialogPlanUser('edit', user);
  }

  viewProjectDetail(project){
    let routingToUrl:string = (
       this.permission.isGranted(this.Resource_TabVendor_ProjectDetail)
   
     )
    ? "/app/training-project-detail/training-weekly-report" : "/app/training-project-detail/training-project-general"
    ? "/app/product-project-detail/product-weekly-report" : "/app/product-project-detail/product-project-general"
    ? "/app/list-project-detail/weeklyreport" : "/app/list-project-detail/list-project-general"
    const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], { queryParams: {
      id: project.projectId,
      type: project.projectType, 
      projectName: project.projectName, 
      projectCode:" "} }));
    window.open(url, '_blank');
  }
}
