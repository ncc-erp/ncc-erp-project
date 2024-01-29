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
import { PlanningBillInfoService } from "@app/service/api/bill-account-plan.service";
import { Subscription } from "rxjs";
import { finalize } from "rxjs/operators";

@Component({
  selector: 'app-bill-account-dialog-note',
  templateUrl: './bill-account-dialog-note.component.html',
  styleUrls: ['./bill-account-dialog-note.component.css']
})

export class BillAccountDialogNoteComponent implements OnInit {
  billNote: string = "";
  userId: number;   
  projectId: number;          
  fullName: string;
  projectName: string;
  saving = false;
  @Output() onSave = new EventEmitter<null>();
  
  subscription: Subscription[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<BillAccountDialogNoteComponent>,
    private planningBillInfoService: PlanningBillInfoService,
  ) {}

  ngOnInit(): void {
    this.billNote = this.data.note;
    this.userId = this.data.userInfor.userId;
    this.projectId = this.data.projectId;
    this.fullName = this.data.userInfor.fullName;
    this.projectName = this.data.projectName;
  }

  SaveAndClose() {
    let requestBody = {
      userId: this.userId,
      projectId: this.projectId,
      note:this.billNote
    };
    this.saving = true;
    this.subscription.push(
      this.planningBillInfoService
        .UpdateBillNote(requestBody)
        .pipe(
          finalize(() => {
            this.saving = false;
          })
        )
        .subscribe(() => {
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
