import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-view-guiline',
  templateUrl: './view-guiline.component.html',
  styleUrls: ['./view-guiline.component.css']
})
export class ViewGuilineComponent  {

  constructor(
        public dialogRef: MatDialogRef<ViewGuilineComponent >,
        @Inject(MAT_DIALOG_DATA) public data: any) {
        }

      onNoClick(): void {
        this.dialogRef.close();
      }

}
