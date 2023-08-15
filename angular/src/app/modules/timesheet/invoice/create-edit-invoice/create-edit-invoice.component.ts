import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';

@Component({
  selector: 'app-create-edit-invoice',
  templateUrl: './create-edit-invoice.component.html',
  styleUrls: ['./create-edit-invoice.component.css']
})
export class CreateEditInvoiceComponent implements OnInit {
  public isDisable:boolean=false;
  public invoice;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,) { }

  ngOnInit(): void {
    this.invoice= this.data.item;

  }
  SaveAndClose(){

  }

}
