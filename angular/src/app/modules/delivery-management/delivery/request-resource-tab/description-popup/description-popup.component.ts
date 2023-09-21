import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-description-popup',
  templateUrl: './description-popup.component.html',
  styleUrls: ['./description-popup.component.css']
})
export class DescriptionPopupComponent extends AppComponentBase implements OnInit {

  public note:string
  constructor(  injector: Injector,
    public dialogRef: MatDialogRef<DescriptionPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { 
      super(injector);
    }

  ngOnInit(): void {
    this.note = this.data
  }
}
