import { ResourceManagerService } from "./../../../../../../service/api/resource-manager.service";
import { UserService } from "./../../../../../../service/api/user.service";
import {
  Component,
  EventEmitter,
  Inject,
  OnDestroy,
  OnInit,
  Output,
} from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Subscription } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
  selector: "app-add-note-dialog",
  templateUrl: "./add-note-dialog.component.html",
  styleUrls: ["./add-note-dialog.component.css"],
})
export class AddNoteDialogComponent implements OnInit, OnDestroy {

  saving = false;
  @Output() onSave = new EventEmitter<null>();

  subscription: Subscription[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<AddNoteDialogComponent>,
    public userService: UserService,
    private resourceService: ResourceManagerService
  ) {}

  ngOnInit(): void {
  }
  

  SaveAndClose() {
    let requestBody = {
      userId: this.data.userId,
      note: this.data.poolNote,
    };
    this.saving = true;
    this.subscription.push(
      this.resourceService
        .updatePoolNote(requestBody)
        .pipe(
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe(() => {
            this.dialogRef.close({ userId: this.data.userId, note: this.data.poolNote});
            this.onSave.emit();
            abp.notify.success("Update Note Successful");
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
