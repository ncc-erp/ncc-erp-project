import { catchError } from 'rxjs/operators';
import { BranchService } from '../../../../service/api/branch.service';
import { BranchDto } from '@app/service/model/branch.dto'; 
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';

@Component({
  selector: 'app-create-update-branch',
  templateUrl: './create-update-branch.component.html',
  styleUrls: ['./create-update-branch.component.css']
})
export class CreateUpdateBranchComponent extends AppComponentBase implements OnInit {
  title:string =""
  public branch = {} as BranchDto;
  invoiceDateSettingList: [];
  paymentDueByList = [];
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public branchService: BranchService,
    public dialogRef: MatDialogRef<CreateUpdateBranchComponent>,) { super(injector) }

  ngOnInit(): void {
    if(this.data.command == "update"){
      this.branch = this.data.item;
      this.title = this.data.item.name ? this.data.item.name : ''
    }
    else{
      this.branch.color = "#28a745";
    }
  }
  
  

  SaveAndClose() {
    if (this.data.command == "create") {
      this.branchService.create(this.branch).pipe(catchError(this.branchService.handleError)).subscribe((res) => {
        abp.notify.success("Create branch Successfully!");
        this.dialogRef.close(this.branch);
      }, () => { this.isLoading = false })
    }
    else {
      this.branchService.update(this.branch).pipe(catchError(this.branchService.handleError)).subscribe((res) => {
        abp.notify.success("Update branch Successfully!");
        this.dialogRef.close(this.branch);
      }, () => { this.isLoading = false })
    }

  }

}
