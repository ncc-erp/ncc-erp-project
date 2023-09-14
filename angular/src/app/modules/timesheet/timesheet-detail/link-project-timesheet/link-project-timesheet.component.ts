import { Component, OnInit,Injector, ViewChild, ElementRef,Inject } from '@angular/core';
import { MatDialogRef ,MAT_DIALOG_DATA} from '@angular/material/dialog';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { AppComponentBase } from '@shared/app-component-base';
import { DialogComponentBase } from '@shared/dialog-component-base';

@Component({
  selector: 'app-link-project-timesheet',
  templateUrl: './link-project-timesheet.component.html',
  styleUrls: ['./link-project-timesheet.component.css']
})
export class LinkProjectTimesheetComponent extends AppComponentBase implements OnInit  {
  public saving: boolean = false;
  public title!: string
  public listProjectCode: string
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<LinkProjectTimesheetComponent>,
     injector: Injector,
     private timesheetProjectService: TimesheetProjectService,
  ) { 
    super(injector)
   }

  ngOnInit(): void {
    this.title = `Link ${this.data.projectName} To Project In Timesheet Tool`
    this.listProjectCode = this.data.listProjectCodes
  }


  handleSave() {
    this.saving = true
    this.timesheetProjectService.updateProjectCode(this.data.projectId,this.listProjectCode).subscribe(() => {
        abp.notify.success('Updated ProjectCode successfully')
        this.saving = false
        this.dialogRef.close(true)
    })
  }
}
