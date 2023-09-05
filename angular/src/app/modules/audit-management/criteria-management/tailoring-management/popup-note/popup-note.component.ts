import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ProjectProcessCriteriaAppService } from '@app/service/api/project-process-criteria.service';
import { AppComponentBase } from '@shared/app-component-base';


import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-popup-note',
  templateUrl: './popup-note.component.html',
  styleUrls: ['./popup-note.component.css']
})
export class PopupNoteComponent extends AppComponentBase implements OnInit {
  isEdit: boolean
  title = '';
  constructor(injector: Injector, private processProjectService: ProjectProcessCriteriaAppService,
    public dialogRef: MatDialogRef<PopupNoteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    super(injector);

  }
  public preNote = "";
  public preApplicable = 1;
  public applicableList = [
    {displayName: "Standard", value: 1},
    {displayName: "Modify", value: 2},
  ];

  ngOnInit(): void {
    this.title = this.data.node.code + ' ' + this.data.node.name
    if (this.data.command == 'edit') {
      this.isEdit = true
    }
    this.preNote = this.data.node.note;
    this.preApplicable = this.data.node.applicable;
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  Cancel() {
    this.data.node.note = this.preNote;
    this.data.node.applicable = this.preApplicable;
    this.dialogRef.close()
  }
  SaveAndClose() {
    this.processProjectService.updatePPC(this.data.node.projectProcessCriteriaId, this.data.node.note, this.data.node.applicable)
      .pipe(catchError(this.processProjectService.handleError)).subscribe((res) => {
      abp.notify.success("Update note Successfully!");
      this.preNote = this.data.node.note;
      this.preApplicable = this.data.node.applicable;
      this.dialogRef.close()
    }), () => { this.isLoading = false }
  }
}
