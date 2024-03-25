import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PmReportIssueService } from '@app/service/api/pm-report-issue.service';
import { AppComponentBase } from '@shared/app-component-base';
import {ProjectUserService} from '@app/service/api/project-user.service'

@Component({
  selector: 'app-edit-note-resource',
  templateUrl: './edit-note-resource.component.html',
  styleUrls: ['./edit-note-resource.component.css']
})
export class EditNoteResourceComponent extends AppComponentBase implements OnInit {
  id: number;
  note: string;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector: Injector,
    public dialogRef: MatDialogRef<EditNoteResourceComponent>, private projectUserService: ProjectUserService) {
    super(injector)
  }

  ngOnInit(): void {
    this.id =this.data.id;
    this.note = this.data.note;
  }

  saveAndClose() {
    let requestBody = {
      id: this.id,
      note: this.note
    }
    this.projectUserService.UpdateNoteCurrentResource(requestBody).pipe().subscribe(rs => {
      this.notify.success("Update note Successfully!");
      this.dialogRef.close(this.note)
    })
  }

  Cancel() {
    this.dialogRef.close(this.note)
  }
}
