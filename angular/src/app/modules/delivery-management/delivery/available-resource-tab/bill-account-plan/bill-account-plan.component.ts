import { BillAccountDialogNoteComponent } from './bill-account-dialog-note/bill-account-dialog-note.component';
import { Component, Injector, OnInit } from "@angular/core";
import * as _moment from "moment";
import { APP_ENUMS } from "@shared/AppEnums";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { catchError } from "rxjs/operators";
import { PlanningBillInfoService } from "../../../../../service/api/bill-account-plan.service";
import { THeadTable } from "../../request-resource-tab/request-resource-tab.component";
import { MatDialog } from "@angular/material/dialog";
import { HandleLinkedResourcesDialogComponent } from './handle-linked-resources-dialog/handle-linked-resources-dialog.component';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { UpdateUserSkillDialogComponent } from '@app/users/update-user-skill-dialog/update-user-skill-dialog.component';
import { IBillInfo, IProject } from '@app/service/model/bill-info.interface';
import { AppConsts } from '@shared/AppConsts';
import { UploadCvBillAccountComponent } from '@shared/components/upload-cv-bill-account/upload-cv-bill-account.component';
import { GetCvBillAccountDto } from '@app/service/model/upload-cv.dto';
import { FileHandlerService } from '@app/service/utility/file-handler.service';

@Component({
  selector: "app-bill-account-plan",
  templateUrl: "./bill-account-plan.component.html",
  styleUrls: ["./bill-account-plan.component.css"],
})
export class BillAccountPlanComponent
  extends PagedListingComponentBase<any>
  implements OnInit
{
  public searchProject: string = "";
  public searchClient: string = "";
  public projectStatus;
  public isCharge: boolean = true;
  public isExpose: boolean = true;
  public isShowLevel: boolean = false;
  public isShowBillRate: boolean = false;
  public filterFromDate: string;
  public filterToDate: string;
  public headCount: number;
  public totalHeadCount: number=0;
  public projectId;
  public clientId;
  public billInfoList: IBillInfo[] = [];
  public projectList = [];
  public clientList = [];
  public listAllResource = []

  public theadTable: THeadTable[] = [
    { name: "#", width: "30px", maxWidth: "50px" },
    { name: "Bill Account", width: "250px", maxWidth: "300px"},
    { name: "Client", width: "100px", maxWidth: "150px"},
    { name: "Projects", width: "100px", maxWidth: "150px"},
    { name: "Skills"},
    { name: "Is Charge", width: "70px", padding : "12px 10px", whiteSpace: "nowrap" },
    { name: "Is Expose", width: "70px", padding : "12px 10px", whiteSpace: "nowrap" },
    { name: "Head Count", width: "100px" },
    { name: "Bill Date", width: "100px" },
    { name: "Linked Resource"},
    { name: "Note" },
  ];

  public projectStatusList = [
    { text: "Potential", value: APP_ENUMS.ProjectStatus.Potential },
    { text: "InProgress", value: APP_ENUMS.ProjectStatus.InProgress },
    { text: "Closed", value: APP_ENUMS.ProjectStatus.Closed },
  ];

  public isChargeList = [
    { text: "Charged", value: true },
    { text: "UnCharged", value: false },
  ];

  public isExposeList = [
   { text: "Exposed", value: true },
   { text: "UnExposed", value: false },
  ];

  editingRows: { [key: number]: { [key: number]: { [key: string]: boolean } } } = {};
  originalContribute: { [key: number]: { [key: number]: { [key: string]: number } } } = {};

  Resource_TabAllResource_ViewUserStarSkill = PERMISSIONS_CONSTANT.Resource_TabAllResource_ViewUserStarSkill;
  Resource_TabAllResource_UpdateSkill = PERMISSIONS_CONSTANT.Resource_TabAllResource_UpdateSkill;

  private numberSkill: number = 3;
  private isViewAllUserSkill: { [billId: number] : boolean } = {};


  constructor(
    injector: Injector,
    private planningBillInfoService: PlanningBillInfoService,
    private projectUserBillService: ProjectUserBillService,
    private dialog: MatDialog,
    private fileHandlerService: FileHandlerService
    ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getProjectUserBill();
    this.getProjectClientBill();
    this.getAllLinkResource();
    this.refresh();
  }
  
  getProjectUserBill(): void {
    this.planningBillInfoService
        .GetAllProjectUserBill()
        .pipe(catchError(this.planningBillInfoService.handleError))
        .subscribe((data) => {
            this.projectList = data.result;
        });
  }

  getProjectClientBill(): void {
    this.planningBillInfoService
        .GetAllProjectClientBill()
        .pipe(catchError(this.planningBillInfoService.handleError))
        .subscribe((data) => {
            this.clientList = data.result;
        });
    }

    getAllLinkResource() {
        this.planningBillInfoService.GetAllResource().subscribe(res => {
            this.listAllResource = res.result;
            this.isLoading = false;
        }, () => { this.isLoading = false; });
    }
 
  protected list(
    request: PagedRequestDto,
    pageNumber: number,
  ): void {
    const requestBody :any = {...request,
      searchText: this.searchText,
      projectId: this.projectId,
      clientId: this.clientId,
      projectStatus: this.projectStatus,
      isCharge: this.isCharge,
      isExpose: this.isExpose,
      headCount: this.headCount,
      sortParams: this.sortResource};

    this.planningBillInfoService
      .GetAllBillInfo(requestBody)
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe(
        (data) => {
          this.billInfoList = data.result.gridResult.items;
          this.showPaging(data.result.gridResult, pageNumber);
          this.isLoading = false;
          this.totalHeadCount = data.result.totalHeadCount
        },
        () => {}
      );
    }

    filterLinkedResourcesByBillId(billId: number){  

      let billInfo: any[] = [];

      this.planningBillInfoService
      .GetLinkResources(billId)
      .pipe(catchError(this.planningBillInfoService.handleError))
      .subscribe(
        (data) => {
          billInfo = data.result;
          this.billInfoList.forEach(item => {
              item.projects.forEach(project => {
                  if (project.billId === billId) {
                      project.linkedResources = billInfo;
                  }
              })
           })
        },
        () => {}
      );
    }
    handleOpenDialogShadowAccount(projectId, userId, listResource, id) {
        const show = this.dialog.open(HandleLinkedResourcesDialogComponent, {
            data: {
                projectId: projectId,
                userId: this.userIdOld != userId && this.isEditUserBill ? this.userIdOld : userId,
                listResource: listResource ? listResource.map(item => item.id) : [],
                listAllResource: this.listAllResource,
                userIdNew: userId,
                projectUserBillId: id
            },
            width: "700px",
        })

        show.afterClosed().subscribe((res) => {
            if (res.isSave) {
                this.filterLinkedResourcesByBillId(id);
            }
        })
    }

    public removeLinkResource(userId, id) {
        const req = {
            projectUserBillId: id,
            userIds: [userId]
        }
        abp.message.confirm(
            "Remove linked resource?",
            "",
            (result: boolean) => {
                if (result) {
                    this.isLoading = true;
                    this.planningBillInfoService.RemoveLinkedResource(req).pipe(catchError(this.planningBillInfoService.handleError)).subscribe(data => {
                        abp.notify.success(`Linked Resource Removed Successfully!`);
                        this.filterLinkedResourcesByBillId(id);
                    }, () => {
                        this.isLoading = false
                    })
                }
            }
        )
    }

  protected delete(entity: BillAccountPlanComponent): void {}

  filerByProjectStatus() {
    this.getDataPage(1);
  }

  onSelectedChargeFilter() {
    this.getDataPage(1);
  }

  filerByProject() {
    this.getDataPage(1);
  }

  filerByClient() {
    this.getDataPage(1);
  }

  checkTimeInFilterDate(time) {
    if (
      time >= this.filterFromDate &&
      Date.parse(time) <= Date.parse(this.filterToDate)
    ) {
      return true;
    } else {
      return false;
    }
  }

  applyProjectFilter() {
    this.projectId = "";
    this.searchProject = "";
    this.getDataPage(1);
  }

  applyClientFilter() {
    this.clientId = "";
    this.searchClient = "";
    this.getDataPage(1);
  }

  applyProjectStatusFilter() {
    this.projectStatus = "";
    this.getDataPage(1);
  }

  clearChargeFilter() {
    this.isCharge = null;
    this.getDataPage(1);
  }

  isShowBtnClearChargeFilter() {
    return this.isCharge !== null;
  }

  styleThead(item: any) {
    return {
      width: item.width,
      height: item.height,
    };
  }

  viewProjectDetail(project) {
    let routingToUrl: string = this.getRoutingUrl(project);
    const url = this.router.serializeUrl(
      this.router.createUrlTree([routingToUrl], {
        queryParams: {
          id: project.projectId,
          type: project.projectType,
          projectName: project.projectName,
          projectCode: project.projectCode,
        },
      })
    );
    return url;
  }

  getRoutingUrl(project) {
    if (project.projectType == 5) {
      return this.permission.isGranted(
        this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport
      ) &&
      this.permission.isGranted(
        this.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View
      )
        ? "/app/training-project-detail/training-weekly-report"
        : "/app/training-project-detail/training-project-general";
    } else if (project.projectType == 3) {
      return this.permission.isGranted(
        this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport
      ) &&
      this.permission.isGranted(
        this.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View
      )
        ? "/app/product-project-detail/product-weekly-report"
        : "/app/product-project-detail/product-project-general";
    } else {
      return "/app/list-project-detail/project-bill-tab";
    }
  }

  showLevel(checked: boolean) {
    this.isShowLevel = checked;
  }

  showBillRate(checked: boolean) {
    this.isShowBillRate = checked;
  }

  UpdateBillNote(userInfor, projectId, note, projectName) {
    const addOrEditNoteDialog = this.dialog.open(BillAccountDialogNoteComponent, {
      width: "580px",
      data: {
        userInfor: userInfor,
        projectId: projectId,
        note: note,
        projectName: projectName
      },
    });
    addOrEditNoteDialog.afterClosed().subscribe(() => {
      this.refresh();
    });
  }

  edit(source: number, index: number, field: string, contribute: number): void {
    this.editingRows[source] = {};
    this.originalContribute[source] = {};
    this.editingRows[source][index] = { [field]: true };
    this.originalContribute[source] = { [index]: { [field]: contribute }};
  }

  updateContribute(projectUserBillId: number, userId: number, contribute: number) {
    this.isLoading = true
    const reqAdd = {
      projectUserBillId,
      userId,
      contribute
    };
    
    this.projectUserBillService.UpdateLinkOneProjectUserBillAccount(reqAdd).pipe(
      catchError(this.projectUserBillService.handleError)
    ).subscribe(() => {
      abp.notify.success("Linked resources updated successfully");
      this.isLoading = false;
      this.editingRows[projectUserBillId] = {};
    }, () => { this.isLoading = false; });
  }

  cancelUpdate(source: number, resource: any, index: number): void {
    this.editingRows[source] = {};
    resource.contribute = this.originalContribute[source][index]?.contribute;
  }

  expandCollapseUserSkill(billId: number) {
    this.isViewAllUserSkill[billId] = !this.isViewAllUserSkill[billId];
  }

  updateUserSkill(project: IProject, fullName: string) {
    let ref = this.dialog.open(UpdateUserSkillDialogComponent, {
      width: "700px",
      data: {
        userSkills: project.userSkills,
        id: project.billId,
        fullName: project.accountName || fullName,
        note: project.skillNote,
        viewStarSkillUser: this.permission.isGranted(this.Resource_TabAllResource_ViewUserStarSkill),
        typeUpdate: AppConsts.UpdateUserSkillType.PROJECT
      },
    });
    ref.afterClosed().subscribe(rs => {
      if (rs) {
        this.refresh()
      }
    })
  }

  openUploadCvDialog(project: IProject, itemIndex: number, projectIndex: number): void {
    const dialogRef = this.dialog.open(UploadCvBillAccountComponent, {
      data: { ...project, id: project.billId } as GetCvBillAccountDto,
      width: '700px',
    });
    dialogRef.afterClosed().subscribe((result?: GetCvBillAccountDto) => {
      if (result && this.billInfoList[itemIndex]?.projects[projectIndex]) {
        const updatedProject = { ...this.billInfoList[itemIndex].projects[projectIndex], ...result };
        this.billInfoList[itemIndex].projects[projectIndex] = updatedProject;
      }
    });
  }

  downloadFile(id: number){
    this.projectUserBillService.DownloadCVLink(id).subscribe(data => {
      this.fileHandlerService.downloadFile(data.result.data, data.result.fileName);
    });
  }
}
