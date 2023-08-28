import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PmReportService } from '@app/service/api/pm-report.service';
import { pmReportDto } from '@app/service/model/pmReport.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-add-report-note',
  templateUrl: './add-report-note.component.html',
  styleUrls: ['./add-report-note.component.css']
})
export class AddReportNoteComponent extends AppComponentBase implements OnInit {
  reportId:any
  pmReport= {} as pmReportDto
  constructor(@Inject(MAT_DIALOG_DATA) public data: any, injector: Injector,
    public dialogRef: MatDialogRef<AddReportNoteComponent>, private reportService: PmReportService) {
    super(injector)
  }

  ngOnInit(): void {
    this.reportId = this.data.reportId
    this.getReportById();
  }
  saveAndClose() {
    this.isLoading = true
    this.reportService.updateReportNote(this.reportId, this.pmReport.note).pipe(catchError(this.reportService.handleError))
      .subscribe(data => { abp.notify.success("Updated note"), this.isLoading = false, this.dialogRef.close() },
      () => { this.isLoading = false })
  }
  getReportById(){
    this.reportService.getById(this.reportId).pipe(catchError(this.reportService.handleError)).subscribe(data=>{
      this.pmReport =data.result
    })
  }
}
