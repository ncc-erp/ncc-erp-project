import { Component, Inject, Injector, OnInit, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-update-confirm-modal',
  templateUrl: './update-confirm-modal.component.html',
  styleUrls: ['./update-confirm-modal.component.css']
})
export class UpdateConfirmModalComponent extends AppComponentBase implements OnInit {

  isChecked: boolean = false;
  projectName: string;

  constructor(
    public dialogRef: MatDialogRef<UpdateConfirmModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,

    public injector: Injector,
    ) {
      super(injector);
      this.projectName = data.projectName;
    }

  ngOnInit(): void {
    this.projectName = this.data.projectName
  }

  onConfirm() {
    this.dialogRef.close('confirm');
  }

  // onCancel() {
  //   this.dialogRef.close('cancel');
  // }

  onCancel() {
    if (this.data.markReview) {
      // If data.markReview is true, keep the checkbox checked and do not change the data
      this.isChecked = true;
    } else {
      // If data.markReview is false, uncheck the checkbox and do not change the data
      this.isChecked = false;
    }
    this.dialogRef.close('cancel');
  }


}
