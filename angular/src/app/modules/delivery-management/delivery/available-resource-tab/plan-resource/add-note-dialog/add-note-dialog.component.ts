import { ResourceManagerService } from './../../../../../../service/api/resource-manager.service';
import { DeliveryResourceRequestService } from './../../../../../../service/api/delivery-request-resource.service';
import { UserService } from './../../../../../../service/api/user.service';
import { IUser } from './../../../../../../service/model/user.inteface';

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
  selector: 'app-add-note-dialog',
  templateUrl: './add-note-dialog.component.html',
  styleUrls: ['./add-note-dialog.component.css'],
})
export class AddNoteDialogComponent implements OnInit, OnDestroy {
  fullName: string;
  id: number;
  poolNote: string = '';
  userId: number;

  saving = false;
  @Output() onSave = new EventEmitter<null>();

  subscription: Subscription[] = [];
  constructor(public bsModalRef: BsModalRef, public userService: UserService, private resourceService:ResourceManagerService) {}

  ngOnInit(): void {
    if (this.id) {
      this.subscription.push(
        this.userService.getOne(this.id).subscribe((response) => {
          this.userId = response.result.id;
          this.poolNote = response.result.poolNote;
        })
      );
    }
  }

  SaveAndClose() {
    let requestBody = {
      userId : this.userId,
      note: this.poolNote
    }
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
          this.bsModalRef.hide();
          this.onSave.emit();
          abp.notify.success("Update Note")
        })
    );
  }

  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
}
