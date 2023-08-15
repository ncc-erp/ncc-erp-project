import { FormSendRecruitmentComponent } from '@app/modules/delivery-management/delivery/request-resource-tab/form-send-recruitment/form-send-recruitment.component';
import { ResourceRequestTrainingDto } from '@app/service/model/resource-request.dto';
import { difference, isEmpty, isNull, result } from 'lodash-es';
import { FormSetDoneComponent } from '@app/modules/delivery-management/delivery/request-resource-tab/form-set-done/form-set-done.component';
import { SortableComponent, SortableModel } from './../../../../../shared/components/sortable/sortable.component';
import { AppComponentBase } from 'shared/app-component-base';
import { ResourcePlanDto } from './../../../../service/model/resource-plan.dto';
import { PERMISSIONS_CONSTANT } from './../../../../constant/permission.constant';
import { RESOURCE_REQUEST_STATUS } from './../../../../constant/resource-request-status.constant';
import { MatDialog } from '@angular/material/dialog';
import { finalize, catchError } from 'rxjs/operators';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { TrainingRequestDto } from './../../../../service/model/delivery-management.dto';
import { Component, OnInit, Injector, ChangeDetectorRef, ViewChild, ViewChildren, QueryList } from '@angular/core';
import { InputFilterDto } from '@shared/filter/filter.component';
import { SkillDto } from '@app/service/model/list-project.dto';
import { FormPlanUserComponent } from '@app/modules/delivery-management/delivery/request-resource-tab/form-plan-user/form-plan-user.component';
import * as moment from 'moment';
import { IDNameDto } from '@app/service/model/id-name.dto';
import { CreateUpdateTrainingRequestComponent } from './create-update-training-request/create-update-training-request.component';
import { TrainingRequestService } from '@app/service/api/training-request.service';

@Component({
  selector: 'app-training-request-tab',
  templateUrl: './training-request-tab.component.html',
  styleUrls: ['./training-request-tab.component.css']
})
export class TrainingRequestTabComponent extends PagedListingComponentBase<TrainingRequestDto> implements OnInit {

  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Name" },
    { propertyName: 'projectName', comparisions: [0, 6, 7, 8], displayName: "Project Name" },
    { propertyName: 'timeNeed', comparisions: [0, 1, 2, 3, 4], displayName: "Time Need", filterType: 1 },
    { propertyName: 'timeDone', comparisions: [0, 1, 2, 3, 4], displayName: "Time Done", filterType: 1 },
  ];
  public selectedOption: string = "PROJECT"
  public selectedStatus: any = 0
  public listRequest: TrainingRequestDto[] = [];
  public tempListRequest: TrainingRequestDto[] = [];
  public listStatuses: any[] = []
  public listLevels: any[] = []
  public listSkills: SkillDto[] = [];
  public listProjectUserRoles: IDNameDto[] = []
  public listPriorities: any[] = []
  public selectedLevel: any = this.APP_ENUM.UserLevel.Intern_0
  public isAndCondition: boolean = false;
  public skillIds: number[]
  public theadTable: THeadTable[] = [
    { name: '#' },
    { name: 'Priority', sortName: 'priority', defaultSort: 'DESC', width: '88px' },
    { name: 'Project', sortName: 'projectName', defaultSort: '', width: '88px' },
    { name: 'Quantity', sortName: 'quantity', defaultSort: '', width: '95px' },
    { name: 'Skill', width: '5px'  },
    { name: 'Time request', sortName: 'creationTime', defaultSort: '', width: '128px' },
    { name: 'Time need', sortName: 'timeNeed', defaultSort: '', width: '108px' },
    { name: 'Planned resource'},
    { name: 'PM Note' },
    { name: 'HR/DM Note' },
    { name: 'Status' },
    { name: 'Action', width: '-100px' },

  ]
  public isShowModal: string = 'none'
  public modal_title: string
  public strNote: string
  public typePM: string
  public resourceRequestId: number
  public sortable = new SortableModel('', 0, '')

  TrainingRequest_View = PERMISSIONS_CONSTANT.TrainingRequest_View;
  TrainingRequest_PlanNewTrainingForRequest = PERMISSIONS_CONSTANT.TrainingRequest_PlanNewTrainingForRequest;
  TrainingRequest_UpdateTrainingRequestPlan = PERMISSIONS_CONSTANT.TrainingRequest_UpdateTrainingRequestPlan;
  TrainingRequest_RemoveResouceRequestPlan = PERMISSIONS_CONSTANT.TrainingRequest_RemoveTrainingRequestPlan;
  TrainingRequest_SetDone = PERMISSIONS_CONSTANT.TrainingRequest_SetDone;
  TrainingRequest_CancelAllRequest = PERMISSIONS_CONSTANT.TrainingRequest_CancelAllRequest;
  TrainingRequest_CancelMyRequest = PERMISSIONS_CONSTANT.TrainingRequest_CancelMyRequest;
  TrainingRequest_EditPmNote = PERMISSIONS_CONSTANT.TrainingRequest_EditPmNote;
  TrainingRequest_EditDmNote = PERMISSIONS_CONSTANT.TrainingRequest_EditDmNote;
  TrainingRequest_Edit = PERMISSIONS_CONSTANT.TrainingRequest_Edit;
  TrainingRequest_Delete = PERMISSIONS_CONSTANT.TrainingRequest_Delete;
  TrainingRequest_SendRecruitment = PERMISSIONS_CONSTANT.TrainingRequest_SendRecruitment;

  @ViewChildren('sortThead') private elementRefSortable: QueryList<any>;

  constructor(
    private injector: Injector,
    private trainingRequestService: TrainingRequestService,
    private ref: ChangeDetectorRef,
    private dialog: MatDialog
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllSkills()
    this.getLevels()
    this.getPriorities()
    this.getStatuses()
    this.getProjectUserRoles();
    this.refresh();
  }

  ngAfterContentInit(): void {
    this.ref.detectChanges()
  }

  showDetail(item: any) {
    if (this.permission.isGranted(this.DeliveryManagement_TrainingRequest_ViewDetailTrainingRequest)) {
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
    const show = this.dialog.open(CreateUpdateTrainingRequestComponent, {
      data: {
        command: command,
        item: resourceRequest,
        skills: this.listSkills,
        levels: this.listLevels,
        typeControl: 'request'
      },
      width: "700px",
      maxHeight: '90vh',
    })
    show.afterClosed().subscribe(rs => {
      if (!rs) return
      if (command == 'create')
        this.refresh()
      else if (command == 'edit') {
        let index = this.listRequest.findIndex(x => x.id == rs.id)
        if (index >= 0) {
          this.listRequest[index] = rs
        }
        this.refresh();
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

  cancelRequest(request: TrainingRequestDto) {
    abp.message.confirm(
      'Are you sure cancel request for project: ' + request.projectName,
      '',
      (result) => {
        if (result) {
          this.trainingRequestService.cancelResourceRequest(request.id).subscribe(res => {
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
    let res = await this.trainingRequestService.getPlanResource(item.planUserInfo.projectUserId, item.id)
    return res.result
  }

  sendRecruitment(item: ResourceRequestTrainingDto) {
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
    this.trainingRequestService.updateNote(request, this.typePM).subscribe(rs => {
      if (rs.success) {
        abp.notify.success('Update Note Successfully!')
        let index = this.listRequest.findIndex(x => x.id == request.resourceRequestId);
        if (index >= 0) {
          if (this.typePM == 'PM')
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
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    let requestBody: any = request
    requestBody.skillIds = this.skillIds
    requestBody.isAndCondition = this.isAndCondition
    let objFilter = [
      { name: 'status', isTrue: false, value: this.selectedStatus },
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

    requestBody.isTraining = true;
    if (this.sortable.sort) {
      requestBody.sort = this.sortable.sort;
      requestBody.sortDirection = this.sortable.sortDirection
    }

    this.trainingRequestService.getResourcePaging(requestBody, this.selectedOption).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.trainingRequestService.handleError)).subscribe(data => {
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
    requestBody.skillIds = null
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
    this.skillIds = []
    this.selectedLevel = this.APP_ENUM.UserLevel.Intern_0
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
    this.changeSortableByName(this.sortable.sort, this.sortable.typeSort)
    this.refresh()
  }

  changeSortableByName(sort: string, sortType: string) {
    this.elementRefSortable.forEach((item) => {
      if (item.childValue.sort != sort) {
        item.childValue.typeSort = ''
      }
      else {
        item.childValue.typeSort = sortType
      }
    })
    this.ref.detectChanges()
  }
  // #endregion

  //#region get skills, statuses, levels, priorities, quantities
  getAllSkills() {
    this.trainingRequestService.getSkills().subscribe((data) => {
      this.listSkills = data.result;
    })
  }

  getLevels() {
    this.trainingRequestService.getTrainingLevels().subscribe(res => {
      this.listLevels = res.result
    })
  }

  getPriorities() {
    this.trainingRequestService.getPriorities().subscribe(res => {
      this.listPriorities = res.result
    })
  }

  getStatuses() {
    this.trainingRequestService.getStatuses().subscribe(res => {
      this.listStatuses = res.result
    })
  }

  getProjectUserRoles() {
    this.trainingRequestService.getProjectUserRoles().subscribe((rs: any) => {
      this.listProjectUserRoles = rs.result
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

  protected delete(item: TrainingRequestDto): void {
    abp.message.confirm(
      "Delete this request?",
      "",
      (result: boolean) => {
        if (result) {
          this.trainingRequestService.delete(item.id).pipe(catchError(this.trainingRequestService.handleError)).subscribe(() => {
            abp.notify.success(" Delete request successfully");
            this.refresh();
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
    return this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_CreateNewRequest)
      || this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_CreateNewRequestByPM)
  }

  isShowBtnCancel(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && (this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_CancelAllRequest)
        || this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_CancelMyRequest))
  }

  isShowBtnEdit(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_Edit)
  }

  isShowBtnSetDone(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && item.planUserInfo
      && this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_SetDone)
  }

  isShowBtnSendRecruitment(item) {
    return item.status == RESOURCE_REQUEST_STATUS.PENDING
      && (!item.isRecruitmentSend || !item.recruitmentUrl)
      && this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_SendRecruitment)
  }

  isShowBtnDelete(item) {
    return this.isGranted(PERMISSIONS_CONSTANT.TrainingRequest_Delete)
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

