import { catchError } from 'rxjs/operators';
import { TechnologyService } from '../../../../service/api/technology.service';
import { TechnologyDto } from '@app/service/model/technology.dto'; 
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';

@Component({
  selector: 'app-create-update-technology',
  templateUrl: './create-update-technology.component.html',
  styleUrls: ['./create-update-technology.component.css']
})
export class CreateUpdateTechnologyComponent extends AppComponentBase implements OnInit {
  title:string =""
  public technology = {} as TechnologyDto;
  invoiceDateSettingList: [];
  paymentDueByList = [];
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public technologyService: TechnologyService,
    public dialogRef: MatDialogRef<CreateUpdateTechnologyComponent>,) { super(injector) }

  ngOnInit(): void {
    if(this.data.command == "update"){
      this.technology = this.data.item;
      this.title = this.data.item.name ? this.data.item.name : ''
    }
    else{
      this.technology.color = "#28a745";
    }
  }
  
  SaveAndClose() {
    if (this.data.command == "create") {
      this.technologyService.create(this.technology).pipe(catchError(this.technologyService.handleError)).subscribe((res) => {
        abp.notify.success("Create Technology Successfully!");
        this.dialogRef.close(this.technology);
      }, () => { this.isLoading = false })
    }
    else {
      this.technologyService.update(this.technology).pipe(catchError(this.technologyService.handleError)).subscribe((res) => {
        abp.notify.success("Update Technology Successfully!");
        this.dialogRef.close(this.technology);
      }, () => { this.isLoading = false })
    }

  }

}
