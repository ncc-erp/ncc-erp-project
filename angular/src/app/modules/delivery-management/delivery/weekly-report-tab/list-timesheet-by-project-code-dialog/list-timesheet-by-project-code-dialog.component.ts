import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-list-timesheet-by-project-code-dialog',
  templateUrl: './list-timesheet-by-project-code-dialog.component.html',
  styleUrls: ['./list-timesheet-by-project-code-dialog.component.css']
})
export class ListTimesheetByProjectCodeDialogComponent implements OnInit {
  listProjectWithTimesheet: ProjectTimesheetInfoDto[] = []
  searchText: string = ""
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<ListTimesheetByProjectCodeDialogComponent>) { }

  ngOnInit(): void {
    this.listProjectWithTimesheet = this.data
  }

  searchProject() {
    this.listProjectWithTimesheet = this.data.filter(project => project.projectName.toLowerCase().includes(this.searchText.toLowerCase())
      || project.projectCode.toLowerCase().includes(this.searchText.toLowerCase() 
      || project.pmName.toLowerCase().includes(this.searchText.toLowerCase())))
  }

}
export class ProjectTimesheetInfoDto {
  normalWorkingTime: number
  overTime: number
  projectName: string
  projectCode: string
}