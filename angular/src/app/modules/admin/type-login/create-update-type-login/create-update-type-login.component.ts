import { TypeLoginService } from '../../../../service/api/type-login.service';
import { Inject, Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-create-update-type-login',
  templateUrl: './create-update-type-login.component.html',
  styleUrls: ['./create-update-type-login.component.css']
})
export class CreateUpdateTypeLoginComponent extends AppComponentBase implements OnInit {
  public title = '';
  public model: any = {};

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public typeLoginService: TypeLoginService,
    public dialogRef: MatDialogRef<CreateUpdateTypeLoginComponent>) { super(injector); }

  ngOnInit(): void {
    this.model = this.data.item || {};
    this.title = this.model.name || '';
  }

  SaveAndClose() {
    if (this.data.command === 'create') {
      this.typeLoginService.create(this.model).pipe(catchError(this.typeLoginService.handleError)).subscribe(() => {
        abp.notify.success('Create Type Login Successfully!');
        this.dialogRef.close(this.model);
      }, () => { this.isLoading = false; });
    } else {
      this.typeLoginService.update(this.model).pipe(catchError(this.typeLoginService.handleError)).subscribe(() => {
        abp.notify.success('Update Type Login Successfully!');
        this.dialogRef.close(this.model);
      }, () => { this.isLoading = false; });
    }
  }
}
