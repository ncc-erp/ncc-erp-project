import { Component, Inject, Injector, OnInit, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppConfigurationService } from '@app/service/api/app-configuration.service';

@Component({
  selector: 'app-report-guideline-detail',
  templateUrl: './report-guideline-detail.component.html',
  styleUrls: ['./report-guideline-detail.component.css']
})
export class ReportGuidelineDetailComponent extends AppComponentBase implements OnInit {

  WeeklyReport_ReportDetail_GuideLine_Update = PERMISSIONS_CONSTANT.WeeklyReport_ReportDetail_GuideLine_Update;

  isEditMode = false;
  public trustedHtml: SafeHtml;
  public guideline: any;
  public name: string;
  public guideLineItem: any;
  public previousGuideline;

  WeeklyReport_ReportDetail_GuideLine_View = PERMISSIONS_CONSTANT.WeeklyReport_ReportDetail_GuideLine_View;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<ReportGuidelineDetailComponent>,
    private sanitizer: DomSanitizer,
    private settingService: AppConfigurationService,
  ) {
    super(injector);
    this.trustedHtml = this.sanitizer.bypassSecurityTrustHtml(this.data.guideline);
  }

  @Input() item: any;

  ngOnInit(): void {
    this.guideline = this.data.guidelineContent || "";
    this.name = this.data.name;
    this.guideLineItem = this.data.item;

  }

  startEdit() {
    this.isEditMode = true;
    this.previousGuideline = this.data.guidelineContent;
  }

  cancelEdit() {
    this.isEditMode = false;
    this.data.guidelineContent = this.previousGuideline;
  }

  SaveAndClose() {
    const updatedGuideline = { ...this.guideLineItem };

    if (updatedGuideline) {
      switch (this.name) {
        case "Criteria Status":
          updatedGuideline.criteriaStatus = this.data.guidelineContent ;
          break;
        case "Issue":
          updatedGuideline.issue = this.data.guidelineContent ;
          break;
        case "Risk":
          updatedGuideline.risk = this.data.guidelineContent ;
          break;
        case "PM Note":
          updatedGuideline.pmNote = this.data.guidelineContent ;
          break;
      }
    }

    this.settingService.editGuideLine(updatedGuideline)
      .pipe(
        catchError(this.settingService.handleError)
      )
      .subscribe(
        (res) => {
          if (res.success) {
            abp.notify.success("Update Successfully!");
            this.dialogRef.close(updatedGuideline);
          }
        },
        () => { this.isLoading = false }
      );
  }

}
