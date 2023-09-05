import { Component, OnInit, Injector, ViewChild, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';

import { MatMenuTrigger } from '@angular/material/menu';
import { TimesheetProjectService } from '@app/service/api/timesheet-project.service';
import { ActivatedRoute } from '@angular/router';

import { TimesheetService } from '@app/service/api/timesheet.service'
import { FileExportInvoiceDto} from './../../../../service/model/timesheet.dto';

import { UserService } from '@app/service/api/user.service';
import { ClientService } from '@app/service/api/client.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ProjectUserBillService } from '@app/service/api/project-user-bill.service';

import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MatDialog } from '@angular/material/dialog';
import { DialogComponentBase } from '@shared/dialog-component-base';

@Component({
  selector: 'app-export-invoice',
  templateUrl: './export-invoice.component.html',
  styleUrls: ['./export-invoice.component.css']
})
export class ExportInvoiceComponent extends DialogComponentBase<any> implements OnInit {

  @ViewChild(MatMenuTrigger)
  menu: MatMenuTrigger
  contextMenuPosition = { x: '0', y: '0' }

  constructor(injector: Injector,
    public sanitizer: DomSanitizer,
    private timesheetService: TimesheetService,
    public timesheetProjectService: TimesheetProjectService,
    private projectUserBillService: ProjectUserBillService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private ref: ChangeDetectorRef,
    private userService: UserService,
    private clientService: ClientService,
    private _modalService: BsModalService,
    ) {
      super(injector);
      this.html = this.dialogData.html;
      this.fileName = this.dialogData.fileName;
     }

  public html:string;
  public fileName:string;
  public invoicePdfData: FileExportInvoiceDto = {} as FileExportInvoiceDto;

  ngOnInit(): void {

  }

  public printInvoiceToPDF() {
    var content = this.html;
    var printWindow = window.open('', this.fileName, '_blank');
    printWindow.document.write(content);
    printWindow.document.close();

    // Create a style element with the print CSS
    var printCSS = '* {-webkit-print-color-adjust: exact !important; color-adjust: exact !important; font-family: "Times New Roman", Times, serif;}';
    var style = printWindow.document.createElement('style');
    style.type = 'text/css';
    style.media = 'print';
    style.appendChild(printWindow.document.createTextNode(printCSS));

    // Add the CSS to hide the header and footer
    var hideHeaderFooterCSS = '@media print { @page { size: auto; margin: 0; } @page :header { display: none; } @page :footer { display: none; } }';
    style.appendChild(printWindow.document.createTextNode(hideHeaderFooterCSS));

    printWindow.document.head.appendChild(style);

    // Set the title of the print window
    printWindow.addEventListener('beforeprint', (event) => {
      printWindow.document.title = this.fileName;
    });

    // Print the content and close the print window
    printWindow.print();
    printWindow.addEventListener('afterprint', (event) => {
      printWindow.clearTimeout;
      printWindow.close();
      abp.notify.success("Export Invoice to PDF Successfully!");
    });
  }

}
