import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateCvstatusComponent } from './create-update-cvstatus/create-update-cvstatus.component';
import { MatDialog } from '@angular/material/dialog';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { CvstatusService } from '../../../service/api/cvstatus.service';
import { catchError, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-cvstatus',
  templateUrl: './cvstatus.component.html',
  styleUrls: ['./cvstatus.component.css']
})
export class CVStatusComponent extends PagedListingComponentBase<CVStatusComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.cvStatusService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.cvStatusService.handleError)).subscribe(data => {
      this.cVStatusList = data.result.items;
      this.showPaging(data.result, pageNumber)
    })
  }
  protected delete(entity: CVStatusComponent): void {
    throw new Error('Method not implemented.');
  }
  
  constructor(private dialog: MatDialog,
    injector: Injector,
    private cvStatusService: CvstatusService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  public showDialog(command: string, cvstatus: any) {
    let item = {
      name: cvstatus.name,
      color: cvstatus.color,
      id: cvstatus.id,
    };
    const show = this.dialog.open(CreateUpdateCvstatusComponent, {
      data: {
        item: item,
        command: command
      },
      width: "700px"
    });
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }

  public createCVStatus() {
    this.showDialog("create", {});
  }

}
