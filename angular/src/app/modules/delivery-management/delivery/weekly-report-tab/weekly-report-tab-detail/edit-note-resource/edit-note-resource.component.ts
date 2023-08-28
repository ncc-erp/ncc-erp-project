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

  currentResource = {} as any
  oldNote = ""
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector: Injector,
    public dialogRef: MatDialogRef<EditNoteResourceComponent>, private projectUserService: ProjectUserService) {
    super(injector)
  }

  ngOnInit(): void {
    this.currentResource = this.data
    this.oldNote =this.data.note
  }
  saveAndClose() {
    this.projectUserService.EditCurentResourceNote(this.data.id, this.currentResource.note).pipe().subscribe(rs => {
      this.notify.success("Update note Successfully!");
      this.dialogRef.close(this.currentResource.note)
    })
  }
  Cancel() {
    this.dialogRef.close(this.oldNote)
  }
}
