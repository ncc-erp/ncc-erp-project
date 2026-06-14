import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfigItService } from '@app/service/api/config-it.service';
import { ConfigItDto, ConfigItUserOptionDto } from '@app/service/model/config-it.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-create-edit-config-it',
  templateUrl: './create-edit-config-it.component.html',
  styleUrls: ['./create-edit-config-it.component.css']
})
export class CreateEditConfigItComponent extends AppComponentBase implements OnInit {
  title = '';
  item = {} as ConfigItDto;
  userList: ConfigItUserOptionDto[] = [];
  searchUser = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<CreateEditConfigItComponent>,
    private configItService: ConfigItService,
    public injector: Injector
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.item = this.data.item || {} as ConfigItDto;
    this.title = this.data.command === 'create' ? 'Create Config IT' : `Edit Config IT: ${this.item.fullName || ''}`;
    this.loadUsers();
  }

  loadUsers(): void {
    this.configItService.getAllUser().subscribe(res => {
      this.userList = res.result || res || [];
    });
  }

  save(): void {
    this.isLoading = true;
    const requestUserId = this.item.userId;

    if (this.data.command === 'create') {
      this.configItService.create(requestUserId).pipe(catchError(this.configItService.handleError)).subscribe(() => {
        abp.notify.success('Create Config IT successfully!');
        this.dialogRef.close(true);
      }, () => this.isLoading = false);
      return;
    }

    this.configItService.updateConfig(this.item.id, requestUserId).pipe(catchError(this.configItService.handleError)).subscribe(() => {
      abp.notify.success('Update Config IT successfully!');
      this.dialogRef.close(true);
    }, () => this.isLoading = false);
  }
}
