import { AppComponentBase } from '@shared/app-component-base';

import { catchError } from 'rxjs/operators';

import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { ClientDto } from '@app/service/model/list-project.dto';
import { UserDto } from '@shared/service-proxies/service-proxies';
import { ProjectDto } from '@app/service/model/list-project.dto';
import { CurrencyDto } from '@app/service/model/currency.dto';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { UserService } from '@app/service/api/user.service';
import { ClientService } from '@app/service/api/client.service';
import { ListProjectService } from '@app/service/api/list-project.service';
import { CurrencyService } from '@app/service/api/currency.service';

@Component({
  selector: 'app-training-project-general',
  templateUrl: './training-project-general.component.html',
  styleUrls: ['./training-project-general.component.css']
})
export class TrainingProjectGeneralComponent extends AppComponentBase implements OnInit {
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
  Projects_TrainingProjects_ProjectDetail_TabGeneral_View = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabGeneral_View;
  Projects_TrainingProjects_ProjectDetail_TabGeneral_Edit = PERMISSIONS_CONSTANT.Projects_TrainingProjects_ProjectDetail_TabGeneral_Edit;
  // PmManager_Project_Update = PERMISSIONS_CONSTANT.PmManager_Project_Update;
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
        abp.notify.success("Edited: " + this.project.name);
        if(res.result == "update-only-project-tool"){
          abp.notify.success("Edited project: "+this.project.name);
        }
        else if(res.result == null || res.result == ""){
          abp.message.success(`<p>Edited project name <b>${this.project.name}</b> in <b>PROJECT TOOL</b> successful!</p> 
          <p style='color:#28a745'>Edited project name <b>${this.project.name}</b> in <b>TIMESHEET TOOL</b> successful!</p>`, 
         'Edit project result',true);
        }
        else{
          abp.message.error(`<p>Edited project <b>${this.project.name}</b> in <b>PROJECT TOOL</b> successful!</p> 
          <p style='color:#dc3545'>${res.result}</p>`, 
          'Edit project result',true);
        }
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
