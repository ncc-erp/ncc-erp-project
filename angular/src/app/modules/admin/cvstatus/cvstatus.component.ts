import { Component, Injector, OnInit } from '@angular/core';
import { CreateUpdateCvstatusComponent } from './create-update-cvstatus/create-update-cvstatus.component';
import { MatDialog } from '@angular/material/dialog';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { CvstatusService } from '../../../service/api/cvstatus.service';
import { catchError, finalize } from 'rxjs/operators';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { CVStatusDto, CvStatusCreateEditDto } from '@app/service/model/cvstatus.dto';
import { AppConsts } from '@shared/AppConsts';

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
  protected delete(cvstatus: CVStatusComponent): void {
    abp.message.confirm(
      "Delete CV Status " + cvstatus.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.cvStatusService.delete(cvstatus.id).pipe(catchError(this.cvStatusService.handleError)).subscribe(() => {
            abp.notify.success("Delete CV Status " + cvstatus.name);
            this.cVStatusList = this.cVStatusList.filter(cvStatus => cvStatus.id !== cvstatus.id);
          })
        }
      }
    )
  }

  Admin_CVStatus_View = PERMISSIONS_CONSTANT.Admin_CVStatus_View;
  Admin_CVStatus_Create = PERMISSIONS_CONSTANT.Admin_CVStatus_Create;
  Admin_CVStatus_Edit = PERMISSIONS_CONSTANT.Admin_CVStatus_Edit;
  Admin_CVStatus_Delete = PERMISSIONS_CONSTANT.Admin_CVStatus_Delete;

  public cVStatusList: CVStatusDto[] = [];
  
  constructor(private dialog: MatDialog,
    injector: Injector,
    private cvStatusService: CvstatusService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  public showDialog(cvStatusCreateEditDto: CvStatusCreateEditDto) {
    const show = this.dialog.open(CreateUpdateCvstatusComponent, {
      data: cvStatusCreateEditDto,
      width: "50%"
    });
    show.afterClosed().subscribe((res: CVStatusDto) => {
      if (res) {
        const existingIndex = this.cVStatusList.findIndex(cvStatus => cvStatus.id === res.id);
        if(existingIndex !== - 1) {
          this.cVStatusList[existingIndex] = { ...res };
        } else {
          this.cVStatusList.push({ ...res });
        }
      }
    });
  }

  public createCVStatus() {
    let cvStatusCreateEditDto = new CvStatusCreateEditDto();
    cvStatusCreateEditDto.command = AppConsts.CommandTypes.CREATE;
    cvStatusCreateEditDto.cvStatus = new CVStatusDto();
    this.showDialog(cvStatusCreateEditDto);
  }

  public editCVStatus(cVStatus: CVStatusDto) {
    let cvStatusCreateEditDto = new CvStatusCreateEditDto();
    cvStatusCreateEditDto.command = AppConsts.CommandTypes.UPDATE;
    cvStatusCreateEditDto.cvStatus = { ...cVStatus };
    this.showDialog(cvStatusCreateEditDto);
  }

}
