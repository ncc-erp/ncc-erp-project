import { AppComponentBase } from 'shared/app-component-base';
import { AppComponent } from './../../../../../../app.component';
import { ProjectUserService } from './../../../../../../service/api/project-user.service';
import { catchError } from 'rxjs/operators';
import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DeliveryResourceRequestService } from './../../../../../../service/api/delivery-request-resource.service';
import { Component, Inject, OnInit, Injector } from '@angular/core';
import { userToRequestDto } from '@app/service/model/delivery-management.dto';
import * as moment from 'moment';
import { sortedUniq } from 'lodash-es';

@Component({
  selector: 'app-add-user-to-request',
  templateUrl: './add-user-to-request.component.html',
  styleUrls: ['./add-user-to-request.component.css']
})
export class AddUserToRequestComponent extends AppComponentBase implements OnInit {

  public userToRequest = {} as userToRequestDto;
  public editToRequest = {} as userToRequestDto;
  public minDate = new Date();
  public timeNeed;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector: Injector
    , private resourceRequestService: DeliveryResourceRequestService,
    private route: ActivatedRoute,
    public dialogRef: MatDialogRef<AddUserToRequestComponent>,
    private projectUserService: ProjectUserService) {
    super(injector);
  }

  ngOnInit(): void {
    this.timeNeed= this.route.snapshot.queryParamMap.get('timeNeed');
    
    
    console.log(this.route.snapshot.queryParamMap.get('timeNeed'))
    this.minDate= new Date(moment(this.timeNeed).format("YYYY/MM/DD"));
    console.log(this.minDate)
    this.userToRequest = this.data.item;
    this.userToRequest.userId = this.data.userId;
    this.userToRequest.resourceRequestId = Number(this.route.snapshot.queryParamMap.get('id'));
    
      
  

  }
  SaveAndClose() {
    this.isLoading = true;
    this.userToRequest.startTime = moment(this.userToRequest.startTime).format("YYYY/MM/DD");
    if (this.data.command == "create") {
      this.resourceRequestService.AddUserToRequest(this.userToRequest).pipe(catchError(this.resourceRequestService.handleError)).subscribe((res) => {
        abp.notify.success("Add successfully");
        this.dialogRef.close(this.userToRequest)
      }, () => this.isLoading = false);
    } else {
      this.projectUserService.update(this.userToRequest).pipe(catchError(this.resourceRequestService.handleError)).subscribe((res) => {
        abp.notify.success("Update successfully");
        this.dialogRef.close(this.userToRequest)
      }, () => this.isLoading = false);
    }
  }
  getPercentage(report, data) {
    report.allocatePercentage = data
  }


}
