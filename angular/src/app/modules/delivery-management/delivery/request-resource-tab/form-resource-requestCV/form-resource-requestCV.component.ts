import { ResourceRequestDto } from './../../../../../service/model/resource-request.dto';
import { resourceRequestCodeDto } from './../multiple-select-resource-request-code/multiple-select-resource-request-code.component';
import { APP_ENUMS } from '@shared/AppEnums';
import { AppComponent } from '../../../../../app.component';
import { ResourcePlanDto } from '../../../../../service/model/resource-plan.dto';
import { result } from 'lodash-es';
import { AppComponentBase } from '@shared/app-component-base';
import { ChangeDetectorRef, Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialog } from "@angular/material/dialog";
import * as moment from 'moment';
import { DeliveryResourceRequestService } from '@app/service/api/delivery-request-resource.service';
import { ResourceRequestCVDto } from '@app/service/model/resource-requestCV.dto';
import { catchError } from 'rxjs/operators';
import { ImportFileResourceComponent } from './../import-file-resource/import-file-resource.component';
import { RequestResourceDto } from '@app/service/model/delivery-management.dto';
import 'moment-timezone';
import * as momentTime from "moment-timezone";

import { UploadCVPathResourceRequestCV } from './../upload-cvPath-resource-requestCV/upload-cvPath-resource-requestCV.component';
import { CVStatusDto } from '@app/service/model/cvstatus.dto';
import { CvstatusService } from '@app/service/api/cvstatus.service';
@Component({
    selector : 'app-form-resource-requestCV',
    templateUrl:'./form-resource-requestCV.component.html',
    styleUrls: ['./form-resource-requestCV.component.css']
})
export class ResourceRequestCVComponent extends AppComponentBase implements OnInit{
   public listUsers: any[] = [];
   public billInfoPlan : any;
   public search: string = '';
   public inteviewDate : Date;
   public sendCVDate : Date;
   public status : any;
   public cvName : string;
   public cvPath : any;
   public linkCVPath : any;
   public note : string;
   public kpiPoint: number;
   public userId : number;
   public resourceRequestCV = {} as ResourceRequestCVDto;
   public resourceRequestId: number;
   public code: string;
   public cvStatusList: string[] = Object.keys(this.APP_ENUM.CVStatus)
   selectedFile: File;
   public requestResourceDto : RequestResourceDto;
   public resourceRequestDto : ResourceRequestDto;
   public cVStatusList: CVStatusDto[] = [];

   constructor(
    injector: Injector,
    @Inject(MAT_DIALOG_DATA) public input: any,
    public dialogRef: MatDialogRef<ResourceRequestCVComponent>,
    private ref: ChangeDetectorRef,  
    private dialog: MatDialog,
    private resourceRequestService: DeliveryResourceRequestService,
    private cvStatusService: CvstatusService
  ) 
{
  super(injector);
}
  ngOnInit():void{
    this.getAllCVStatus();
    this.resourceRequestCV = this.input;
    this.billInfoPlan = {startTime: this.input.billUserInfo ? this.input.billUserInfo.plannedDate : '',
    resourceRequestId: this.input.resourceRequestId,
    userId :  this.input.userId ,
  
    };
    if(this.input.command=='create'){
       this.resourceRequestCV = new ResourceRequestCVDto()     
         this.resourceRequestCV.resourceRequestId = this.input.resourceRequestId ;    
    }else{
       this.getResourceRequestCVById(this.input.item1.id);
    }
      this.resourceRequestId = this.input.resourceRequestId;
      this.resourceRequestCV.interviewDate = this.input.interviewDate || null;
      this.listUsers = this.input.listUsers;
      this.resourceRequestCV.sendCVDate = this.input.sendCVDate || null;
      this.resourceRequestCV.status= this.APP_ENUM.CVStatus['Chưa làm CV'];
      this.resourceRequestCV.cvPath = this.input.cvPath? this.input.cvPath: " ";
      this.resourceRequestCV.note = this.input.note? this.input.note: " ";
      this.resourceRequestCV.kpiPoint = this.input.kpiPoint;
      this.resourceRequestCV.cvName = this.input.cvName? this.input.cvName: " ";
      this.resourceRequestCV.linkCVPath = this.input.linkCVPath? this.input.linkCVPath: " ";
      this.requestResourceDto = this.input.item1.requestResource;
      this.code = this.input.code;
      this.resourceRequestCV.cvStatusId = this.input.cvStatusId? this.input.cvStatusId: 1;
  }

  getAllCVStatus() {
    this.cvStatusService.getAll().subscribe(res => {
      this.cVStatusList = res.result;
    });
  }

  ngAfterViewChecked(): void {
    this.ref.detectChanges()
  }
 
  getResourceRequestCVById(resourceRequestCVId : number){
     this.resourceRequestService.getResourceRequestCVById(resourceRequestCVId).subscribe(res=>{
        this.resourceRequestCV = res.result;
     })
  }
 
  public formatInterviewDateToVN(resourceRequestCV: ResourceRequestCVDto): any {
    return {
      ...resourceRequestCV,
      interviewDate: resourceRequestCV.interviewDate 
        ? momentTime(resourceRequestCV.interviewDate)
            .tz('Asia/Ho_Chi_Minh')
            .format('YYYY-MM-DD HH:mm:ss')
        : null,
      sendCVDate: resourceRequestCV.sendCVDate 
        ? momentTime(resourceRequestCV.sendCVDate)
            .tz('Asia/Ho_Chi_Minh')
            .format('YYYY-MM-DD HH:mm:ss')
        : null
    };
  }

 SaveAndClose(){
   this.isLoading = true;
   let resourceCv = new ResourceRequestCVDto();
   resourceCv.id = this.resourceRequestCV.id;
   resourceCv.userId = this.billInfoPlan.userId;
   resourceCv.cvName = this.resourceRequestCV.cvName;
   resourceCv.cvPath = this.resourceRequestCV.cvPath;
   resourceCv.status = this.resourceRequestCV.status;
   resourceCv.note = this.resourceRequestCV.note;
   resourceCv.kpiPoint = this.resourceRequestCV.kpiPoint;
   resourceCv.resourceRequestId =this.resourceRequestCV.resourceRequestId;
   resourceCv.sendCVDate = this.resourceRequestCV.sendCVDate;
   resourceCv.interviewDate = this.resourceRequestCV.interviewDate;
   resourceCv.cvStatusId = this.resourceRequestCV.cvStatusId;

   if(this.input.command=="create"){
       resourceCv.id =0
       let createResourceRequestCV = this.formatInterviewDateToVN(resourceCv);
       this.resourceRequestService.addCV(createResourceRequestCV).pipe(catchError(this.resourceRequestService.handleError)).subscribe((res)=>{
          abp.notify.success("Create SuccessFully");
       
          this.dialogRef.close(this.resourceRequestCV);
       },()=> this.isLoading = false)
   }else{
     let createResourceRequestCV = this.formatInterviewDateToVN(resourceCv);
    this.resourceRequestService.updateResourceRequestCV(createResourceRequestCV).pipe(catchError(this.resourceRequestService.handleError)).subscribe((res)=>{
      abp.notify.success("Update Successfully!");
      this.dialogRef.close(res.result);
    }, () => this.isLoading = false)
   }
 }
 checkNullOrEmpty(a : string):boolean{
  return a===null|| a===undefined|| a.trim().length===0;
}
  cancel(){
    this.dialogRef.close()
  }
  getStyleStatusUser(isActive: boolean){
    return isActive?"badge badge-pill badge-success":"badge badge-pill badge-danger"
  }

  getValueStatusUser(isActive: boolean){
    return isActive?"Active":"InActive"
  }


 
}
