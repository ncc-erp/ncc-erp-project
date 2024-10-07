import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { CVStatusDto } from '@app/service/model/cvstatus.dto';
import { CvstatusService } from '@app/service/api/cvstatus.service';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-create-update-cvstatus',
  templateUrl: './create-update-cvstatus.component.html',
  styleUrls: ['./create-update-cvstatus.component.css']
})
export class CreateUpdateCvstatusComponent extends AppComponentBase implements OnInit {

  title:string =""
  public cvstatus = {} as CVStatusDto
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateUpdateCvstatusComponent>,
    public cvStatusService: CvstatusService
  ) { super(injector) }

  ngOnInit(): void {
    if(this.data.command == 'update') {
      this.cvstatus = this.data.item;
      this.title = this.data.item.name ? this.data.item.name : ''
    } else {
      this.setRandomColor();
    }
  }

  setRandomColor(): void {
    const randomColor = Math.floor(Math.random() * 16777215).toString(16);
    this.cvstatus.color = `#${randomColor}`;
  }

  SaveAndClose() {
    if (this.data.command == "create") {
      this.cvStatusService.create(this.cvstatus).pipe(catchError(this.cvStatusService.handleError)).subscribe((res) => {
        abp.notify.success("Create CV Status Successfully!");
        this.dialogRef.close(this.cvstatus);
      }, () => { this.isLoading = false })
    } else {
      this.cvStatusService.update(this.cvstatus).pipe(catchError(this.cvStatusService.handleError)).subscribe((res) => {
        abp.notify.success("Update CV Status Successfully!");
        this.dialogRef.close(this.cvstatus);
      }, () => { this.isLoading = false })
    }
  }
}
