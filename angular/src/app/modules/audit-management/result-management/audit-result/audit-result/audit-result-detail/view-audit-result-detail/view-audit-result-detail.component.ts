import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-view-audit-result-detail',
  templateUrl: './view-audit-result-detail.component.html',
  styleUrls: ['./view-audit-result-detail.component.css']
})
export class ViewAuditResultDetailComponent {

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<ViewAuditResultDetailComponent>
  ) { }

  availableColors = [
    { status: 1, color: null, style: 'background-color: #EA9999', label: 'Non-Compliance' },
    { status: 2, color: null, style: 'background-color: #FFF2CC', label: 'Observation' },
    { status: 3, color: null, style: 'background-color: #CFE2F3', label: 'Recommendation' },
    { status: 4, color: null, style: 'background-color: #B6D7A8', label: 'Excellent' }
  ];

  applicableProjectProcessCriteria = [
    {applicable: 0, color: null, style: 'background-color: #EA9999', label: 'Not Yet'},
    {applicable: 1, color: null, style: 'background-color: #CFE2F3', label: 'Standard'},
    {applicable: 2, color: null, style: 'background-color: #B6D7A8', label: 'Modify'}
  ]

  isContentTypeViewFromName(): boolean {
    return this.data.contentType === 'viewFromName';
  }

  ngOnInit(): void {
    //console.log(this.data.node)
  }

}
