import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import { GetCvBillAccountDto, UploadCvBillAccountDto } from '@app/service/model/upload-cv.dto';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-upload-cv-bill-account',
  templateUrl: './upload-cv-bill-account.component.html',
  styleUrls: ['./upload-cv-bill-account.component.css']
})

export class UploadCvBillAccountComponent implements OnInit {
  public billAccountCv: GetCvBillAccountDto;
  public selectedFile: File | null = null;

  constructor(@Inject(MAT_DIALOG_DATA) public data: GetCvBillAccountDto,
    private dialogRef: MatDialogRef<UploadCvBillAccountComponent>,
    private projectUserBillService: ProjectUserBillService) {
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

  onSubmit(): void {
    const request: UploadCvBillAccountDto = {
      id: this.billAccountCv.id,
      nameCv: this.billAccountCv.nameCv,
      selectedFile: this.selectedFile
    };
    this.projectUserBillService.UploadCvBillAccount(request)
      .pipe(catchError(error => this.projectUserBillService.handleError(error)))
      .subscribe({
        next: res => {
          abp.notify.success("Upload CV successfully");
          this.billAccountCv = res.result;
          this.dialogRef.close(this.billAccountCv);
        },
        error: () => { this.dialogRef.close() }
      });
  }
}