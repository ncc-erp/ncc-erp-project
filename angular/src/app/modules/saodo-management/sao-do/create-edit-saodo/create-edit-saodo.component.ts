import { AppComponentBase } from 'shared/app-component-base';
import { catchError } from 'rxjs/operators';
import { SaodoService } from './../../../../service/api/saodo.service';
import { SaodoDto } from './../../../../service/model/saodo.dto';
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit, Inject, Injector } from '@angular/core';
import * as moment from 'moment';
// import { FormControl } from '@angular/forms';
// import * as moment from 'moment';
// import { MatDatepicker } from '@angular/material/datepicker';
// import { Moment} from 'moment';
// import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
// import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
// export const MY_FORMATS = {
//   parse: {
//     dateInput: 'MM/YYYY',
//   },
//   display: {
//     dateInput: 'MM/YYYY',
//     monthYearLabel: 'MMM YYYY',
//     dateA11yLabel: 'LL',
//     monthYearA11yLabel: 'MMMM YYYY',
//   },
// };


@Component({
  selector: 'app-create-edit-saodo',
  templateUrl: './create-edit-saodo.component.html',
  styleUrls: ['./create-edit-saodo.component.css'],
  // providers: [
  //   {
  //     provide: DateAdapter,
  //     useClass: MomentDateAdapter,
  //     deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
  //   },

  //   {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  // ],
})
export class CreateEditSaodoComponent extends AppComponentBase implements OnInit {
  public saodo={} as SaodoDto;
  // public dateStart = new FormControl(moment());
  // public dateEnd = new FormControl(moment());
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector:Injector,
  public dialogRef: MatDialogRef<CreateEditSaodoComponent>,
  private router: Router,
  private saodoService : SaodoService) { 
    super(injector);
  }
 
  ngOnInit(): void {
    if (this.data.command == "edit") {
      this.saodo = this.data.item;
    }
  }
  SaveAndClose(){
    this.saodo.startTime = moment(this.saodo.startTime).format("YYYY-MM-DD");
    if(this.saodo.endTime){
      this.saodo.endTime = moment(this.saodo.endTime).format("YYYY-MM-DD");
    }
    this.isLoading = true
    if (this.data.command == "create") {
      // this.timesheet.isActive=true;
      this.saodoService.create(this.saodo).pipe(catchError(this.saodoService.handleError)).subscribe((res) => {
        abp.notify.success("Create Saodo successfully");
        this.dialogRef.close(this.saodo);
      }, () => this.isLoading = false);
      // 
    }
    else {
      this.saodoService.update(this.saodo).pipe(catchError(this.saodoService.handleError)).subscribe((res) => {
        abp.notify.success("Saodo has been edited successfully");
        this.dialogRef.close(this.saodo);
      }, () => this.isLoading = false);
    }
  }
  


  
  

}
