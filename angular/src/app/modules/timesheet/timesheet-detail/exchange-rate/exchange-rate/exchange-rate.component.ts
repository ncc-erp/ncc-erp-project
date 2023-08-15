import { catchError } from 'rxjs/operators';
import { result } from 'lodash-es';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject, EventEmitter, Output } from '@angular/core';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import * as moment from 'moment';
import * as FileSaver from 'file-saver';
import { TimesheetInfoDto } from '@app/service/model/timesheet.dto';
@Component({
  selector: 'app-exchange-rate',
  templateUrl: './exchange-rate.component.html',
  styleUrls: ['./exchange-rate.component.css']
})
export class ExchangeRateComponent extends AppComponentBase implements OnInit {
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<ExchangeRateComponent>,
    public timeSheetProjectService: TimesheetProjectService
  ) { super(injector) }
  public listCurrencies: any;
  public startDate = new Date();
  public submitDate = '';
  public symbols = '';
  ngOnInit(): void {

    this.listCurrencies = [...this.data.currencyInfor];
    this.submitDate = moment(this.startDate.toISOString()).format('YYYY-MM-DD');
    this.listCurrencies.forEach(element => {
      this.symbols += element.currencyName + ',';
    });
    this.getExchangeRate();

  }
  public getExchangeRate() {
    this.timeSheetProjectService.getExchangeRate(this.submitDate, "VND", this.symbols, 50).subscribe((res) => {
      if (res.result.success) {
        this.listCurrencies.forEach(element => {
          element.exchangeRate = res.result.rates[element.currencyName.trim()] == undefined ? 0 : 1 / res.result.rates[element.currencyName];
        });
      }
      else {
      }
    });
  }
  public onDateChange(): void {
    this.submitDate = moment(this.startDate.toISOString()).format('YYYY-MM-DD');
    this.getExchangeRate();
  }
  public ExportAllInvoice() {
    let input = new TimesheetInfoDto();
    input.timesheetId = this.data.timesheetId;
    input.timesheetName = this.data.timesheetName;
    input.date = moment(this.startDate.toISOString()).format('DD/MM/YYYY');
    input.currencies = this.listCurrencies;
    this.timeSheetProjectService.exportAllTimeSheetProjectToExcel(input).subscribe((res) => {
      const file = new Blob([this.s2ab(atob(res.result.base64))], {
        type: "application/vnd.ms-excel;charset=utf-8"
      });
      FileSaver.saveAs(file, res.result.fileName);
      abp.notify.success("Export Invoice Successfully!");
      this.dialogRef.close();
    })
  }
  s2ab(s) {
    var buf = new ArrayBuffer(s.length);
    var view = new Uint8Array(buf);
    for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
    return buf;
  }
  onInputCurrency(event : any, item:any) {
    item.exchangeRate = event.target.value;
  }
}
