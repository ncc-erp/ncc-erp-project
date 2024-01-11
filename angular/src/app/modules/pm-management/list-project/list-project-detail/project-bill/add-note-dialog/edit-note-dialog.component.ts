import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';
import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
  Inject
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-edit-note-dialog',
  templateUrl: './edit-note-dialog.component.html',
  styleUrls: ['./edit-note-dialog.component.css'],
})
export class EditNoteDialogComponent extends AppComponentBase implements OnInit, OnDestroy {
  fullName: string;
  projectName: string;
  id: number;
  note: string;
  saving = false;
  @Output() onSave = new EventEmitter<null>();

  subscription: Subscription[] = [];
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
      public injector: Injector,
      private projectUserBillService:ProjectUserBillService,
      public dialogRef: MatDialogRef<EditNoteDialogComponent>) {super(injector)}

  ngOnInit(): void {
   this.id =this.data.id;
   this.note = this.data.note;
  }

  SaveAndClose() {
    let requestBody = {
      id : this.id,
      note: this.note
    }
    this.saving = true;
    this.subscription.push(
      this.projectUserBillService
        .updateNote(requestBody)
        .pipe(
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe(() => {
          this.onSave.emit();
          abp.notify.success("Updated Note")
          this.dialogRef.close(this.note);
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
