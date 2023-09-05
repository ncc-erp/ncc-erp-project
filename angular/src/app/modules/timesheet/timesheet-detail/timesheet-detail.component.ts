import { MatMenuTrigger } from '@angular/material/menu';
import { ViewBillComponent } from './view-bill/view-bill.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { CreateEditTimesheetDetailComponent } from './create-edit-timesheet-detail/create-edit-timesheet-detail.component';
import { ExportInvoiceComponent } from './export-invoice/export-invoice.component';
import { ActivatedRoute } from '@angular/router';
import { TimesheetDetailDto,TotalAmountByCurrencyDto} from './../../../service/model/timesheet.dto';
import { Component, OnInit, Injector, ViewChild, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { InputFilterDto } from '@shared/filter/filter.component';
import { TimesheetService } from '@app/service/api/timesheet.service'
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { ImportFileTimesheetDetailComponent } from './import-file-timesheet-detail/import-file-timesheet-detail.component';
import * as FileSaver from 'file-saver';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { CreateInvoiceComponent } from './create-invoice/create-invoice.component';
import { UserService } from '@app/service/api/user.service';
import { ClientService } from '@app/service/api/client.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EditTimesheetProjectDialogComponent } from './edit-timesheet-project-dialog/edit-timesheet-project-dialog/edit-timesheet-project-dialog.component';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { ExchangeRateComponent } from './exchange-rate/exchange-rate/exchange-rate.component';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ActiveTimesheetProjectComponent } from './active-timesheet-project/active-timesheet-project.component';


@Component({
  selector: 'app-timesheet-detail',
  templateUrl: './timesheet-detail.component.html',
  styleUrls: ['./timesheet-detail.component.css']
})
export class TimesheetDetailComponent extends PagedListingComponentBase<TimesheetDetailDto> implements OnInit {
  Projects_OutsourcingProjects_ProjectDetail = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail;
  requestBody: PagedRequestDto
  pageNum: number
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.requestBody = request
    this.pageNum = pageNumber
    let objFilter = [
      {name: 'pmId', isTrue: false, value: this.pmId},
      {name: 'clientId', isTrue: false, value: this.clientId},
    ];

    objFilter.forEach((item) => {
      if(!item.isTrue){
        request.filterItems = this.AddFilterItem(request, item.name, item.value)
      }
      if(item.value == -1){
        request.filterItems = this.clearFilter(request, item.name, "")
        item.isTrue = true
      }
    })
    this.timesheetProjectService.GetTimesheetDetail(this.timesheetId, request).pipe(catchError(this.timesheetProjectService.handleError)).pipe(finalize(()=>finishedCallback()))
      .subscribe((res) => {
        let timesheetDetaiList = res.result.listTimesheetDetail;
        this.TimesheetDetaiList = timesheetDetaiList.items.map(el => {
          el.projectBillInfomation.map(item => {
            return {...item, isShow: false}
          })
          return el;
        });

        this.listTotalAmountByCurrency = res.result.listTotalAmountByCurrency;
        this.pageNumber = timesheetDetaiList.totalCount;

        this.showPaging(timesheetDetaiList, pageNumber);
        this.projectTimesheetDetailId = timesheetDetaiList.items.map(el => { return el.projectId })
        objFilter.forEach((item) => {
          if(!item.isTrue){
            request.filterItems = this.clearFilter(request, item.name, '')
          }
        })
      })
  }
  protected delete(item: TimesheetDetailDto): void {
    this.menu.closeMenu();
    abp.message.confirm(
      "Delete TimeSheet " + item.projectName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.timesheetProjectService.delete(item.id).pipe(catchError(this.timesheetProjectService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Project Timesheet " + item.projectName);
            this.refresh();
          });
        }
      }
    );
  }

  @ViewChildren('checkboxExportInvoice') private elementRefCheckbox : QueryList<any>;
  public TimesheetDetaiList: TimesheetDetailDto[] = [];
  public tempTimesheetDetaiList: TimesheetDetailDto[] = [];
  public requestId: any;
  public projectTimesheetDetailId: any;
  public searchText: string = "";
  public timesheetId: any;
  public isActive: boolean;
  public createdInvoice: boolean;
  public listExportInvoice = [];
  public listExportInvoiceChargeType = [];
  public isMonthlyToDaily = false;
  public clientId: number = -1;
  public isShowButtonAction: boolean;
  public pmId = -1;
  public pmList: any[] = [];
  public searchPM: string = "";
  public clientList: any[]= []
  public searchClient: string = ""
  public chargeType = ['d','m','h']
  public titleTimesheet: string = ''
  public meId: number;
  public updateAction = UpdateAction;
  public currency: string = "";
  public clientIdInvoice: number = -1;
  public totalAmount: number;
  public listTotalAmountByCurrency: TotalAmountByCurrencyDto[] = [];
  public sending: boolean = false;
  public canExportInvoice = false;
  public listActiveTimesheetProject = [];
  @ViewChild(MatMenuTrigger)
  menu: MatMenuTrigger
  contextMenuPosition = { x: '0', y: '0' }
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'pmFullName', displayName: "PM Name", comparisions: [0, 6, 7, 8] },
    { propertyName: 'projectName', displayName: "Project Name", comparisions: [0, 6, 7, 8] },
    { propertyName: 'hasFile', displayName: "Has file", comparisions: [0], filterType: 2 },
    { propertyName: 'isComplete', displayName: "Status", comparisions: [0], filterType: 5 },
    { propertyName: 'clientName', displayName: "Client Name", comparisions: [0, 6, 7, 8] },
    { propertyName: 'clientCode', displayName: "Client Code", comparisions: [0, 6, 7, 8] },
  ];

  Timesheets_TimesheetDetail_View = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_View;
  Timesheets_TimesheetDetail_ViewBillRate = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ViewBillRate;
  Timesheets_TimesheetDetail_AddProjectToTimesheet = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_AddProjectToTimesheet;
  Timesheets_TimesheetDetail_UploadTimesheetFile = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UploadTimesheetFile;
  Timesheets_TimesheetDetail_ExportInvoice = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ExportInvoice;
  Timesheets_TimesheetDetail_ExportInvoiceAllProject = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ExportInvoiceAllProject;
  Timesheets_TimesheetDetail_UpdateNote = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateNote;
  Timesheets_TimesheetDetail_Delete = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_Delete;
  Timesheets_TimesheetDetail_ViewAll = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ViewAll;
  Timesheets_TimesheetDetail_ViewMyProjectOnly = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ViewMyProjectOnly;
  Timesheets_TimesheetDetail_UpdateTimsheet = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateTimsheet;
  Timesheets_TimesheetDetail_UpdateBill = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_UpdateBill;
  Timesheets_TimesheetDetail_ExportInvoiceForTax = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_ExportInvoiceForTax;
  Timesheets_TimesheetDetail_EditInvoiceInfo = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_EditInvoiceInfo;
  Timesheets_TimesheetDetail_SendInvoiceToFinfast = PERMISSIONS_CONSTANT.Timesheets_TimesheetDetail_SendInvoiceToFinfast;
  constructor(
    private timesheetService: TimesheetService,
    public timesheetProjectService: TimesheetProjectService,
    private projectUserBillService: ProjectUserBillService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    injector: Injector,
    private ref: ChangeDetectorRef,
    private userService: UserService,
    private clientService: ClientService,
    private _modalService: BsModalService,
    private sanitizer: DomSanitizer
  ) {
    super(injector)


  }
  ngOnInit(): void {
    this.meId = Number(this.appSession.userId);
    if(!this.permission.isGranted(this.Timesheets_TimesheetDetail_ViewAll)){
      this.pmId = this.meId
    }
    this.timesheetId = this.route.snapshot.queryParamMap.get('id');
    this.titleTimesheet = this.route.snapshot.queryParamMap.get('name')
    this.isActive = this.route.snapshot.queryParamMap.get('isActive') == 'true' ? true : false;
    this.createdInvoice = this.route.snapshot.queryParamMap.get('createdInvoice') == 'true' ? true : false;
    this.getAllPM();
    this.getAllClient()
    this.refresh();
    //this.showButtonAction();
  }
  ngAfterContentChecked() {
    this.ref.detectChanges();
  }
  ngAfterViewInit(){
    this.elementRefCheckbox.changes.subscribe(c => {
      c.toArray().forEach(element => {
        if(this.listExportInvoice.includes(element.value.projectId)){
          element._checked = true
        }
        else{
          element._checked = false
        }
      });
    })
  }
  showDialog(command: String, Timesheet: any): void {
    let timesheetDetail = {};
    if (command == "edit") {
      timesheetDetail = {
        projectId: Timesheet.projectId,
        timesheetId: Timesheet.timesheetId,
        clientName: Timesheet.clientName,
        projectName: Timesheet.projectName,
        note: Timesheet.note,
        id: Timesheet.id,
        projectBillInfomation: Timesheet.projectBillInfomation,
      }

    }
    const show = this.dialog.open(CreateEditTimesheetDetailComponent, {
      data: {
        item: timesheetDetail,
        command: command,
        projectTimesheetDetailId: this.projectTimesheetDetailId,
      },
      width: "700px",
      disableClose: true,
    });
    show.afterClosed().subscribe(res => {
      if (res) {
        this.refresh()
      }
    })
  }


  public isEnablePMFilter(){
    return this.permission.isGranted(this.Timesheets_TimesheetDetail_ViewAll)
  }
  public searchProjectTS(){
    if (this.isEnablePMFilter() && this.searchText != ""){
      this.pmId = -1;
    }
    this.pageNumber = 1;
    this.refresh();
  }


  // reloadTimesheetFile(id) {
  //   this.timesheetProjectService.GetTimesheetDetail(this.timesheetId, this.requestBody).pipe(catchError(this.timesheetProjectService.handleError))
  //     .subscribe((data: PagedResultResultDto) => {
  //       this.TimesheetDetaiList = data.result.items;
  //       if (!this.TimesheetDetaiList.filter(timesheet => timesheet.id == id)[0].file) {
  //         setTimeout(() => {
  //           this.reloadTimesheetFile(id)
  //         }, 1000)
  //       }
  //       else {
  //         this.showPaging(data.result, this.pageNum);
  //         this.projectTimesheetDetailId = data.result.items.map(el => { return el.projectId })
  //         abp.notify.success("import file successfull")
  //       }
  //     })
  // }

  createTimeSheet() {
    this.showDialog('create', {})
  }
  editTimesheet(timesheet: TimesheetDetailDto) {
    this.menu.closeMenu();
    this.showDialog("edit", timesheet);
  }

  showDialogUpdateFile(command: string) {
  }
  importExcel(id: any) {
    const dialogRef = this.dialog.open(ImportFileTimesheetDetailComponent, {
      data: { id: id, width: '500px' }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // this.reloadTimesheetFile(result)
        this.refresh()
      }
    });
  }
  DeleteFile(item: any) {
    abp.message.confirm(
      "Delete File " + item.file + "?",
      "",
      async (result: boolean) => {
        if (result) {
          this.timesheetProjectService.UpdateFileTimeSheetProject(null, item.id).subscribe(
            (res) => {
              abp.notify.success("Deleted File Success");
              item.file = null
              item.hasFile = false
            },
            (err) => {
              abp.notify.error(err)
            },
          );
        }
      }
    );
  }
  search() {
    this.TimesheetDetaiList = this.tempTimesheetDetaiList.filter((item) => {
      return item.projectName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        item.file?.toLowerCase().includes(this.searchText.toLowerCase());
    });
  }

  importFile(id: number) {
    this.timesheetProjectService.DownloadFileTimesheetProject(id).subscribe(data => {
    })
  }

  downloadFile(projectTimesheet: any) {
    this.timesheetProjectService.GetTimesheetFile(projectTimesheet.id).subscribe(data => {
      const file = new Blob([this.s2ab(atob(data.result.data))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, data.result.fileName);
    })

  }
  s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }

  createInvoice() {
    const show = this.dialog.open(CreateInvoiceComponent, {
      data: {
        timeSheetId: this.timesheetId,
        title: "Create Invoice"
      },
      width: "700px",
      disableClose: true,
    });
    show.afterClosed().subscribe(res => {
      if (res) {
        this.reloadComponent();
        // this.refresh();
      }
    })
  }

  exportInvoice() {
    const show = this.dialog.open(CreateInvoiceComponent, {
      data: {
        timeSheetId: this.timesheetId,
        title: "Export Invoice"
      },
      width: "700px",
      disableClose: true,
    });
    show.afterClosed().subscribe(res => {
      if (res) {
        this.reloadComponent();
        // this.refresh();
      }
    })
  }
  showActions(e) {
    e.preventDefault();
    this.contextMenuPosition.x = e.clientX + 'px';
    this.contextMenuPosition.y = e.clientY + 'px';
    this.menu.openMenu();
  }
  public reloadComponent() {
    this.router.navigate(['app/timesheetDetail'], {
      queryParams: {
        id: this.timesheetId,
        createdInvoice: true,
        isActive: false
      }
    })
    this.isActive = false;
    this.createdInvoice = true;
  }
  public viewBillDetail(bill,action) {
    this.menu.closeMenu()
    const show = this.dialog.open(ViewBillComponent, {
      width: "95%",
      data: {
        billInfo:bill,
        action: action
      },

    })
    show.afterClosed().subscribe((res) => {
      this.refresh();
    })
  }
  mouseEnter(item) {
    item.showIcon = true

  }
  mouseLeave(item) {
    item.showIcon = false
  }

  exportInvocie(item: any,exportInvoiceMode) {
    this.timesheetProjectService.exportInvoice(this.timesheetId, item.projectId,exportInvoiceMode).pipe(catchError(this.timesheetProjectService.handleError)).subscribe(data => {
      const file = new Blob([this.s2ab(atob(data.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, data.result.fileName);
      abp.notify.success("Export Invoice Successfully!");
    })
  }

  exportInvocieAsPDF(item: any,exportInvoiceMode) : void {
    this.timesheetProjectService.exportInvoiceAsPDF(this.timesheetId, item.projectId,exportInvoiceMode).pipe(catchError(this.timesheetProjectService.handleError)).subscribe(res => {
    this.dialog.open(ExportInvoiceComponent,{
      data: {
        fileName: res.result.fileName,
        html: res.result.html
      },
      width: '85%',
      maxWidth: '85%',
      panelClass: 'invoice-dialog',
    });
  });
  }


  exportQuickInvoiceForTax(item: any,exportInvoiceMode) {
    let invoiceExcelDto = {
      timesheetId: this.timesheetId,
      projectIds: [item.projectId],
      mode: exportInvoiceMode
    }
    this.timesheetProjectService.exportInvoiceForTax(invoiceExcelDto).pipe(catchError(this.timesheetProjectService.handleError)).subscribe(data => {
      const file = new Blob([this.s2ab(atob(data.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, data.result.fileName);
      abp.notify.success("Export Invoice For Tax Successfully!");
    })
  }

  addProjectToExport(event, item) {
    let chargeTypeProject = item.projectBillInfomation.find(s => s.chargeType == this.APP_ENUM.ChargeType.Monthly);
    if (!event.checked) {
      let index = this.listExportInvoice.indexOf(event.source.value.projectId);
      let indexChargeType = this.listExportInvoiceChargeType.indexOf(chargeTypeProject ? this.APP_ENUM.ChargeType.Monthly : event.source.value.chargeType);
      if (index > -1) {
        this.listExportInvoice.splice(index, 1);
      }
      if (indexChargeType > -1) {
        this.listExportInvoiceChargeType.splice(indexChargeType, 1);
      }
      this.listActiveTimesheetProject.splice(index, 1);
      this.checkExportInvoice(this.listActiveTimesheetProject)
    }
    else {
      let checkClientId = event.source.value.clientId;
      let checkCurrency = event.source.value.currency;
      this.currency = checkCurrency;
      this.clientIdInvoice = checkClientId;
      this.listExportInvoice.push(event.source.value.projectId);
      this.listExportInvoiceChargeType.push(chargeTypeProject ? this.APP_ENUM.ChargeType.Monthly : event.source.value.chargeType);
      this.listActiveTimesheetProject.push(item);
      this.checkExportInvoice(this.listActiveTimesheetProject)
    }
    if (this.listExportInvoiceChargeType.indexOf(this.APP_ENUM.ChargeType.Monthly) !== -1) {
      this.isMonthlyToDaily = true;
    }
    else {
      this.isMonthlyToDaily = false;
    }
  }
  checkExportInvoice(listActiveTimesheetProject) {
    if (listActiveTimesheetProject.length > 0) {
      let countClient = listActiveTimesheetProject.reduce((r: any, a: any) => {
        r[a.clientId] = r[a.clientId] || [];
        r[a.clientId].push(a);
        return r;
      }, {});
      let countCurrency = listActiveTimesheetProject.reduce((r: any, a: any) => {
        r[a.currency] = r[a.currency] || [];
        r[a.currency].push(a);
        return r;
      }, {});
      if (Object.keys(countClient).length > 1 || Object.keys(countCurrency).length) {
        this.canExportInvoice = false;
      }
      if (Object.keys(countClient).length == 1 && Object.keys(countCurrency).length == 1) { this.canExportInvoice = true }
    }
    else {
      this.canExportInvoice = false;
    }
  }

  exportInvoiceClient(exportInvoiceMode) {
    let invoiceExcelDto = {
      timesheetId: this.timesheetId,
      projectIds: this.listExportInvoice,
      mode: exportInvoiceMode
    }
    this.timesheetProjectService.exportInvoiceClient(invoiceExcelDto).subscribe((res) => {
      const file = new Blob([this.s2ab(atob(res.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      this.refresh();
      this.listExportInvoice=[];
      FileSaver.saveAs(file, res.result.fileName);
      abp.notify.success("Export Invoice Successfully!");
    })
  }

  // Use this function when you want to preview or edit template
  public exportInvoiceClientAsPDF(exportInvoiceMode): void {
    let invoicePdfDto = {
      timesheetId: this.timesheetId,
      projectIds: this.listExportInvoice,
      mode: exportInvoiceMode
    };
    this.timesheetProjectService.exportInvoiceClientAsPDF(invoicePdfDto).subscribe((res) => {
    this.dialog.open(ExportInvoiceComponent,{
      data: {
        fileName: res.result.fileName,
        html: res.result.html
      },
      width: '85%',
      maxWidth: '85%',
      panelClass: 'invoice-dialog',
    });
  });
  }

  exportInvoiceForTax(exportInvoiceMode) {
    let invoiceExcelDto = {
      timesheetId: this.timesheetId,
      projectIds: this.listExportInvoice,
      mode: exportInvoiceMode
    }
    this.timesheetProjectService.exportInvoiceForTax(invoiceExcelDto).subscribe((res) => {
      const file = new Blob([this.s2ab(atob(res.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      this.refresh();
      this.listExportInvoice=[];
      FileSaver.saveAs(file, res.result.fileName);
      abp.notify.success("Export Invoice For Tax Successfully!");
    })
  }

  public getAllPM(): void {
    this.timesheetProjectService.getAllPM().pipe(catchError(this.userService.handleError))
      .subscribe(data => {
        this.pmList = data.result;
      })
  }
  public getAllClient(): void{
    this.clientService.getAllClient()
    .subscribe((res) =>{
      this.clientList = res.result
    })
  }
  requiredFile(item){
    if(item.requireTimesheetFile && !item.hasFile)
      return 'bd-red';
    return ''
  }
  getChargeType(index){
    if(index != null && index >= 0)
      return '/'+this.chargeType[index]
    return ''
  }
  covertMoneyType(money){
    return (
      money.toLocaleString("vi", {
        currency: "VND",
      })
    );
  }

  updateTimesheetProject(item) {
    let editTimesheetProjectDialog: BsModalRef;
    editTimesheetProjectDialog = this._modalService.show(EditTimesheetProjectDialogComponent, {
      class: 'modal',
      initialState: {
        id: item.id,
        invoiceNumber: item.invoiceNumber,
        workingDay: item.workingDay,
        projectName: item.projectName,
        transferFee: item.transferFee,
        discount: item.discount,
        isMainProjectInvoice: item.isMainProjectInvoice,
        mainProjectId: item.mainProjectId,
        subProjects: item.subProjects,
        subProjectIds: item.subProjectIds,
        projectId: item.projectId
      },

    });

    editTimesheetProjectDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }


  viewProjectDetail(project){
    let routingToUrl:string = "/app/list-project-detail/list-project-general"
    const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], { queryParams: {
      id: project.projectId,
      projectName: project.projectName,
      projectCode: project.projectCode} }));
      window.open(url, '_blank');
  }

  public isShowExportInvoiceMonthlyToDaily(item: TimesheetDetailDto){
    return this.isGranted(this.Timesheets_TimesheetDetail_ExportInvoice)
    && this.hasChargeTypeMonthly(item)
  }
  public isShowExportInvoiceForTaxMonthlyToDaily(item: TimesheetDetailDto){
    return this.isGranted(this.Timesheets_TimesheetDetail_ExportInvoiceForTax)
    && this.hasChargeTypeMonthly(item)
  }

  private hasChargeTypeMonthly(item: TimesheetDetailDto){
      return item
      && item.projectBillInfomation
      && item.projectBillInfomation.find(s => s.chargeType == this.APP_ENUM.ChargeType.Monthly)
  }

  getColorByCurrency(currencyName: string){
    switch(currencyName) {
      case 'VND':
        return 'blue';
      case 'USD':
        return 'red';
      case 'EURO':
          return 'orange';
      case 'YEN':
            return '#42032C';
      case 'BATH':
        return '#17a2b8';
      default:
        return '#F675A8';
    }
  }

  sendInvoiceToFinfast() {
    this.sending = true;
    abp.message.confirm("Send invoice to finfast", "", (result) => {
      if (result) {
        this.timesheetProjectService.checkTimesheetProjectSetting(this.timesheetId).subscribe(rs => {
          if (rs.result.length) {
            abp.message.warn(rs.result);
            this.sending = false;
            return;
          }
          this.timesheetProjectService.sendInvoiceToFinfast(this.timesheetId).subscribe(rs => {
            if (rs.result.isSuccess) {
              abp.message.success("Invoice sended to finfast")
            }
            else {
              abp.message.error(rs.result.message)
            }
          })
        })
      }
      this.sending = false;
    })
  }
  confirmExport() {
    const show = this.dialog.open(ExchangeRateComponent, {
      width: "30%",
      data: {
        timesheetId: this.timesheetId,
        timesheetName: this.titleTimesheet,
        currencyInfor: this.listTotalAmountByCurrency.map((x:any) => ({ currencyName: x.currencyName, exchangeRate: 0 })),
      },
      autoFocus: false
    })
    show.afterClosed().subscribe((res) => {
      this.refresh();
    })
  }
  selectActiveProject() {
    const show = this.dialog.open(ActiveTimesheetProjectComponent, {
      width: "60%",
      data: {
        timesheetId: this.timesheetId,
        listActiveTimesheetProject: this.listActiveTimesheetProject
      }
    })
    show.afterClosed().subscribe(() => {
      this.refresh();
    })
  }
}
export const UpdateAction = {
  UpdateBillInfo: 1,
  UpdateTimesheet: 2
}
