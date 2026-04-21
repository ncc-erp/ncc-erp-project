import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectUserOnboardingService }  from './../../../../../../service/api/project-user-onboarding.service';
import { OnboardingChecklistItemDto, AddOnboardingDto } from './../../../../../../service/model/onboarding-checklist-Item.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { finalize } from 'rxjs/operators';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';

@Component({
  selector: 'app-onboarding-dialog',
  templateUrl: './onboarding-dialog.component.html',
  styleUrls: ['./onboarding-dialog.component.css']
})
export class OnboardingDialogComponent extends AppComponentBase implements OnInit {
  
  checklist: OnboardingChecklistItemDto[] = [];
  projectUserId: number;
  fullName: string;
  isLoading: boolean = false;
  isSaving: boolean = false;
  Admin_OnboardingChecklist_ForceDone = PERMISSIONS_CONSTANT.Admin_OnboardingChecklist_ForceDone;
  
  constructor(
    injector: Injector,
    private _onboardingService: ProjectUserOnboardingService,
    public dialogRef: MatDialogRef<OnboardingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    super(injector);
    this.projectUserId = data.projectUserId;
    this.fullName = data.fullName;
  }

  ngOnInit(): void {
    this.getOnboardingData();
  }

  getOnboardingData() {
    this.isLoading = true;
    this._onboardingService.getOnboardingInfo(this.projectUserId)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe((res: any) => {
        if (res.result) {
            this.checklist = res.result.checklist;
        } else {
            this.checklist = res.checklist || []; 
        }
        
        console.log('Checklist data:', this.checklist);
      });
  }

  save() {
    this.isSaving = true;
    const input = new AddOnboardingDto();
    input.projectUserId = this.projectUserId;
    input.checklist = this.checklist;
    this._onboardingService.onboardingUser(input)
      .pipe(finalize(() => this.isSaving = false))
      .subscribe(() => {
        abp.notify.success('Onboarding user successfully');
        this.dialogRef.close(true); 
      });
  }

  close() {
    this.dialogRef.close(false);
  }

  get isAllChecked(): boolean {
    return this.checklist && this.checklist.every(x => x.isChecked);
  }

  forceDone() {
    abp.message.confirm(
      `Are you sure you want to force mark onboarding as DONE for ${this.fullName}?`,
      'Admin Confirmation',
      (result: boolean) => {
        if (result) {
          this.isSaving = true; 
          this._onboardingService.forceDone(this.projectUserId)
            .pipe(finalize(() => this.isSaving = false))
            .subscribe(() => {
              abp.notify.success('Force Done successfully');
              this.dialogRef.close(true); 
            });
        }
      }
    );
  }
}