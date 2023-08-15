import { ProjectUserService } from '@app/service/api/project-user.service';
import { EditFutureResourceComponent } from './edit-future-resource/edit-future-resource.component';
import { PlanUserComponent } from './../plan-resource/plan-user/plan-user.component';
import { MatDialog } from '@angular/material/dialog';
import { InputFilterDto } from './../../../../../../shared/filter/filter.component';
import { result } from 'lodash-es';
import { futureResourceDto } from './../../../../../service/model/delivery-management.dto';
import { catchError, finalize, filter } from 'rxjs/operators';
import { DeliveryResourceRequestService } from './../../../../../service/api/delivery-request-resource.service';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-future-resource',
  templateUrl: './future-resource.component.html',
  styleUrls: ['./future-resource.component.css']
})
export class FutureResourceComponent extends PagedListingComponentBase<FutureResourceComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.isLoading= true;
    this.availableRerourceService.availableResourceFuture(request).pipe(finalize(()=>{
      finishedCallback();
    }),catchError(this.availableRerourceService.handleError)).subscribe((data)=>{
      this.futureResourceList=data.result.items;
      this.showPaging(data.result,pageNumber);
      this.isLoading = false;
    })
  }
  protected delete(item: FutureResourceComponent): void {
    abp.message.confirm(
      "Delete future resource: " + item.userName + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.projectUserService.removeProjectUser(item.id).pipe(catchError(this.projectUserService.handleError)).subscribe(() => {
            abp.notify.success("Deleted TimeSheet " + item.userName);
            this.refresh();
          });
        }
      }
    );
  }
  
  public futureResourceList:futureResourceDto[]=[];
  constructor(public injector:Injector,
    private availableRerourceService: DeliveryResourceRequestService,
    private dialog:MatDialog,
    private projectUserService: ProjectUserService) {super(injector)}
    // public readonly FILTER_CONFIG: InputFilterDto[] = [
    //   { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Tên dự án", },
    //   { propertyName: 'clientName', comparisions: [0, 6, 7, 8], displayName: "Tên khách hàng", },
    //   { propertyName: 'pmName', comparisions: [0, 6, 7, 8], displayName: "Tên PM", },
    //   // { propertyName: 'status', comparisions: [0], displayName: "Trạng thái", filterType: 3, dropdownData: this.statusFilterList },
    //   { propertyName: 'isCharge', comparisions: [0], displayName: "Charge khách hàng", filterType: 2 },
    //   { propertyName: 'isSent', comparisions: [0], displayName: "Đã gửi weekly", filterType: 2 },
    //   { propertyName: 'startTime', comparisions: [0, 1, 2, 3, 4], displayName: "Thời gian bắt đầu", filterType: 1 },
    //   { propertyName: 'endTime', comparisions: [0, 1, 2, 3, 4], displayName: "Thời gian kết thúc", filterType: 1 },
    //   { propertyName: 'dateSendReport', comparisions: [0, 1, 2, 3, 4], displayName: "Thời gian gửi report", filterType: 1 },
    //   { propertyName: 'projectType', comparisions: [0], displayName: "Loại dự án", filterType: 3, dropdownData: this.projectTypeParam },
     
    // ];
    userTypeParam = Object.entries(this.APP_ENUM.UserType).map((item)=>{
      return{
        displayName: item[0],
        value: item[1]
      }
      
    })
    branchParam = Object.entries(this.APP_ENUM.UserBranch).map((item)=>{
      return {
        displayName: item[0],
        value: item[1]
      }
    })

    public readonly FILTER_CONFIG: InputFilterDto[] = [
      { propertyName: 'fullName', comparisions: [0, 6, 7, 8], displayName: "User Name" },
      { propertyName: 'projectName', comparisions:  [0, 6, 7, 8], displayName: "Project Name" },
      { propertyName: 'use', comparisions:  [0, 1, 2, 3, 4], displayName: "Used" },
      { propertyName: 'startDate', comparisions:  [0, 1, 2, 3, 4], displayName: "Start date", filterType: 1 },
      { propertyName: 'userType' , comparisions: [0], displayName: "User Type", filterType: 3, dropdownData: this.userTypeParam},
      { propertyName: 'branch' , comparisions: [0], displayName: "Branch", filterType: 3, dropdownData: this.branchParam}
      
    ];
    


  ngOnInit(): void {
    this.refresh();
  }
  showDialog(command:string,User:futureResourceDto){
    let item={
      fullName:User.fullName,
      status:2,
      userId:User.userId,
      allocatePercentage:User.use,
      startTime:User.startDate,
      projectId:User.projectid,
      id:User.id,
     
    }
    const show=this.dialog.open(EditFutureResourceComponent, {
      width: '700px',
      disableClose: true,
      data: {
        futureResource:item,
        command:command
      },
    });
    show.afterClosed().subscribe(result => {
      if(result){
        this.refresh()
      }
    });
    
  }

  updateFutureResource(user){
    this.showDialog("update",user);
  }


  


}
