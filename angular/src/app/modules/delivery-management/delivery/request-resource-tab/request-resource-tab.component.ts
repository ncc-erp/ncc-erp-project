import { ProjectStatusPipe } from './../../../../../shared/pipes/project-status-pipe.pipe';
import { FormSendRecruitmentComponent } from "./form-send-recruitment/form-send-recruitment.component";
import { ResourceRequestDto } from "./../../../../service/model/resource-request.dto";
import { isEmpty, isNull, result } from "lodash-es";
import { FormSetDoneComponent } from "./form-set-done/form-set-done.component";
import {
  SortableComponent,
  SortableModel,
} from "./../../../../../shared/components/sortable/sortable.component";
import { AppComponentBase } from "shared/app-component-base";
import { ResourcePlanDto } from "./../../../../service/model/resource-plan.dto";
import { PERMISSIONS_CONSTANT } from "./../../../../constant/permission.constant";
import { RESOURCE_REQUEST_STATUS } from "./../../../../constant/resource-request-status.constant";
import { CreateUpdateResourceRequestComponent } from "./create-update-resource-request/create-update-resource-request.component";
import { MatDialog } from "@angular/material/dialog";
import { DeliveryResourceRequestService } from "./../../../../service/api/delivery-request-resource.service";
import { finalize, catchError } from "rxjs/operators";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "@shared/paged-listing-component-base";
import { RequestResourceDto } from "./../../../../service/model/delivery-management.dto";
import {
  Component,
  OnInit,
  Injector,
  ChangeDetectorRef,
  ViewChild,
  ViewChildren,
  QueryList,
} from "@angular/core";
import { InputFilterDto } from "@shared/filter/filter.component";
import { SkillDto } from "@app/service/model/list-project.dto";
import { FormPlanUserComponent } from "./form-plan-user/form-plan-user.component";
import * as moment from "moment";
import { IDNameDto } from "@app/service/model/id-name.dto";
import { ProjectDescriptionPopupComponent } from "./project-description-popup/project-description-popup.component";
import { FormCvUserComponent } from "./form-cv-user/form-cv-user.component";
import { ListProjectService } from "@app/service/api/list-project.service";
import { DescriptionPopupComponent } from "./description-popup/description-popup.component";
import { ProjectUserService } from "./../../../../service/api/project-user.service";
import { concat, forkJoin, empty } from "rxjs";
import { UpdateUserSkillDialogComponent } from "@app/users/update-user-skill-dialog/update-user-skill-dialog.component";
import { resourceRequestCodeDto } from './multiple-select-resource-request-code/multiple-select-resource-request-code.component';
import { ResourceManagerService } from '../../../../service/api/resource-manager.service';
import { ImportFileResourceComponent } from './import-file-resource/import-file-resource.component';
import * as FileSaver from 'file-saver';
import { ResourceRequestCVDto } from './../../../../service/model/resource-requestCV.dto';
import * as momentTime from "moment-timezone";
import { ResourceRequestCVComponent } from "./form-resource-requestCV/form-resource-requestCV.component";
import { UploadCVPathResourceRequestCV } from './upload-cvPath-resource-requestCV/upload-cvPath-resource-requestCV.component';
import { FormResourceRequestCVUserComponent } from './form-resourceRequestCVUser/form-resourceRequestCVUser.component';
import { of } from 'rxjs';
import 'moment-timezone';

@Component({
  selector: "app-request-resource-tab",
  templateUrl: "./request-resource-tab.component.html",
  styleUrls: ["./request-resource-tab.component.css"],
})
export class RequestResourceTabComponent
  extends PagedListingComponentBase<RequestResourceDto>
  implements OnInit
{
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: "name", comparisions: [0, 6, 7, 8], displayName: "Name" },
    {
      propertyName: "projectName",
      comparisions: [0, 6, 7, 8],
      displayName: "Project Name",
    },
    {
      propertyName: "timeNeed",
      comparisions: [0, 1, 2, 3, 4],
      displayName: "Time Need",
      filterType: 1,
    },
    {
      propertyName: "timeDone",
      comparisions: [0, 1, 2, 3, 4],
      displayName: "Time Done",
      filterType: 1,
    },
  ];
  public pageSizeType = 50;
  public pageSize = 50;
  public projectId = -1;
  public selectedOption: string = "PROJECT";
  public selectedStatus: any = 0;
  public listRequest: RequestResourceDto[] = [];
  public tempListRequest: RequestResourceDto[] = [];

  public listStatuses: any[] = [];
  public listLevels: any[] = [];

  public listSkills: SkillDto[] = [];
  public listProjectUserRoles: IDNameDto[] = [];
  public listProject = [];
  public searchProject: string = "";

  public listRequestCode: resourceRequestCodeDto[] =[];
  public selectedListRequestCode: resourceRequestCodeDto[] =[];

  public listPriorities: any[] = [];
  public isAndCondition: boolean = false;
  public sortResource = {"code":0};
  public theadTable: THeadTable[] = [
    { name: "#" },
    { name: "Request Info", sortName: "projectName", defaultSort: ""},
    { name: "Skill need" },
    { name: "Bill Account", sortName: "billCVEmail", defaultSort: ""},
    { name: "Code", sortName: "code", defaultSort: "ASC"},
    { name: "Resource"},
    { name: "Description"},
    { name: "Note" },
    { name: "Action" },
  ];
  public isShowModal: string = "none";
  public strNote: string;
  public typePM: string;
  public resourceRequestId: number;
  public sortable = new SortableModel("", 0, "");

  public isNewBillAccount: number = 1;
  public isBillAccountList = [
    { text: "All", value: -1 },
    { text: "Bill", value: 1 },
    { text: "No Bill", value: 0 },
  ];

  public listUsers: any[] = [];
  public listActiveUsers: any[] = [];
  public listResourceRequestCV: ResourceRequestCVDto[] = [];
  public isRowExpand: { [key: number]: boolean } = {};
  public expandedRows: number[] = [];
  public editing: { [key: number]: { [key: string]: boolean } } = {};
  public originalValues: { [index: number]: { [field: string]: any } } = {};
  public cvStatusList: string[] = Object.keys(this.APP_ENUM.CVStatus)

  ResourceRequest_View = PERMISSIONS_CONSTANT.ResourceRequest_View;
  ResourceRequest_PlanNewResourceForRequest = PERMISSIONS_CONSTANT.ResourceRequest_PlanNewResourceForRequest;
  ResourceRequest_UpdateResourceRequestPlan = PERMISSIONS_CONSTANT.ResourceRequest_UpdateResourceRequestPlan;
  ResourceRequest_CreateBillResourceForRequest = PERMISSIONS_CONSTANT.ResourceRequest_CreateBillResourceForRequest;
  ResourceRequest_RemoveResouceRequestPlan = PERMISSIONS_CONSTANT.ResourceRequest_RemoveResouceRequestPlan;
  ResourceRequest_UpdateUserBillResourceSkill = PERMISSIONS_CONSTANT.ResourceRequest_UpdateUserBillResourceSkill;
  ResourceRequest_ViewUserResourceStarSkill = PERMISSIONS_CONSTANT.ResourceRequest_ViewUserResourceStarSkill;
  ResourceRequest_SetDone = PERMISSIONS_CONSTANT.ResourceRequest_SetDone;
  ResourceRequest_CancelAllRequest = PERMISSIONS_CONSTANT.ResourceRequest_CancelAllRequest;
  ResourceRequest_CancelMyRequest = PERMISSIONS_CONSTANT.ResourceRequest_CancelMyRequest;
  ResourceRequest_EditPmNote = PERMISSIONS_CONSTANT.ResourceRequest_EditPmNote;
  ResourceRequest_EditDmNote = PERMISSIONS_CONSTANT.ResourceRequest_EditDmNote;
  ResourceRequest_Edit = PERMISSIONS_CONSTANT.ResourceRequest_Edit;
  ResourceRequest_Delete = PERMISSIONS_CONSTANT.ResourceRequest_Delete;
  ResourceRequest_SendRecruitment = PERMISSIONS_CONSTANT.ResourceRequest_SendRecruitment;

  @ViewChildren("sortThead") private elementRefSortable: QueryList<any>;
  constructor(
    private injector: Injector,
    private resourceRequestService: DeliveryResourceRequestService,
    private ref: ChangeDetectorRef,
    private dialog: MatDialog,
    private listProjectService: ListProjectService,
    private projectUserService: ProjectUserService,
    private resourceManagerService: ResourceManagerService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllSkills();
    this.getLevels();
    this.getPriorities();
    this.getStatuses();
    this.getProjectUserRoles();
    this.getAllProject();
    this.getAllRequestCode();
    this.getAllUser();
    this.refresh();
  }

  ngAfterContentInit(): void {
    this.ref.detectChanges();
  }



  showDetail(item: any) {
    if (
      this.permission.isGranted(
        this.DeliveryManagement_ResourceRequest_ViewDetailResourceRequest
      )
    ) {
      this.router.navigate(["app/resourceRequestDetail"], {
        queryParams: {
          id: item.id,
          timeNeed: item.timeNeed,
        },
      });
    }
  }

  edit(index: number, field: string, item: ResourceRequestCVDto, request: RequestResourceDto): void {
    if (!this.editing[index]) {
      this.editing[index] = {};          
    }
    if (!this.originalValues[index]) {
      this.originalValues[index] = {};
    }
    if (!this.originalValues[index][field]) {
      this.originalValues[index][field] = item[field];
    } 
    this.editing[index][field] = true;     
  }
  updateStatusCV(index: number, item: ResourceRequestCVDto, field: string) {
    if (this.editing[index]) {
      const res = {
        resourceRequestCVId: item.id,
        status: item.status,
      }; 
      this.resourceRequestService.updateStatusResourceRequestCV(res).subscribe(
        result => {
          abp.notify.success("Update Status ResourceRequest successfully");  
          const requestIndex = this.listRequest.findIndex(req => req.id === item.resourceRequestId); 
          if (result.result.getResourceRequestDto != null) {
            Object.assign(this.listRequest[requestIndex], result.result.getResourceRequestDto);
            this.listRequest = [...this.listRequest];
          }  
          this.editing[index][field] = false;  
        },
        error => {
          abp.notify.error("Failed to update status");
        }
      );
    }
  }
  
  updateKpiPointCV(index:number,item : ResourceRequestCVDto,field : string){
    if (this.editing[index]) {
        const res = {
          resourceRequestCVId : item.id,
          kpiPoint : item.kpiPoint,
        }
        this.resourceRequestService.updateKpiPointResourceRequestCV(res).subscribe(
          result=>{
            if(res.kpiPoint>10 || res.kpiPoint<0){
              abp.notify.error("Nhập Kpi Point không hợp lệ ! Vui Lòng nhập lại .")
              return;
            }
            abp.notify.success("Update KpiPoint ResourceRequest successfully");
          }
        )
        this.editing[index][field]=false;
    }
  }
  updateSendCVDate(index:number,item : ResourceRequestCVDto,field : string){
    if (this.editing[index]) { 
        const res = {
          resourceRequestCVId : item.id,
          sendCVDate : momentTime(item.interviewDate)
          .tz('Asia/Ho_Chi_Minh')
          .format('YYYY-MM-DD HH:mm:ss'),
        }   
        this.resourceRequestService.updateSendCVDateResourceRequestCV(res).subscribe(
          result=>{
            abp.notify.success("Update SendCVDate ResourceRequest successfully");
          }
        )
        this.editing[index][field]=false;
    }
  }
  updateInterviewTimeCV(index:number,item : ResourceRequestCVDto,field : string){
    if (this.editing[index]) {
  
        const res = {
          resourceRequestCVId : item.id,
          interviewDate : momentTime(item.interviewDate)
          .tz('Asia/Ho_Chi_Minh')
          .format('YYYY-MM-DD HH:mm:ss'),
        }
        this.resourceRequestService.updateInterviewTimeResourceRequestCV(res).subscribe(
          result=>{
            abp.notify.success("Update Status ResourceRequest successfully");
          }
        )
        this.editing[index][field]=false;
    }
  }
  updateNoteCV(index:number,item : ResourceRequestCVDto,field : string){
    if (this.editing[index]) { 
        const res = {
          resourceRequestCVId : item.id,
          note : item.note,
        }
        this.resourceRequestService.updateNoteResourceRequestCV(res).subscribe(
          result=>{
            abp.notify.success("Update Note ResourceRequest successfully");    
          }
        )
        this.editing[index][field]=false;
    }
  }

  cancelEdit(index: number, field: string, request: RequestResourceDto): void {
    if (this.editing[index]) {
      const po = this.listRequest.findIndex(res => res.id === request.id);
      this.listRequest[po].resCV[index][field] = this.originalValues[index][field];
      Object.keys(this.editing[index]).forEach(field => {
        this.editing[index][field] = false;
      });
    }
  }
  getIconClass(entity : RequestResourceDto): string {
      return this.isRowExpand[entity.id] ? 'pi pi-chevron-down' : 'pi pi-chevron-right';
  }
  async getResourceRequestCVExpand(entity: RequestResourceDto) {
    const rowindex = this.expandedRows.indexOf(entity.id);
    if (this.isRowExpand[entity.id]) {   
      delete this.isRowExpand[entity.id];
      const rowindex = this.expandedRows.indexOf(entity.id);
      if (rowindex !== -1) {
        this.expandedRows.splice(rowindex, 1);
      }
    } else { 
      this.isRowExpand[entity.id] = true;
      this.expandedRows.push(entity.id);
    }
      const rs = await this.resourceRequestService.getResouceRequestCV(entity.id).toPromise();     
       if (rs && rs.success) {
        const index = this.listRequest.findIndex(res => res.id === entity.id);
        if (index !== -1) {
          this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];       
        }
      } else {
        console.error('Unexpected response:', rs);
      } 
  }
  checkRowExpanded(id: number): boolean {
    return this.expandedRows.includes(id);
  }
  checkArrayNullOrEmpty(array: any[]): boolean {
    return array === null || array === undefined || array.length === 0;
  }
  checkNullOrEmpty(a : string):boolean{
    return a===null|| a===undefined|| a.trim().length===0;
  }
  public removeCVPath(item: ResourceRequestCVDto) {
    abp.message.confirm("Delete this Link CV?", "", (result: boolean) => {
      if (result) { 
   const input1 = {...item,cvPath: " ",linkCVPath: " "};
   this.resourceRequestService.updateResourceRequestCV(input1).pipe(
    catchError(error => {
      console.error('Error in updateResourceRequestCV API call:', error);
      return null; 
    })
  ).subscribe(
    response => {
      abp.notify.success("Delete Link CV successfully");
        this.resourceRequestService.getResouceRequestCV(item.resourceRequestId).subscribe(
          rs => {
              const index = this.listRequest.findIndex(res => res.id ===item.resourceRequestId);
              if (index !== -1) {
                this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];
                this.listRequest = [...this.listRequest];              
              }          
          },
          error => {
            console.error('Error fetching resource request CV:', error);
          }
        );    
    },
    error => {
      console.error('Error updating resource request CV:', error);
    }
  );
   } 
  });

}
  public deleteResourceRequestCV(item: ResourceRequestCVDto, re: RequestResourceDto): void {
    abp.message.confirm("Delete this ResourceRequestCV?", "", (result: boolean) => {
      if (result) {
        this.resourceRequestService
          .deleteResourceRequestCV(item.id)
          .pipe(catchError(this.resourceRequestService.handleError))
          .subscribe(() => {
            abp.notify.success("Delete ResourceRequestCV successfully");
            const index = this.listRequest.findIndex(res => res.id == re.id);
            this.listRequest[index].resCV = this.listRequest[index].resCV.filter(req => req.id !== item.id);
          });
      }
    });
  }
 public formatInterviewDateToVN(resourceRequestCV: ResourceRequestCVDto): any {
    return {
      ...resourceRequestCV,
      interviewDate: momentTime(resourceRequestCV.interviewDate)
        .tz('Asia/Ho_Chi_Minh')
        .format('YYYY-MM-DD HH:mm:ss'), 
      sendCVDate: momentTime(resourceRequestCV.sendCVDate)
        .tz('Asia/Ho_Chi_Minh')
        .format('YYYY-MM-DD HH:mm:ss')
    };
}
  public updateResourceRequestCV(resourceRequestCV: ResourceRequestCVDto, request: RequestResourceDto): void {  
    const formattedCV = this.formatInterviewDateToVN(resourceRequestCV);
    this.resourceRequestService.updateResourceRequestCV(formattedCV).pipe(
        catchError(error => {
            console.error('Error in updateResourceRequestCV API call:', error);
            return of(null); 
        })
    ).subscribe(response => {
            this.resourceRequestService.getResouceRequestCV(request.id).subscribe(rs => {  
                abp.notify.success(`Update CV Successfully!`);                         
                if (rs && rs.success) {
                    const index = this.listRequest.findIndex(res => res.id === request.id);                                               
                   this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];
                   this.listRequest = [...this.listRequest];
                }
            });
       
    },
    error => {
        console.error('Error updating resource request CV:', error);
    });
}

  public AddCV(request: RequestResourceDto) {
    this.showModalResourceRequestCV("create", {}, request);
  }
  public EditCV(item: any, request: RequestResourceDto) {
    this.showModalResourceRequestCV("edit", item, request);
  }
  async showModalResourceRequestCV(command: string, item: any, request: RequestResourceDto) {
    let resourceRequestCV = {
      id: item.id ? item.id : null,
      requestResource: request,
    };
    const show = this.dialog.open(ResourceRequestCVComponent, {
      data: {
        command: command,
        item1: resourceRequestCV,
        resourceRequestId: request.id,
        requestResourceDto: request,
        cvPath: item.cvPath,
        linkCVPath: item.linkCVPath,
        cvName: item.cvName,
        userId: item.userId,
        status: item.status,
        note: item.note,
        kpiPoint: item.kpiPoint,
        inteviewDate: item.inteviewDate,
        sendCVDate: item.sendCVDate ,
        listUsers: this.listUsers,
      },
      width: "800px",
      maxHeight: "90vh",
    });
    show.afterClosed().subscribe((rs) => {
      if (rs) {
        this.resourceRequestService.getResouceRequestCV(request.id).subscribe(
          rs => {
            const index = this.listRequest.findIndex(res => res.id === request.id);
            this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];
          },
          error => {
            console.error('Error fetching resource request CV', error);
          }
        );
      }
    })
  }

  uploadCVPath(item: ResourceRequestCVDto, request: RequestResourceDto) {
    const dialogRef = this.dialog.open(UploadCVPathResourceRequestCV, {
      data: { id: item.id, width: '500px' }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.resourceRequestService.getResouceRequestCV(request.id).subscribe(
          rs => {
            const index = this.listRequest.findIndex(res => res.id === request.id);
            this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];
          },
          error => {
            console.error('Error fetching resource request CV', error);
          }
        );
      }
    });
  }

  async showCvUser(item : RequestResourceDto,resCv :ResourceRequestCVDto) { 
    const show = this.dialog.open(FormResourceRequestCVUserComponent, {
      data: {
        resourceRequestCV : resCv,
        resourceRequestId: item.id,
        billUserInfo: resCv.user,
        listUsers: this.listUsers,
        cvName: resCv.cvName,
      },
      width: "800px",
      maxHeight: "90vh",
    });
    show.afterClosed().subscribe((rs) => {
      if (rs) {
        this.resourceRequestService.getResouceRequestCV(resCv.resourceRequestId).subscribe(
          rs => {         
            const index = this.listRequest.findIndex(res => res.id === resCv.resourceRequestId);
            this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];
          },
          error => {
            console.error('Error fetching resource request CV', error);
          }
        );
      }
    });
  }
async removeResourceRequestCvUser(resCv : ResourceRequestCVDto){
  abp.message.confirm(
    "Remove This CV User ?",
    "",
    (result: boolean) => {
      if (result) {
        const input = {...resCv,userId : 0 , user : { } };  
     this.resourceRequestService.updateResourceRequestCV(input).pipe(
         catchError(error => {
          console.error('Error in updateResourceRequestCV API call:', error);
          return null; 
         })
      ).subscribe(
             response => {    
                  this.resourceRequestService.getResouceRequestCV(resCv.resourceRequestId).subscribe(
                        rs => {             
                         abp.notify.success(` UserCV Removed Successfully!`);  
                         const index = this.listRequest.findIndex(res => res.id ===resCv.resourceRequestId);
                         if (index !== -1) {
                              this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[];
                              this.listRequest = [...this.listRequest];
                    }          
            },
            error => {
              console.error('Error fetching resource request CV:', error);
            }
          );    
      },
      error => {
        console.error('Error updating resource request CV:', error);
      }
    );

      }
    }
  )
}

  showDialog(command: string, request: any) {
    let resourceRequest = {
      id: request.id ? request.id : null,
      projectId: 0,
    };

    const show = this.dialog.open(CreateUpdateResourceRequestComponent, {
      data: {
        command: command,
        item: resourceRequest,
        skills: this.listSkills,
        levels: this.listLevels,
        typeControl: "request",
        listRequestCode: this.listRequestCode,
      },
      width: "700px",
      maxHeight: "90vh",
    });
    show.afterClosed().subscribe((rs) => {
      if (rs) {
        this.refresh();
        this.getAllRequestCode();
      }
    });
  }

  public createRequest() {
    this.showDialog("create", {});
  }

  public editRequest(item: any) {
    this.showDialog("edit", item);
  }

  public setDoneRequest(item) {
    if (!item.planUserInfo) {
      const request = {
        requestId: item.id,
        startTime: moment().format("YYYY-MM-DD"),
        billStartTime: null,
      };
      this.resourceRequestService.setDoneRequest(request).subscribe((rs) => {
        if (rs) {
          abp.notify.success(`Set done success`);
          item.status = RESOURCE_REQUEST_STATUS.DONE;
          item.statusName = "DONE"
        }
      });
    } else {
      let data = {
        ...item.planUserInfo,
        billUserInfo: item.billUserInfo,
        resourceRequestId: item.id,
      };
      const showModal = this.dialog.open(FormSetDoneComponent, {
        data,
        width: "700px",
        maxHeight: "90vh",
      });
      showModal.afterClosed().subscribe((rs) => {
        if (rs) {
          item.status = RESOURCE_REQUEST_STATUS.DONE;
          item.statusName = "DONE"
        }
      });
    }
  }

  showProject(item) {
    const show = this.dialog.open(ProjectDescriptionPopupComponent, {
      width: "800px",
      maxHeight: "90vh",
      data: item,
    });
    show.afterClosed().subscribe((rs) => {});
  }

  cancelRequest(request: RequestResourceDto) {
    const cancelResourceRequest =
      this.resourceRequestService.cancelResourceRequest(request.id);
    const actions = [cancelResourceRequest];
    abp.message.confirm(
      "Are you sure you want to cancel the request for project: " +
      request.projectName,
      "",
      (result) => {
        if (result) {
          concat(...actions)
            .pipe(
              catchError((error) => {
                abp.notify.error(error);
                return empty(); // Return an empty observable to continue
              })
            )
            .subscribe(() => {
              abp.notify.success("Request canceled successfully!");
              request.status = RESOURCE_REQUEST_STATUS.CANCELLED;
              request.statusName = "CANCELLED"
            });
        }
      }
    );
  }

  active(request: RequestResourceDto) {
    const cancelResourceRequest =
      this.resourceRequestService.activeResourceRequest(request.id);
    const actions = [cancelResourceRequest];
    abp.message.confirm(
      "Are you sure you want to active the request for project: " +
      request.projectName,
      "",
      (result) => {
        if (result) {
          concat(...actions)
            .pipe(
              catchError((error) => {
                abp.notify.error(error);
                return empty(); // Return an empty observable to continue
              })
            )
            .subscribe(() => {
              abp.notify.success("Request has been successfully activated!");
              request.status = RESOURCE_REQUEST_STATUS.PENDING;
              request.statusName = "PENDING"
            });
        }
      }
    );
  }

  async showModalPlanUser(item) {
    const data = await this.getPlanResource(item);
    const show = this.dialog.open(FormPlanUserComponent, {
      data: { ...data, projectUserRoles: this.listProjectUserRoles, listActiveUsers: this.listActiveUsers },
      width: "800px",
      maxHeight: "90vh",
    });
    show.afterClosed().subscribe((rs) => {
      if (!rs) return;
      item.planUserInfo = rs.data;
    });
  }

  getAllUser(){
    this.resourceManagerService.GetListAllUserShortInfo().subscribe(res => {
      this.listUsers = res.result
      this.listActiveUsers = this.listUsers.filter(x=>x.isActive);
    });
  }

  async getPlanResource(item) {
    let data = new ResourcePlanDto(item.id, 0);
    if (!item.planUserInfo) return data;

    data.projectRole = item.planUserInfo.role;
    data.projectUserId = item.planUserInfo.projectUserId;
    data.startTime = item.planUserInfo.plannedDate;
    data.userId = item.planUserInfo.employee.id;
    data.cvName = item.cvName;
    return data;
  }

  async showModalCvUser(item) {
    const isHasResource = item.planUserInfo !== null;
    const planUser = await this.getPlanResource(item);

    const show = this.dialog.open(FormCvUserComponent, {
      data: {
        resourceRequestId: item.id,
        billUserInfo: item.billUserInfo,
        listUsers: this.listUsers, 
        cvName : item.cvName,  
      },
      width: "800px",
      maxHeight: "90vh",
    });
    show.afterClosed().subscribe((rs) => {
        item.billUserInfo = rs.data.billUserInfo;
        item.planUserInfo = rs.data.planUserInfo;
        item.cvName = rs.data.cvName;
    });
  }

  async removePlanUser(item) {
    abp.message.confirm(
      "Remove This Resource ?",
      "",
      (result: boolean) => {
        if (result) {
          this.resourceRequestService.RemoveResourceRequestPlan(item.id).pipe(catchError(this.resourceRequestService.handleError)).subscribe(data => {
            abp.notify.success(` Resource Removed Successfully!`);
            item.planUserInfo = null;
          }, () => {
          })
        }
      }
    )
  }

  async removeCvUser(item) {
    const input = {
      resourceRequestId: item.id
    };

    abp.message.confirm(
      "Remove This Bill Account ?",
      "",
      (result: boolean) => {
        if (result) {
          this.resourceRequestService.UpdateBillInfoPlan(input).pipe(catchError(this.resourceRequestService.handleError)).subscribe(data => {
            abp.notify.success(` Bill Account Removed Successfully!`);
            item.billUserInfo = null;
          }, () => {
          })
        }
      }
    )
  }

  sendRecruitment(item: ResourceRequestDto) {
    const show = this.dialog.open(FormSendRecruitmentComponent, {
      data: {
        id: item.id,
        name: item.name,
        dmNote: item.dmNote,
        pmNote: item.pmNote,
      } as SendRecruitmentModel,
      width: "700px",
      maxHeight: "90vh",
    });
    show.afterClosed().subscribe((rs) => {
      if (!rs) return;
      item.isRecruitmentSend = rs.isRecruitmentSend;
      item.recruitmentUrl = rs.recruitmentUrl;
    });
  }
   importExcel(item: ResourceRequestDto) {
    const dialogRef = this.dialog.open(ImportFileResourceComponent, {
      data: { id: item.id, width: '500px' }
    });
    dialogRef.afterClosed().subscribe(res => {
      // item.File= result.File;     
        this.resourceRequestService.getResouceRequestCV(item.id).subscribe(
          rs => {             
                const index = this.listRequest.findIndex(res => res.id ===item.id);
                this.listRequest[index].resCV = rs.result as ResourceRequestCVDto[]; 
                this.listRequest[index].linkCv = res;    
                this.listRequest = [...this.listRequest];               
              },
          error => {
          console.error('Error fetching resource request CV:', error);
           });
          });
  }

  // #region update note for pm, dmPm
  public openModal(name, typePM, content, id, code) {
    this.typePM = typePM;
    this.modal_title = name;
    this.strNote = content;
    this.resourceRequestId = id;
    this.isShowModal = "block";
    this.code = code;
  }

  public closeModal() {
    this.isShowModal = "none";
  }

  public titleModal(typePM) {
    switch (typePM) {
      case 'Note':
        return 'Note for Request Code: ' +  `${this.code}`;
      case 'Description':
        return 'Description'
      default: break;
    }
  }

  openPopupSkill(user, userSkill) {
    let ref = this.dialog.open(UpdateUserSkillDialogComponent, {
      width: "700px",
      data: {
        isNotUpdate: !this.permission.isGranted(
          this.ResourceRequest_UpdateUserBillResourceSkill
        ),
        userSkills: userSkill ? userSkill : [],
        viewStarSkillUser: this.permission.isGranted(
          this.ResourceRequest_ViewUserResourceStarSkill
        ),
        id: user.id,
        fullName: user.fullName,
        note: userSkill ? userSkill[0].skillNote : "",
      },
    });
    ref.afterClosed().subscribe((rs) => {
      if (rs) {
        this.refresh();
      }
    });
  }

  public updateNote() {
    let request = {
      resourceRequestId: this.resourceRequestId,
      note: this.strNote,
    };
    this.resourceRequestService
      .updateNote(request, this.typePM)
      .subscribe((rs) => {
        if (rs.success) {
          abp.notify.success("Update Note Successfully!");
          let index = this.listRequest.findIndex(
            (x) => x.id == request.resourceRequestId
          );
          if (index >= 0) {
            if (this.typePM == "Description")
              this.listRequest[index].pmNote = request.note;
            else this.listRequest[index].dmNote = request.note;
          }
          this.closeModal();
        } else {
          abp.notify.error(rs.result);
        }
      });
  }
  // #endregion

  // #region paging, search, sortable, filter
  protected list(
    request: any,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    this.isLoading= true;
    let requestBody: any = request;
    requestBody.isAndCondition = this.isAndCondition;

    let objFilter = [
      { name: "status", isTrue: false, value: this.selectedStatus },
      { name: "projectId", isTrue: false, value: this.projectId },
      { name: "isNewBillAccount", isTrue: false, value: this.isNewBillAccount },
    ];

    objFilter.forEach((item) => {
      if (!item.isTrue) {
        requestBody.filterItems = this.AddFilterItem(
          requestBody,
          item.name,
          item.value,
        );
      }
      if (item.value == -1) {
        requestBody.filterItems = this.clearFilter(requestBody, item.name, "");
        item.isTrue = true;
      }
    });
    requestBody.filterRequestCode = this.selectedListRequestCode.map(item => item.code);
    requestBody.filterRequestStatus = this.selectedListRequestCode.map(item => item.status);
    requestBody.isTraining = false;
    requestBody.sortParams = this.sortResource;
    this.resourceRequestService
      .getResourcePaging(requestBody, this.selectedOption)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
        catchError(this.resourceRequestService.handleError)
      )
      .subscribe(
        (data) => {
          this.listRequest = this.tempListRequest = data.result.items;
          this.showPaging(data.result, pageNumber);
          this.isLoading= false
        },
        (error) => {
          abp.notify.error(error);
          this.isLoading= false
        }
      );
    let rsFilter = this.resetDataSearch(requestBody, request, objFilter);
    request = rsFilter.request;
    requestBody = rsFilter.requestBody;
  }

  resetDataSearch(requestBody: any, request: any, objFilter: any) {
    objFilter.forEach((item) => {
      if (!item.isTrue) {
        request.filterItems = this.clearFilter(request, item.name, "");
      }
    });
    requestBody.sort = null;
    requestBody.sortDirection = null;


    return {
      request,
      requestBody,
      objFilter,
    };
  }

  clearAllFilter() {
    this.filterItems = [];
    this.searchText = "";
    this.projectId = -1;
    this.selectedStatus = 0;
    this.isNewBillAccount = -1
    this.selectedListRequestCode = [];
    this.changeSortableByName("priority", "DESC");
    this.sortable = new SortableModel("", 1, "");
    this.refresh();
  }

  onChangeStatus() {
    let status = this.listStatuses.find((x) => x.id == this.selectedStatus);
    if (status && status.name == "DONE") {
      this.sortable = new SortableModel("timeDone", 1, "DESC");
      this.changeSortableByName("", "");
    }
    this.getDataPage(1);
  }

  sortTable(event: any) {
    this.sortable = event;
    this.changeSortableByName(
      this.sortable.sort,
      this.sortable.typeSort,
      this.sortable.sortDirection
    );
    this.refresh();
  }

  changeSortableByName(sort: string, sortType: string, sortDirection?: number) {
    if (!sortType) {
      delete this.sortResource[sort];
    } else {
      this.sortResource[sort] = sortDirection;
    }
    this.ref.detectChanges();
  }
  // #endregion

  //#region get skills, statuses, levels, priorities
  getAllSkills() {
    this.resourceRequestService.getSkills().subscribe((data) => {
      this.listSkills = data.result;
    });
  }

  getLevels() {
    this.resourceRequestService.getLevels().subscribe((res) => {
      this.listLevels = res.result;
    });
  }

  getPriorities() {
    this.resourceRequestService.getPriorities().subscribe((res) => {
      this.listPriorities = res.result;
    });
  }

  getStatuses() {
    this.resourceRequestService.getStatuses().subscribe((res) => {
      this.listStatuses = res.result;
    });
  }

  getProjectUserRoles() {
    this.resourceRequestService.getProjectUserRoles().subscribe((rs: any) => {
      this.listProjectUserRoles = rs.result;
    });
  }

  getAllProject() {
    this.listProjectService.getMyProjects().subscribe((data) => {
      this.listProject = data.result;
    });
  }

  getAllRequestCode() {
    this.resourceRequestService.getListRequestCode().subscribe((data) => {
      this.listRequestCode = data.result;
    });
  }

  onCancelFilterListRequestCode() {
    this.selectedListRequestCode = [];
    this.getDataPage(1);
  }

  onSelectedBillAccountFilter() {
    this.getDataPage(1);
  }

  filterRequestCode(selectedRequestCode: resourceRequestCodeDto[]) {
    this.selectedListRequestCode = selectedRequestCode;
    this.getDataPage(1);
  }

  // #endregion
  styleThead(item: any) {
    return {
      width : item.width,
      'min-width': item.minWidths,
      'max-width':item.maxWidth,
      height: item.height,
    };
  }

  public getValueByEnum(enumValue: number, enumObject) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }

  viewRecruitment(url) {
    window.open(url, "_blank");
  }

  showDescription(note) {
    const show = this.dialog.open(DescriptionPopupComponent, {
      width: "1100px",
      maxHeight: "90vh",
      data: note,
    });
  }

  protected delete(item: RequestResourceDto): void {
    abp.message.confirm("Delete this request?", "", (result: boolean) => {
      if (result) {
        this.resourceRequestService
          .delete(item.id)
          .pipe(catchError(this.resourceRequestService.handleError))
          .subscribe(() => {
            abp.notify.success(" Delete request successfully");
            this.listRequest = this.listRequest.filter(req => req.id !== item.id);
            this.getAllRequestCode();
          });
      }
    });
  }

  isShowButtonMenuAction(item) {
    return (
      item.statusName != "DONE" ||
      //&& !item.isRecruitmentSend
      item.statusName != "CANCELLED"
    );
  }

  isShowBtnCreate() {
    return (
      this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CreateNewRequest) ||
      this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CreateNewRequestByPM)
    );
  }

  isShowBtnCancel(item) {
    return (
      item.status == RESOURCE_REQUEST_STATUS.PENDING &&
      (this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CancelAllRequest) ||
        this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CancelMyRequest))
    );
  }

  isShowBtnActivate(item) {
    return (
      item.status == RESOURCE_REQUEST_STATUS.CANCELLED &&
      this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_Activate)
    );
  }

  isShowBtnEdit(item) {
    return (
      item.status == RESOURCE_REQUEST_STATUS.PENDING &&
      this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_Edit)
    );
  }

  isShowBtnSetDone(item) {
    return (
      item.status == RESOURCE_REQUEST_STATUS.PENDING &&
      ((item.isRequiredPlanResource && item.planUserInfo) ||
        !item.isRequiredPlanResource) &&
      this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_SetDone)
    );
  }

  isShowBtnSendRecruitment(item) {
    return (
      item.status == RESOURCE_REQUEST_STATUS.PENDING &&
      (!item.isRecruitmentSend || !item.recruitmentUrl) &&
      this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_SendRecruitment)
    );
  }
  isShowBtnUpload(item){
    return(item);
  }
  downloadFile(resourceRequest: any){ 
    this.resourceRequestService.DownloadCVLink(resourceRequest.id).subscribe(data => {
      const file = new Blob([this.s2ab(atob(data.result.data))],{
        type: "application/vnd.ms-excel;charset=utf-8"
        
      });
      FileSaver.saveAs(file,data.result.fileName);
    })
  }
  s2ab(s){
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }
  openInNewTab(event: MouseEvent, resourceRequest: any){
    event.preventDefault();
    if(resourceRequest.id){
      this.resourceRequestService.DownloadCVLink(resourceRequest.id).subscribe(data => {
        const file = new Blob([this.s2ab(atob(data.result.data))],{
          type: "application/vnd.ms-excel;charset=utf-8"
          
        });
        FileSaver.saveAs(file,data.result.fileName);
      })
      window.open('_blank');
    }
  }
  isShowBtnDelete(item) {
    return this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_Delete);
  }

  public sliceUrl(url: string): string {
    if (isNull(url) || isEmpty(url)) {
      return "";
    }
    var regexp = new RegExp(/\/[\d]\d{0,}/g);
    return regexp.exec(url).toString().slice(1);
  }

  extractEmailName(emailAddress) {
    return emailAddress.split('@')[0];
  }
}

export class THeadTable {
  name: string;
  width?: string = "auto";
  minWidth?: string = "auto";
  maxWidth?: string = "auto";
  height?: string = "auto";
  backgroud_color?: string;
  sortName?: string;
  defaultSort?: string;
  padding?: string;
  whiteSpace?: string;
}

export class SendRecruitmentModel {
  id: number;
  name: string;
  dmNote: string;
  pmNote: string;
} 
