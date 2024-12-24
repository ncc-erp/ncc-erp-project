import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepicker } from '@angular/material/datepicker';
import * as moment from 'moment';
import { Moment } from 'moment';

export const MONTH_YEAR_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
  },
};

@Component({
  selector: 'app-month-year-picker',
  templateUrl: './month-year-picker.component.html',
  styleUrls: ['./month-year-picker.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },
    { provide: MAT_DATE_FORMATS, useValue: MONTH_YEAR_FORMATS },
  ],
})
export class MonthYearPickerComponent implements OnInit {
  @Input() label: string;
  @Output() dateChange = new EventEmitter<Moment>();
  dateControl = new FormControl(moment().startOf('month')); // Initialize with the first day of the current month
  selectedMonthYear: Moment;

  constructor() { }

  ngOnInit(): void {
    this.selectedMonthYear = this.dateControl.value;
  }

  setMonthAndYear(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.dateControl.value ?? moment();
    ctrlValue.month(normalizedMonth.month());
    this.dateControl.setValue(ctrlValue);
    this.selectedMonthYear = ctrlValue;
    this.dateChange.emit(ctrlValue);
    datepicker.close();
  }

  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.dateControl.value ?? moment();
    ctrlValue.year(normalizedYear.year());
    this.dateControl.setValue(ctrlValue);
  }
}