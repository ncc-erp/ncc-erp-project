import { Component, EventEmitter, Inject, OnDestroy, OnInit, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { OffboardUserService } from '@app/service/api/offboard-user.service';

@Component({
  selector: 'app-add-offboard-note-dialog',
  templateUrl: './add-offboard-note-dialog.component.html',
  styleUrls: ['./add-offboard-note-dialog.component.css']
})
export class AddOffboardNoteDialogComponent implements OnInit, OnDestroy {
  saving = false;
  @Output() onSave = new EventEmitter<null>();
  subscriptions: Subscription[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AddOffboardNoteDialogComponent>,
    private offboardUserService: OffboardUserService
  ) { }

  ngOnInit(): void {
  }

  SaveAndClose(): void {
    const requestBody = {
      offboardUserId: this.data.offboardUserId,
      note: this.data.note,
    };

    this.saving = true;
    this.subscriptions.push(
      this.offboardUserService.UpdateOffboardHistoryNote(requestBody)
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
