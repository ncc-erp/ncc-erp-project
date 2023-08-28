import { Component, Inject, Injector, OnInit, Optional } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogComponentBase } from '../../dialog-component-base';
import * as moment from 'moment';

@Component({
  selector: 'app-custom-time',
  templateUrl: './custom-time.component.html',
  styleUrls: ['./custom-time.component.css'],
  providers: [{
    provide: DateAdapter,
    useClass: MomentDateAdapter,
    deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
  },
  ],
})
export class CustomTimeComponent extends DialogComponentBase<any> implements OnInit {

  public formPopup: FormGroup;
  public mode:number
  public timeOld
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _dialogRef: MatDialogRef<CustomTimeComponent>
  ) {
    super(injector);
  }

  ngOnInit() {
    this.title = "Custom time";
    this.mode = this.dialogData.mode
    this.timeOld ={
      textDate:this.dialogData.textDate,
      viewTimeOld:this.dialogData.viewTimeOld
    }
    this.setInitialValue();
  }

  public setInitialValue(){
    this.formPopup = this.fb.group({
      fromDateCustomTime: [new Date(), Validators.required],
      toDateCustomTime: [new Date(), Validators.required]
    })
  }



  public onSubmit() {
    if (!this.formPopup.valid) {
      return;
    }
    const date = {
      from: moment(this.formPopup.value.fromDateCustomTime),
      to: moment(this.formPopup.value.toDateCustomTime) ,
    }
    this.onClose(true, date);
  }

  public onClose(result: any, data?: any): void {
    this._dialogRef.close({ result, data });
  }
}
