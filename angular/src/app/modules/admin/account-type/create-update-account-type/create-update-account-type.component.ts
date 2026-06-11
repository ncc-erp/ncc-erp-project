import { AccountTypeService } from './../../../../service/api/account-type.service';
import { AccountTypeDto } from './../../../../service/model/list-project.dto';
import { Inject, Injector } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'app-create-update-account-type',
  templateUrl: './create-update-account-type.component.html',
  styleUrls: ['./create-update-account-type.component.css']
})
export class CreateUpdateAccountTypeComponent extends AppComponentBase implements OnInit {
  public title = '';
  public accountType = {} as AccountTypeDto;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public accountTypeService: AccountTypeService,
    public dialogRef: MatDialogRef<CreateUpdateAccountTypeComponent>) { super(injector); }

  ngOnInit(): void {
    this.accountType = this.data.item;
    this.title = this.accountType.name;
  }

  SaveAndClose() {
    if (this.data.command === 'create') {
      this.accountTypeService.create(this.accountType).pipe(catchError(this.accountTypeService.handleError)).subscribe(() => {
        abp.notify.success('Create AccountType Successfully!');
        this.dialogRef.close(this.accountType);
      }, () => { this.isLoading = false; });
    } else {
      this.accountTypeService.update(this.accountType).pipe(catchError(this.accountTypeService.handleError)).subscribe(() => {
        abp.notify.success('Update AccountType Successfully!');
        this.dialogRef.close(this.accountType);
      }, () => { this.isLoading = false; });
    }
  }
}
