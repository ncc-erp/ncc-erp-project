import { result } from 'lodash-es';
import { CurrencyService } from './../../../../../service/api/currency.service';
import { CurrencyDto } from './../../../../../service/model/currency.dto';
import { PERMISSIONS_CONSTANT } from './../../../../../constant/permission.constant';
import { ClientService } from './../../../../../service/api/client.service';
import { UserDto } from './../../../../../../shared/service-proxies/service-proxies';
import { UserService } from './../../../../../service/api/user.service';
import { AppComponentBase } from '@shared/app-component-base';
import { ProjectDto, ClientDto } from './../../../../../service/model/list-project.dto';
import { catchError } from 'rxjs/operators';
import { ListProjectService } from './../../../../../service/api/list-project.service';
import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';

@Component({
  selector: 'app-list-project-general',
  templateUrl: './list-project-general.component.html',
  styleUrls: ['./list-project-general.component.css']
})
export class ListProjectGeneralComponent extends AppComponentBase implements OnInit {
  public searchClient: string = "";
  public searchPM: string = "";
  public readMode: boolean = true;
  private projectId: number;
  public projectTypeList: string[] = Object.keys(this.APP_ENUM.ProjectType)
  public projectStatusList: string[] = Object.keys(this.APP_ENUM.ProjectStatus)
  public clientList: ClientDto[] = [];
  public pmList: UserDto[] = [];
  public project = {} as ProjectDto;
  public currencyList: CurrencyDto[]=[];
  public isShowCurrencyCharge: boolean = false;
  public chargeTypeList : string[]= Object.keys(this.APP_ENUM.ChargeType)
  Projects_OutsourcingProjects_ProjectDetail_TabGeneral_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_View;
  Projects_OutsourcingProjects_ProjectDetail_TabGeneral_Edit = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_Edit;
  constructor(injector: Injector, private userService: UserService, private clientService: ClientService,
     private projectService: ListProjectService, private route: ActivatedRoute, private currencyService: CurrencyService) {
    super(injector);
  }
  ngOnInit(): void {
    this.projectId = Number(this.route.snapshot.queryParamMap.get("id"));
    this.getProjectDetail();
    this.getClient();
    this.getPm();
    this.getAllCurrency();
  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
  public getProjectDetail(): void {
    this.projectService.getProjectById(this.projectId).pipe(catchError(this.projectService.handleError)).subscribe(data => {
      this.project = data.result;
      this.getShowCurrencyCharge(this.project.projectType);
    })
  }

  public getShowCurrencyCharge(projectType: number) {
    for (const key in this.APP_ENUM.CurrencyChargeProjectType) {
      if (this.APP_ENUM.CurrencyChargeProjectType[key] == projectType) {
        this.isShowCurrencyCharge = true;
        return;
      }
    }
  }
  public editRequest(): void {
    this.readMode = false
  }
  public getClient(): void {
    this.clientService.getAll().pipe(catchError(this.clientService.handleError)).subscribe(data => {
      this.clientList = data.result;
    })
  }
  public getPm(): void {
    this.userService.GetAllUserActive(true).pipe(catchError(this.userService.handleError)).subscribe(data => {
      this.pmList = data.result;
    })
  }
  public getAllCurrency(){
    this.currencyService.getAll().pipe(catchError(this.currencyService.handleError)).subscribe(data => {
      this.currencyList = data.result;
    })

  }

  public saveAndClose(): void {
    this.isLoading=true;
    this.project.startTime = moment(this.project.startTime).format("YYYY-MM-DD");
    if (this.project.endTime) {
      this.project.endTime = moment(this.project.endTime).format("YYYY-MM-DD");
    }
      this.isLoading = true;
      // this.project.status = 0;
      this.projectService.update(this.project).pipe(catchError(this.projectService.handleError)).subscribe((res) => {
        abp.notify.success("updated: " + this.project.name);
        this.readMode = true;
        this.isLoading=false;
        this.getProjectDetail();
      }, () => this.isLoading = false);
  }
  checkValue(e){
    if(e.checked == true){
      this.project.chargeType = 0;
    }
  }

  changeTextProjectType(projectType: string) {
    return projectType === 'TAM' ? 'T&M' : projectType
  }


}
