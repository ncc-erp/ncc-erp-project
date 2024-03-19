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
  poolNote: string = "";
  userId: number;

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
    if (this.data.id) {
      this.subscription.push(
        this.userService.getOne(this.data.id).subscribe((response) => {
          this.userId = response.result.id;
          this.poolNote = response.result.poolNote;
        })
      );
    }
  }

  SaveAndClose() {
    let requestBody = {
      userId: this.userId,
      note: this.poolNote,
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
        .subscribe((response) => {
          this.poolNote = response.note;
          this.dialogRef.close();
          this.onSave.emit();
          abp.notify.success("Update Note");
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
