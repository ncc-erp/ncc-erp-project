import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-create-update-cvstatus',
  templateUrl: './create-update-cvstatus.component.html',
  styleUrls: ['./create-update-cvstatus.component.css']
})
export class CreateUpdateCvstatusComponent extends AppComponentBase implements OnInit {

  title:string =""
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateUpdateCvstatusComponent>,) { super(injector) }

  ngOnInit(): void {
  }

}
