import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { CvStatusCreateEditDto, CVStatusDto } from '@app/service/model/cvstatus.dto';
import { CvstatusService } from '@app/service/api/cvstatus.service';
import { catchError } from 'rxjs/operators';
import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'app-create-update-cvstatus',
  templateUrl: './create-update-cvstatus.component.html',
  styleUrls: ['./create-update-cvstatus.component.css']
})
export class CreateUpdateCvstatusComponent extends AppComponentBase implements OnInit {

  public cvStatus: CVStatusDto = new CVStatusDto();
  public titleName: string = "";
  public triggerActionList = Object.keys(this.APP_ENUM.CvStatusTriggerAction);
  constructor(@Inject(MAT_DIALOG_DATA) public data: CvStatusCreateEditDto,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateUpdateCvstatusComponent>,
    public cvStatusService: CvstatusService
  ) { super(injector) }

  ngOnInit(): void {
    if(this.data.command == AppConsts.CommandTypes.UPDATE) {
      this.cvStatus = this.data?.cvStatus;
      this.titleName = this.cvStatus.name ?? "";
    } else {
      this.cvStatus.triggerAction = null;
      this.setRandomColor();
    }
  }

  setRandomColor(): void {
    const randomColor = Math.floor(Math.random() * 16777215).toString(16).padStart(6, '0');
    this.cvStatus.color = `#${randomColor}`;
  }

  SaveAndClose() {
    if (this.data.command == AppConsts.CommandTypes.CREATE) {
      this.cvStatusService.create(this.cvStatus).pipe(catchError(this.cvStatusService.handleError)).subscribe((res) => {
        abp.notify.success("Create CV Status Successfully!");
        this.dialogRef.close(res.result);
      })
    } else {
      this.cvStatusService.update(this.cvStatus).pipe(catchError(this.cvStatusService.handleError)).subscribe((res) => {
        abp.notify.success("Update CV Status Successfully!");
        this.dialogRef.close(res.result);
      })
    }
  }
}
