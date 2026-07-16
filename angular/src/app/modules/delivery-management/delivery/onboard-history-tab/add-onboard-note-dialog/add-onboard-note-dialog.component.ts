import { Component, EventEmitter, Inject, OnDestroy, OnInit, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ProjectUserOnboardingService } from '@app/service/api/project-user-onboarding.service';

@Component({
  selector: 'app-add-onboard-note-dialog',
  templateUrl: './add-onboard-note-dialog.component.html',
  styleUrls: ['./add-onboard-note-dialog.component.css']
})
export class AddOnboardNoteDialogComponent implements OnInit, OnDestroy {
  saving = false;
  @Output() onSave = new EventEmitter<null>();
  subscriptions: Subscription[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AddOnboardNoteDialogComponent>,
    private projectUserOnboardingService: ProjectUserOnboardingService
  ) { }

  ngOnInit(): void {
  }

  SaveAndClose(): void {
    const requestBody = {
      projectUserOnboardingId: this.data.projectUserOnboardingId,
      note: this.data.note,
    };

    this.saving = true;
    this.subscriptions.push(
      this.projectUserOnboardingService.updateOnboardHistoryNote(requestBody)
        .pipe(
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe(() => {
          this.dialogRef.close({ success: true, note: this.data.note });
          this.onSave.emit();
          abp.notify.success('Update Note Successful');
        })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }
}
