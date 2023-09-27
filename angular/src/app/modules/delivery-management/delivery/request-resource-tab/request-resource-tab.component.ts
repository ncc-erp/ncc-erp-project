import { FormSendRecruitmentComponent } from './form-send-recruitment/form-send-recruitment.component';
import { ResourceRequestDto } from './../../../../service/model/resource-request.dto';
import { isEmpty, isNull, result } from 'lodash-es';
import { FormSetDoneComponent } from './form-set-done/form-set-done.component';
import { SortableComponent, SortableModel } from './../../../../../shared/components/sortable/sortable.component';
import { AppComponentBase } from 'shared/app-component-base';
import { ResourcePlanDto } from './../../../../service/model/resource-plan.dto';
import { PERMISSIONS_CONSTANT } from './../../../../constant/permission.constant';
import { RESOURCE_REQUEST_STATUS } from './../../../../constant/resource-request-status.constant';
import { CreateUpdateResourceRequestComponent } from './create-update-resource-request/create-update-resource-request.component';
import { MatDialog } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from './../../../../service/api/delivery-request-resource.service';
import { finalize, catchError } from 'rxjs/operators';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { RequestResourceDto } from './../../../../service/model/delivery-management.dto';
import { Component, OnInit, Injector, ChangeDetectorRef, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { InputFilterDto } from '@shared/filter/filter.component';
import { SkillDto } from '@app/service/model/list-project.dto';
import { FormPlanUserComponent } from './form-plan-user/form-plan-user.component';
import * as moment from 'moment';
import { IDNameDto } from '@app/service/model/id-name.dto';
import { ProjectDescriptionPopupComponent } from './project-description-popup/project-description-popup.component';
import { FormCvUserComponent } from './form-cv-user/form-cv-user.component';
import { ListProjectService } from '@app/service/api/list-project.service';
import { DescriptionPopupComponent } from './description-popup/description-popup.component';

@Component({
  selector: 'app-request-resource-tab',
  templateUrl: './request-resource-tab.component.html',
  styleUrls: ['./request-resource-tab.component.css']
})
export class RequestResourceTabComponent extends PagedListingComponentBase<RequestResourceDto> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Name" },
    { propertyName: 'projectName', comparisions: [0, 6, 7, 8], displayName: "Project Name" },
    { propertyName: 'timeNeed', comparisions: [0, 1, 2, 3, 4], displayName: "Time Need", filterType: 1 },
    { propertyName: 'timeDone', comparisions: [0, 1, 2, 3, 4], displayName: "Time Done", filterType: 1 },
  ];
  public projectId = -1;
  public selectedOption: string = "PROJECT"
  public selectedStatus: any = 0
  public listRequest: RequestResourceDto[] = [];
  public tempListRequest: RequestResourceDto[] = [];
  public listStatuses: any[] = []
  public listLevels: any[] = []
  public listSkills: SkillDto[] = [];
  public listProjectUserRoles: IDNameDto[] = []
  public listProject = []
  public searchProject:string = "";
  public listRequestCode = [];
  public requestCode:any = -1;
  public searchCode:string = "";
  public listPriorities: any[] = []
  public isAndCondition: boolean = false;
  public sortResource = {priority: 0}
  public theadTable: THeadTable[] = [
    { name: '#' },
    { name: 'Request Info' },
    { name: 'Skill need' },
    { name: 'Code' },
    { name: 'CV' },
    { name: 'Resource'},
    { name: 'Description'},
    { name: 'Note' },
    { name: 'Action' },
  ]
  public isShowModal: string = 'none'
  public modal_title: string
  public strNote: string
  public typePM: string
  public resourceRequestId: number
  public sortable = new SortableModel('', 0, '')

  ResourceRequest_View = PERMISSIONS_CONSTANT.ResourceRequest_View;
  ResourceRequest_PlanNewResourceForRequest = PERMISSIONS_CONSTANT.ResourceRequest_PlanNewResourceForRequest;
  ResourceRequest_UpdateResourceRequestPlan = PERMISSIONS_CONSTANT.ResourceRequest_UpdateResourceRequestPlan;
  ResourceRequest_CreateBillResourceForRequest = PERMISSIONS_CONSTANT.ResourceRequest_CreateBillResourceForRequest;
  ResourceRequest_RemoveResouceRequestPlan = PERMISSIONS_CONSTANT.ResourceRequest_RemoveResouceRequestPlan;
  ResourceRequest_SetDone = PERMISSIONS_CONSTANT.ResourceRequest_SetDone;
  ResourceRequest_CancelAllRequest = PERMISSIONS_CONSTANT.ResourceRequest_CancelAllRequest;
  ResourceRequest_CancelMyRequest = PERMISSIONS_CONSTANT.ResourceRequest_CancelMyRequest;
  ResourceRequest_EditPmNote = PERMISSIONS_CONSTANT.ResourceRequest_EditPmNote;
  ResourceRequest_EditDmNote = PERMISSIONS_CONSTANT.ResourceRequest_EditDmNote;
  ResourceRequest_Edit = PERMISSIONS_CONSTANT.ResourceRequest_Edit;
  ResourceRequest_Delete = PERMISSIONS_CONSTANT.ResourceRequest_Delete;
  ResourceRequest_SendRecruitment = PERMISSIONS_CONSTANT.ResourceRequest_SendRecruitment;


  @ViewChildren('sortThead') private elementRefSortable: QueryList<any>;
  constructor(
    private injector: Injector,
    private resourceRequestService: DeliveryResourceRequestService,
    private ref: ChangeDetectorRef,
    private dialog: MatDialog,
    private listProjectService: ListProjectService,
  ) {
    super(injector)
  }

  ngOnInit(): void {
    this.getAllSkills()
    this.getLevels()
    this.getPriorities()
    this.getStatuses()
    this.getProjectUserRoles();
    this.getAllProject()
    this.getAllRequestCode();
    this.refresh();
  }

  ngAfterContentInit(): void {
    this.ref.detectChanges()
  }

  showDetail(item: any) {
    if (this.permission.isGranted(this.DeliveryManagement_ResourceRequest_ViewDetailResourceRequest)) {
      this.router.navigate(['app/resourceRequestDetail'], {
        queryParams: {
          id: item.id,
          timeNeed: item.timeNeed
        }
      })
    }
  }
  showDialog(command: string, request: any) {
    let resourceRequest = {
      id: request.id ? request.id : null,
      projectId: 0
    }
    const show = this.dialog.open(CreateUpdateResourceRequestComponent, {
      data: {
        command: command,
        item: resourceRequest,
        skills: this.listSkills,
        levels: this.listLevels,
        typeControl: 'request',
        listRequestCode: this.listRequestCode
      },
      width: "700px",
      maxHeight: '90vh',
    })
    show.afterClosed().subscribe(rs => {
      if (rs){
        this.refresh()
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
    let data = {
      ...item.planUserInfo,
      billUserInfo: item.billUserInfo,
      requestName: item.name,
      resourceRequestId: item.id
    }
    const showModal = this.dialog.open(FormSetDoneComponent, {
      data,
      width: "700px",
      maxHeight: "90vh"
    })
    showModal.afterClosed().subscribe((rs) => {
      if (rs)
        this.refresh()
    })
  }

  showProject(item){
      const show = this.dialog.open(ProjectDescriptionPopupComponent , {
        width: "800px",
        maxHeight: '90vh',
        data:item
      })
      show.afterClosed().subscribe(rs => {
       
      });
    }
  
  cancelRequest(request: RequestResourceDto) {
    abp.message.confirm(
      'Are you sure cancel request for project: ' + request.projectName,
      '',
      (result) => {
        if (result) {
          this.resourceRequestService.cancelResourceRequest(request.id).subscribe(res => {
            if (res.success) {
              abp.notify.success('Cancel Request Success!');
              this.refresh();
            }
            else {
              abp.notify.error(res.result)
            }
          })
        }
      }
    )
  }

  async showModalPlanUser(item: any) {
    const data = await this.getPlanResource(item);
    const show = this.dialog.open(FormPlanUserComponent, {
      data: { ...data, projectUserRoles: this.listProjectUserRoles },
      width: "700px",
      maxHeight: "90vh"
    })
    show.afterClosed().subscribe(rs => {
      if (!rs) return
      if (rs.type == 'delete') {
        this.refresh()
      }
      else {
        let index = this.listRequest.findIndex(x => x.id == rs.data.resourceRequestId)
        if (index >= 0)
          this.listRequest[index].planUserInfo = rs.data.result
      }

    });
  }

  async getPlanResource(item) {
    let data = new ResourcePlanDto(item.id, 0);
    if (!item.planUserInfo)
      return data;
    let res = await this.resourceRequestService.getPlanResource(item.planUserInfo.projectUserId, item.id)
    return res.result
  }

  showModalCvUser(item:any){
    const show = this.dialog.open(FormCvUserComponent, {
      data: item,
      width: "700px",
      maxHeight: "90vh"
    })
    show.afterClosed().subscribe(rs => {
      if (rs) this.refresh()
    });
  }

  sendRecruitment(item: ResourceRequestDto) {
    const show = this.dialog.open(FormSendRecruitmentComponent, {
      data: { id: item.id, name: item.name, dmNote: item.dmNote, pmNote: item.pmNote } as SendRecruitmentModel,
      width: "700px",
      maxHeight: "90vh"
    })
    show.afterClosed().subscribe(rs => {
      if (!rs) return;
      item.isRecruitmentSend = rs.isRecruitmentSend;
      item.recruitmentUrl = rs.recruitmentUrl;
    });
  }

  // #region update note for pm, dmPm
  public openModal(name, typePM, content, id) {
    this.typePM = typePM
    this.modal_title = name
    this.strNote = content
    this.resourceRequestId = id
    this.isShowModal = 'block'
  }

  public closeModal() {
    this.isShowModal = 'none'
  }

  public updateNote() {
    let request = {
      resourceRequestId: this.resourceRequestId,
      note: this.strNote,
    }
    this.resourceRequestService.updateNote(request, this.typePM).subscribe(rs => {
      if (rs.success) {
        abp.notify.success('Update Note Successfully!')
        let index = this.listRequest.findIndex(x => x.id == request.resourceRequestId);
        if (index >= 0) {
          if (this.typePM == 'Description')
            this.listRequest[index].pmNote = request.note;
          else
            this.listRequest[index].dmNote = request.note;
        }
        this.closeModal()
      }
      else {
        abp.notify.error(rs.result)
      }
    })
  }
  // #endregion

  // #region paging, search, sortable, filter
  protected list(request: any, pageNumber: number, finishedCallback: Function): void {
    let requestBody: any = request
    requestBody.isAndCondition = this.isAndCondition
    let objFilter = [
      { name: 'status', isTrue: false, value: this.selectedStatus },
      {name:'projectId',isTrue:false, value:this.projectId},
      {name:'code',isTrue:false,value: this.requestCode}
    ];

    objFilter.forEach((item) => {
      if (!item.isTrue) {
        requestBody.filterItems = this.AddFilterItem(requestBody, item.name, item.value)
      }
      if (item.value == -1) {
        requestBody.filterItems = this.clearFilter(requestBody, item.name, "")
        item.isTrue = true
      }
    })

    requestBody.isTraining = false;
    requestBody.sortParams = this.sortResource
    this.resourceRequestService.getResourcePaging(requestBody, this.selectedOption).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.resourceRequestService.handleError)).subscribe(data => {
      this.listRequest = this.tempListRequest = data.result.items;
      this.showPaging(data.result, pageNumber);
    },
      (error) => {
        abp.notify.error(error)
      })
    let rsFilter = this.resetDataSearch(requestBody, request, objFilter)
    request = rsFilter.request
    requestBody = rsFilter.requestBody
  }

  resetDataSearch(requestBody: any, request: any, objFilter: any) {
    objFilter.forEach((item) => {
      if (!item.isTrue) {
        request.filterItems = this.clearFilter(request, item.name, '')
      }
    })
    requestBody.sort = null
    requestBody.sortDirection = null
    this.isLoading = false;

    return {
      request,
      requestBody,
      objFilter
    }
  }

  clearAllFilter() {
    this.filterItems = []
    this.searchText = ''
    this.projectId = -1
    this.requestCode = -1
    this.selectedStatus = 0
    this.changeSortableByName('priority', 'DESC')
    this.sortable = new SortableModel('', 1, '')
    this.refresh()
  }

  onChangeStatus() {
    let status = this.listStatuses.find(x => x.id == this.selectedStatus)
    if (status && status.name == 'DONE') {
      this.sortable = new SortableModel('timeDone', 1, 'DESC')
      this.changeSortableByName('', '')
    }
    this.getDataPage(1);
  }

  sortTable(event: any) {
    this.sortable = event
    this.changeSortableByName(this.sortable.sort, this.sortable.typeSort,this.sortable.sortDirection)
    this.refresh()
  }

  changeSortableByName(sort: string, sortType: string,sortDirection?:number) {
    if(!sortType){
      delete this.sortResource[sort]
    }
    else{
      this.sortResource[sort] = sortDirection
    }
    this.ref.detectChanges()
  }
  // #endregion

  //#region get skills, statuses, levels, priorities
  getAllSkills() {
    this.resourceRequestService.getSkills().subscribe((data) => {
      this.listSkills = data.result;
    })
  }
  getLevels() {
    this.resourceRequestService.getLevels().subscribe(res => {
      this.listLevels = res.result
    })
  }
  getPriorities() {
    this.resourceRequestService.getPriorities().subscribe(res => {
      this.listPriorities = res.result
    })
  }
  getStatuses() {
    this.resourceRequestService.getStatuses().subscribe(res => {
      this.listStatuses = res.result
    })
  }
  getProjectUserRoles() {
    this.resourceRequestService.getProjectUserRoles().subscribe((rs: any) => {
      this.listProjectUserRoles = rs.result
    })
  }

  getAllProject() {
    this.listProjectService.getMyProjects().subscribe(data => {
      this.listProject = data.result;
    })
  }
  getAllRequestCode(){
    this.resourceRequestService.getListRequestCode().subscribe(data=>{
      this.listRequestCode = data.result
    })
  }
  // #endregion

  styleThead(item: any) {
    return {
      width: item.width,
      height: item.height
    }
  }
  public getValueByEnum(enumValue: number, enumObject) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
  viewRecruitment(url) {
    window.open(url, '_blank')
  }
  showDescription(note){
    const show = this.dialog.open(DescriptionPopupComponent, {
      width: "1100px",
      maxHeight: '90vh',
      data: note
    })
  }
  protected delete(item: RequestResourceDto): void {
    abp.message.confirm(
      "Delete this request?",
      "",
      (result: boolean) => {
        if (result) {
          this.resourceRequestService.delete(item.id).pipe(catchError(this.resourceRequestService.handleError)).subscribe(() => {
            abp.notify.success(" Delete request successfully");
            this.refresh();
            this.getAllRequestCode();
          });

        }
      }

    );
  }
  isShowButtonMenuAction(item) {
    return (item.statusName != 'DONE'
      //&& !item.isRecruitmentSend
    )
      || item.statusName != 'CANCELLED'
  }

  isShowBtnCreate() {
    return this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CreateNewRequest)
      || this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CreateNewRequestByPM)
  }

  isShowBtnCancel(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && (this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CancelAllRequest)
        || this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_CancelMyRequest))
  }

  isShowBtnEdit(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_Edit)
  }

  isShowBtnSetDone(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && item.planUserInfo
      && this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_SetDone)
  }

  isShowBtnSendRecruitment(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && (!item.isRecruitmentSend || !item.recruitmentUrl)
      && this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_SendRecruitment)
  }

  isShowBtnDelete(item) {
    return this.isGranted(PERMISSIONS_CONSTANT.ResourceRequest_Delete)
  }
  public sliceUrl(url: string): string{
    if (isNull(url)||isEmpty(url)) {
      return "";
    }
    var regexp = new RegExp(/\/[\d]\d{0,}/g)
    return regexp.exec(url).toString().slice(1);
  }
}

export class THeadTable {
  name: string;
  width?: string = 'auto';
  height?: string = 'auto';
  backgroud_color?: string;
  sortName?: string;;
  defaultSort?: string;
}

export class SendRecruitmentModel {
  id: number;
  name: string;
  dmNote: string;
  pmNote: string;
}

