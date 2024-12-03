import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UploadCvBillAccountDto } from '@app/service/model/upload-cv.dto';

@Component({
  selector: 'app-upload-cv-bill-account',
  templateUrl: './upload-cv-bill-account.component.html',
  styleUrls: ['./upload-cv-bill-account.component.css']
})

export class UploadCvBillAccountComponent implements OnInit {
  public billAccountCv: UploadCvBillAccountDto;
  public selectedFile: File | null = null;

  constructor(@Inject(MAT_DIALOG_DATA) public data: UploadCvBillAccountDto,
    private dialogRef: MatDialogRef<UploadCvBillAccountComponent>) {
  }

  ngOnInit(): void {
    this.billAccountCv = this.data;
  }

  handleFileInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input?.files?.item(0) || null;
    if (this.selectedFile && !this.billAccountCv.nameCv) {
      this.billAccountCv.nameCv = this.selectedFile.name.replace(/\.[^/.]+$/, "");
    }
  }

  onSubmit() {
    this.dialogRef.close();
  }
}