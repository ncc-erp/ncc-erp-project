import { filter } from 'rxjs/operators';
import { AppComponentBase } from 'shared/app-component-base';
import { PMReportProjectService } from './../../../../../../../service/api/pmreport-project.service';
import { pmReportDto } from './../../../../../../../service/model/pmReport.dto';
import { result } from 'lodash-es';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectResourceRequestService } from '@app/service/api/project-resource-request.service';
import { projectForDM } from '@app/service/model/list-project.dto';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent extends AppComponentBase implements OnInit {

  public projectId='';
  public clientName: string;
  public clientCode: string;
  public projectName: string;
  public projectDetail={} as projectForDM;
  public selectedReport ={} as pmReportDto;;
  public searchPmReport: string = "";
  public pmReportList: any = [];
  public pmId=0;
  public show:boolean=false;
  public panelOpenState:boolean=true;
  constructor(@Inject( MAT_DIALOG_DATA) public data:any,
    private resourceRequestService: ProjectResourceRequestService,
    public dialogRef: MatDialogRef<ProjectDetailComponent>,
    private reportService: PMReportProjectService,
    public injector:Injector,
    private route: ActivatedRoute,
  ) {super(injector)}

  ngOnInit(): void {
    this.projectId=this.data.projectId;
    this.clientName = this.data.clientName;
    this.clientCode = this.data.clientCode;
    this.projectName = this.route.snapshot.queryParamMap.get("projectName")

    console.log(this.data)

    // this.getProjectDetail();
    this. getAllPmReport();
  }
  public getProjectDetail(){
    this.resourceRequestService.getProjectForDM(this.projectId,this.selectedReport.reportId).subscribe((data)=>{
      this.projectDetail=data.result;
    })
  }
  public onReportchange(pmReport) {
    this.selectedReport=pmReport;


   this.getProjectDetail();
   this.show=true;


  }
  public getAllPmReport() {
    this.reportService.GetAllByProject(Number(this.projectId)).subscribe(data => {
      this.pmReportList = data.result;
      this.selectedReport = this.pmReportList.filter(item => item.isActive == true)[0];
      // this.pmId=this.selectedReport.reportId;
      console.log("id",this.selectedReport )
      this.getProjectDetail();

    })
  }


}
