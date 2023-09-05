import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-main-sub-invoice',
  templateUrl: './main-sub-invoice.component.html',
  styleUrls: ['./main-sub-invoice.component.css']
})
export class MainSubInvoiceComponent extends AppComponentBase implements OnInit {
  public invoiceSetting: EInvoiceSetting
  public mainProjectId: number;
  public subProjects: number[] = []
  public invoiceSettingOptions = Object.entries(EInvoiceSetting).map((item) => ({
    key: item[0],
    value: item[1]
  }))

  constructor(injector: Injector) { 
    super(injector)
  }

  ngOnInit(): void {
  }

}


export interface MainSubInvoice {

}

export enum EInvoiceSetting {
  Main = 1,
  Sub = 2
}