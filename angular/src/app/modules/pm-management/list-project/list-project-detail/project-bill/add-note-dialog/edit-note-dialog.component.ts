import { ProjectUserBillService } from '@app/service/api/project-user-bill.service'; 
import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-edit-note-dialog',
  templateUrl: './edit-note-dialog.component.html',
  styleUrls: ['./edit-note-dialog.component.css'],
})
export class EditNoteDialogComponent implements OnInit, OnDestroy {
  fullName: string;
  projectName: string;
  id: number;
  note: string;
  saving = false;
  @Output() onSave = new EventEmitter<null>();

  subscription: Subscription[] = [];
  constructor(public bsModalRef: BsModalRef, 
     private projectUserBillService:ProjectUserBillService) {}

  ngOnInit(): void {
   
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
          this.bsModalRef.hide();
          this.onSave.emit();
          abp.notify.success("Updated Note")
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
