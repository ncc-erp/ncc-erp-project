import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { GeneralInformationService } from '@app/service/api/general-informatin.service';
import { projectUserBillDto, ProjectRateDto, projectUserDto} from '@app/service/model/project.dto';
import { UpdateInvoiceDto } from '@app/service/model/updateInvoice.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { ProjectUserService } from '@app/service/api/project-user.service';
import { PMReportProjectService } from '@app/service/api/pmreport-project.service';
import * as moment from 'moment';
import * as echarts from 'echarts';

@Component({
  selector: 'app-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.css']
})
export class GeneralInformationComponent  extends AppComponentBase implements OnInit{

  public userBillList: projectUserBillDto[] = [];
  public filteredUserBillList: projectUserBillDto[] = [];
  public isShowUserBill: boolean = false;
  private projectId: number
  public userBillCurrentPage: number = 1
  public maxBillUserCurrentPage = 20;
  public totalBillList: number;
  public selectedIsCharge: string = "Charge" ;
  public rateInfo = {} as ProjectRateDto;
  public updateInvoiceDto: UpdateInvoiceDto = {} as UpdateInvoiceDto;

  public projectUserList: projectUserDto[] = [];
  public viewHistory: boolean = false;
  public projectUserProcess: boolean = false;
  public isShowProjectUser: boolean = true;
  public userListCurrentPage = 1;
  public maxUserCurrentPage= 20;
  public totalNormalWorkingTime = 0 
  public totalNormalWorkingTimeOfWeekly = 0
  public totalNormalWorkingTimeStandard = 0
  public totalOverTime = 0
  public overTimeNoCharge = 0
  public mondayOf5weeksAgo: any
  public lastWeekSunday: any
  projectCode!: string
  normalTime: string =''
  otTime: string = ''
 


  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport;
  Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = PERMISSIONS_CONSTANT.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View;

  
  constructor( private router: Router,
    private projectUserService: ProjectUserService,
    private generalInformationService: GeneralInformationService,
    private projectUserBillService: ProjectUserBillService,
    private route: ActivatedRoute,
    public pmReportProjectService: PMReportProjectService,
    injector: Injector,
) {
    super(injector)
    this.projectId = Number(this.route.snapshot.queryParamMap.get("id"));
    this.projectCode = this.route.snapshot.queryParamMap.get("projectCode")
  }

  ngOnInit(): void {
    this.getUserBill();
    this.getProjectBillInfo();
    this.getProjectUser()
    this.getCurrentDate()
  }


  getCurrentDate() {
    let currentDate = new Date()
    currentDate.setDate(currentDate.getDate() - (currentDate.getDay() + 6) % 7);
    currentDate.setDate(currentDate.getDate() - 7);
    this.mondayOf5weeksAgo = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
    this.mondayOf5weeksAgo = moment(this.mondayOf5weeksAgo.setDate(this.mondayOf5weeksAgo.getDate() - 28)).format("YYYY-MM-DD")
    this.lastWeekSunday = moment(new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate() + 6)).format("YYYY-MM-DD");
  }

  public getRate() {
    this.projectUserBillService.getRate(this.projectId).subscribe(data => {
      this.rateInfo = data.result;
    })
  }

  private getUserBill(): void {
    this.generalInformationService.getBillAccount(this.projectId).pipe(catchError(this.projectUserBillService.handleError)).subscribe(data => {
      this.userBillList = data.result
      this.filteredUserBillList = this.userBillList.filter(bill => bill.isActive === true);
    })
  }

  filterCharge(){
    if (this.selectedIsCharge === 'All') {
      this.filteredUserBillList = this.userBillList;
    } else if (this.selectedIsCharge === 'Charge') {
      this.filteredUserBillList = this.userBillList.filter(bill => bill.isActive === true);
    } else if (this.selectedIsCharge === 'Not Charge') {
      this.filteredUserBillList = this.userBillList.filter(bill => bill.isActive === false);
    }
  }

  public changePageSizeCurrent()
  {
    this.userListCurrentPage = 1
  }

  public changePageSizeBill()
  {
    this.userBillCurrentPage = 1
  }

  public getProjectBillInfo(){
    this.projectUserBillService.getBillInfo(this.projectId).subscribe((rs) => {
      this.rateInfo = {
        currencyName: rs.result.currencyName
      } as ProjectRateDto
    })
  }

  public routingProject(projectId, projectName, projectCode){
    let routingToUrl:string = (this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport)
    && this.permission.isGranted(this.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View))
   ? "/app/list-project-detail/weeklyreport" : "/app/list-project-detail/list-project-general"

   const url = this.router.serializeUrl(this.router.createUrlTree([routingToUrl], { queryParams: {
     id: projectId,
     projectName: projectName,
     projectCode: projectCode} }));
      window.open(url, '_blank');
  }

  getLastMondayWeek() {
    var d = new Date();
    d.setDate(d.getDate() - (d.getDay() + 6) % 7);
    d.setDate(d.getDate() - 7);
    let lastWeekMonday  = moment(new Date(d.getFullYear(), d.getMonth(), d.getDate())).format("YYYY-MM-DD")
    return lastWeekMonday
  }

  private getProjectUser() {
    this.projectUserService.getCurrentResource(this.projectId, this.viewHistory).pipe(catchError(this.projectUserService.handleError)).subscribe(data => {
      this.totalNormalWorkingTime = 0
      this.totalNormalWorkingTimeOfWeekly = 0
      this.totalNormalWorkingTimeStandard = 0
      this.totalOverTime = 0
      this.overTimeNoCharge = 0
      this.projectUserList = data.result;
      this.projectUserList.forEach((user: projectUserDto) => {
        this.GetTimesheetWeeklyChartOfUserInProject(this.projectCode, user, this.mondayOf5weeksAgo, this.lastWeekSunday)
        this.GetTimesheetOfUserInProjectNew(this.projectCode, user, this.getLastMondayWeek(), this.lastWeekSunday)
      })
    })
  }

  
  public filterProjectUser(event) {
    this.viewHistory = event.checked;
    this.getProjectUser()
  }



  GetTimesheetOfUserInProjectNew(projectCode, user, startTime, endTime) {
    this.pmReportProjectService.GetTimesheetOfUserInProjectNew(projectCode, user.emailAddress, startTime, endTime).subscribe(rs => {
        user.normalWorkingTime = rs.result ? rs.result.normalWorkingTime : 0
        user.overTime = rs.result ? rs.result.overTime : 0
        user.overTimeNoCharge = rs.result ? rs.result.overTimeNoCharge : 0
        user.normalWorkingTimeAll = rs.result ? rs.result.normalWorkingTimeAll : 0
        user.normalWorkingTimeStandard = rs.result ? rs.result.normalWorkingTimeStandard : 0
        this.totalNormalWorkingTime += user.normalWorkingTime
        this.totalOverTime += user.overTime
        this.overTimeNoCharge += user.overTimeNoCharge
        this.totalNormalWorkingTimeOfWeekly += user.normalWorkingTimeAll
        this.totalNormalWorkingTimeStandard += user.normalWorkingTimeStandard
        this.otTime = `${this.overTimeNoCharge}h total OT NoCharge /  ${this.totalOverTime}h total OT`
        this.normalTime =  `${this.totalNormalWorkingTime}h of project /  ${this.totalNormalWorkingTimeOfWeekly}h all / ${this.totalNormalWorkingTimeStandard}h standard`
    })
}

GetTimesheetWeeklyChartOfUserInProject(projectCode, user, startTime, endTime) {
  this.pmReportProjectService.GetTimesheetWeeklyChartOfUserInProject(projectCode, user.emailAddress, startTime, endTime).subscribe(rs => {
    this.genarateUserChart(user, rs.result)
  })
}


public genarateUserChart(user:projectUserDto, chartData) {
  let hasOtValue = chartData.overTimeHours.some(item => item > 0)
  let hasOtNocharge = chartData.otNoChargeHours.some(item => item > 0)
  setTimeout(() => {
    let chartDom = document.getElementById('user' + user.userId);
    let myChart = echarts.init(chartDom);
    let option: echarts.EChartsOption;
    option = {
      tooltip: {
        trigger: 'axis'
      },
      grid: {
        top: "6%",
        left: '3%',
        right: '4%',
        bottom: '2%',
        containLabel: true
      },
      xAxis: {
        data: chartData.labels,
        show: false
      },
      yAxis: {
        type: 'value',
        min: 0,
        max: 60,
        show: false
      },
      series: [

        {
          symbolSize: 2,
          data: chartData.normalWoringHours,
          type: 'line',
          name: 'Normal',
        },
        {
          color: ['#dc3545'],
          symbolSize: 2,
          data: hasOtValue ? chartData.overTimeHours : [],
          type: 'line',
          name: 'OT',
          lineStyle: {color: '#dc3545'}
        },
        {
          color:['orange'],
          symbolSize: 2,
          data: hasOtNocharge ? chartData.otNoChargeHours : [],
          type: 'line',
          name: 'OT no charge',
          lineStyle: { color: 'orange' }
        }
      ]
    };
    option && myChart.setOption(option);
  }, 1)
}
}
