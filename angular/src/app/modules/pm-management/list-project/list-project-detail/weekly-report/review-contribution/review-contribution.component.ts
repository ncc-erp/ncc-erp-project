import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-review-contribution',
  templateUrl: './review-contribution.component.html',
  styleUrls: ['./review-contribution.component.css']
})
export class ReviewContributionComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ReviewContributionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any 
  ) { } 

  ngOnInit(): void {
    console.log('Data received:', this.data);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}