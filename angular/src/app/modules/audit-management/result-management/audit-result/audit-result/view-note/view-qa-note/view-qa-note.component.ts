import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ProjectProcessResultAppService } from "@app/service/api/project-process-result.service";

@Component({
  selector: 'app-view-qa-note',
  templateUrl: './view-qa-note.component.html',
  styleUrls: ['./view-qa-note.component.css']
})
export class ViewQaNoteComponent extends AppComponentBase implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
  public injector: Injector,
  public dialogRef: MatDialogRef<ViewQaNoteComponent>,
  public projectProcessResultAppService: ProjectProcessResultAppService) {
  super(injector);
}

preNote = ""
ngOnInit(): void {
  if (this.data.action == 'edit') {
    this.preNote = this.data.item.note
  }
}
onNoClick(): void {
  this.dialogRef.close();
}
Cancel() {
  this.data.item.note = this.preNote;
  this.dialogRef.close()
}
SaveAndClose() {
  this.projectProcessResultAppService.updateNote(this.data.item.id, this.data.item.note).pipe(catchError(this.projectProcessResultAppService.handleError)).subscribe((res) => {
    abp.notify.success("Update note Successfully!");
    this.preNote = this.data.item.note;
    this.dialogRef.close()
  }), () => { this.isLoading = false }
}

}
