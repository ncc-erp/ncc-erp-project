
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
import { Component, OnInit, Injector, inject, ViewChild } from '@angular/core';
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
import { APP_ENUMS } from '@shared/AppEnums';
import { AddNoteDialogComponent } from '../plan-resource/add-note-dialog/add-note-dialog.component';

@Component({
  selector: 'app-all-resource',
  templateUrl: './all-resource.component.html',
  styleUrls: ['./all-resource.component.css']
})
export class AllResourceComponent extends PagedListingComponentBase<any> implements OnInit {

  subscription: Subscription[] = [];
  public searchSkill:string = '';
  public searchBranch:string = '';
  public searchPosition:string ='';
  public searchUserType:string ='';
  public listSkills: SkillDto[] = [];
  public listBranchs: BranchDto[] = [];
  public listPositions: PositionDto[] = [];
  public listUserTypes: any = [];
  public listSkillsId: number[] = [];
  public listBranchsId: number[] = [];
  public listPositionsId: number[] = [];
  public listUserTypesId: number[] = [];
  public skill = '';
  public skillsParam = [];
  public selectedSkillId: number[] = [];
  public selectedSkillIdCr: number[] = [];
  public selectedSkillIdOld: number[]= [];
  public selectedBranchIds: number[] = [];
  public selectedBranchIdsCr: number[] = [];
  public selectedBranchIdsOld: number[] = [];
  public selectedUserTypes: number[] = [];
  public selectedUserTypesCr: number[] = [];
  public selectedUserTypesOld: number[] = [];
  public selectedPositions: number[] = [];
  public selectedPositionsCr: number[] = [];
  public selectedPositionsOld: number[] = [];
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
  Resource_TabAllResource_EditNote = PERMISSIONS_CONSTANT.Resource_TabAllResource_EditNote;
  Resource_TabAllResource_ViewUserStarSkill = PERMISSIONS_CONSTANT.Resource_TabAllResource_ViewUserStarSkill
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
      planStatus: this.selectedIsPlanned || APP_ENUMS.PlanStatus.AllPlan
    };

    this.subscription.push(
      this.availableRerourceService.GetAllResource(requestBody)
        .pipe(
          catchError(this.availableRerourceService.handleError)
        )
        .subscribe(data => {
          this.availableResourceList = data.result.items;
          this.availableResourceList.forEach(item => item.isViewAll = false);
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

  @ViewChild("selectPosition") selectPosition;
  @ViewChild("selectBranch") selectBranch;
  @ViewChild("selectSkill") selectSkill;
  @ViewChild("selectUserType") selectUserType;

  ngOnInit(): void {
    this.pageSizeType = 100
    this.changePageSize();
    this.getAllSkills();
    this.getAllPositions();
    this.getAllBranchs();
    this.getAllUserTypes();
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

  openedChange(opened,typeSelect){
    if(!opened){
      switch(typeSelect){
        case 'Branch':
          this.selectedBranchIds = [...this.selectedBranchIdsOld]
          this.selectedBranchIdsCr = [...this.selectedBranchIdsOld]
          this.searchBranch = '';
          break;
        case 'Position':
          this.selectedPositions = [...this.selectedPositionsOld]
          this.selectedPositionsCr = [...this.selectedPositionsOld]
          this.searchPosition = '';
          break;
        case 'Skill':
          this.selectedSkillId = [...this.selectedSkillIdOld]
          this.selectedSkillIdCr = [...this.selectedSkillIdOld]
          this.searchSkill = '';
          break;
        case 'UserType':
          this.selectedUserTypes = [...this.selectedUserTypesOld]
          this.selectedUserTypesCr = [...this.selectedUserTypesOld]
          this.searchUserType = '';
          break;
      }
    }
  }

  actionSelect(typeSelect){
    switch(typeSelect.type){
      case 'Branch':
        this.selectedBranchIds = typeSelect.data
        this.selectedBranchIdsCr = typeSelect.data
        break;
      case 'Position':
        this.selectedPositions = typeSelect.data
        this.selectedPositionsCr = typeSelect.data
        break;
      case 'Skill':
        this.selectedSkillId = typeSelect.data
        this.selectedSkillIdCr = typeSelect.data
        break;
      case 'UserType':
        this.selectedUserTypes = typeSelect.data
        this.selectedUserTypesCr = typeSelect.data
        break;
    }
  }

  onSelectChange(listSelect,id){
    if(listSelect.includes(id)){
      return listSelect.filter(res => res != id)
    }
    else{
      listSelect.push(id)
      return listSelect
    }
  }

  onSelectChangePosition(id){
    const position = this.onSelectChange(this.selectedPositionsCr,id)
    this.selectedPositionsCr = position
    this.selectedPositions = [...position]
    this.listPositions = this.orderList(this.listPositions,this.selectedPositions)
  }

  onSelectChangeBranch(id){
    const branch = this.onSelectChange(this.selectedBranchIdsCr,id)
    this.selectedBranchIdsCr = branch
    this.selectedBranchIds = [...branch]
    this.listBranchs = this.orderList(this.listBranchs,this.selectedBranchIds)
  }

  onSelectChangeSkill(id){
    const skill = this.onSelectChange(this.selectedSkillIdCr,id)
    this.selectedSkillIdCr = skill
    this.selectedSkillId= [...skill]
    this.listSkills = this.orderList(this.listSkills,this.selectedSkillId)
  }

  onSelectChangeUserType(id){
    const userType = this.onSelectChange(this.selectedUserTypesCr,id)
    this.selectedUserTypesCr = userType
    this.selectedUserTypes = [...userType]
    this.listUserTypes = this.orderList(this.listUserTypes,this.selectedUserTypes)
  }

  orderList(listAll, listIdSelect){
    const listSelect = listAll.filter(item => listIdSelect.includes(item.id))
    const listUnSelect = listAll.filter(item => !listIdSelect.includes(item.id))
    return [...listSelect, ...listUnSelect]
  }

  selectDone(type){
    switch(type){
      case 'Branch':
        this.selectedBranchIdsOld = this.selectedBranchIds
        this.selectBranch.close()
        break;
      case 'Position':
        this.selectedPositionsOld = this.selectedPositions
        this.selectPosition.close()
        break;
      case 'Skill':
        this.selectedSkillIdOld = this.selectedSkillId
        this.selectSkill.close()
        break;
      case 'UserType':
        this.selectedUserTypesOld = this.selectedUserTypes
        this.selectUserType.close()
        break;
    }
    this.refresh()
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
        this.listSkillsId = data.result.map(item => item.id)
        this.skillsParam = data.result.map(item => {
          return {
            displayName: item.name,
            value: item.id
          }
        })
      })
    )
  }

  planStatusList = [
    { value: APP_ENUMS.PlanStatus.AllPlan, displayName: 'Has plans' },
    { value: APP_ENUMS.PlanStatus.PlanningJoin, displayName: 'Planning join' },
    { value: APP_ENUMS.PlanStatus.PlanningOut, displayName: 'Planning out' },
    { value: APP_ENUMS.PlanStatus.NoPlan, displayName: 'No plan' }
  ];

  applyPlanFilter() {
    this.selectedIsPlanned = APP_ENUMS.PlanStatus.All;
    this.isFilterSelected = false;
    this.getDataPage(1);
}

  getAllBranchs() {
    this.branchService.getAllNotPagging().subscribe((data) => {
      this.listBranchs = data.result
      this.listBranchsId = data.result.map(item => item.id)
      this.selectedBranchIds = data.result.map(item => item.id)
      this.selectedBranchIdsOld = [...this.selectedBranchIds]
      this.selectedBranchIdsCr = this.selectedBranchIds
      //this.refresh();
    })
  }

  getAllPositions() {
    this.positionService.getAllNotPagging().subscribe((data) => {
      this.listPositions = data.result
      this.listPositionsId = data.result.map(item => item.id)
      this.selectedPositions = data.result.map(item => item.id)
      this.selectedPositionsOld = [...this.selectedPositions]
      this.selectedPositionsCr = this.selectedPositions
      //this.refresh();
    })
  }

  getAllUserTypes() {
    this.listUserTypes = Object.entries(this.APP_ENUM.UserTypeTabAllResource).map((item) => {
      return {
        displayName: item[0],
        value: item[1],
      };
    });
    this.listUserTypesId = this.listUserTypes.map(item => item.value);
    this.selectedUserTypes = this.listUserTypes.map(item => item.value);
    this.selectedUserTypesOld = [...this.selectedUserTypes];
    this.selectedUserTypesCr = this.selectedUserTypes;
    //this.refresh();
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
  updateUserSkill(user, note) {
    let ref = this.dialog.open(UpdateUserSkillDialogComponent, {
      width: "700px",
      data: {
        userSkills: user.userSkills,
        id: user.userId,
        fullName: user.fullName,
        note: note,
        viewStarSkillUser: this.permission.isGranted(this.Resource_TabAllResource_ViewUserStarSkill),
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
      width: '1200px',
      disableClose: true,
      data: {
        item: userInfo,
      },
    });
    /*show.afterClosed().subscribe((result) => {
      if (result) {
        this.refresh();
      }
    });*/
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
    /*show.afterClosed().subscribe((result) => {
      if (result) {
        this.refresh();
      }
    });*/
  }
  RetroReviewHistoryUser(user: availableResourceDto) {
    this.showDialogRetroReviewHistoryUser(user);
  }

  updateNote(user: availableResourceDto) {
    const addOrEditNoteDialog = this.dialog.open(AddNoteDialogComponent, {
      width: "40%",
      data: {
        userId: user.userId,
        fullName: user.fullName,
        poolNote: user.poolNote
      },
    });

    addOrEditNoteDialog.afterClosed().subscribe((data) => {
      const item = this.availableResourceList.find(item => item.userId == data.userId);
      if (item != null){
        item.poolNote = data.note;
      }
    });
  }
  toggle(item){    
    item.isViewAll = !item.isViewAll
  }

}
