import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConfigItService } from '@app/service/api/config-it.service';
import { ConfigItDto } from '@app/service/model/config-it.dto';
import { CreateEditConfigItComponent } from './create-edit-config-it/create-edit-config-it.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { AppComponentBase } from '@shared/app-component-base';
@Component({
  selector: 'app-config-it',
  templateUrl: './config-it.component.html',
  styleUrls: ['./config-it.component.css']
})
export class ConfigItComponent extends AppComponentBase implements OnInit {

  configList: ConfigItDto[] = [];
  searchText = '';
  isLoading = false;

  constructor(
    private configItService: ConfigItService,
    private dialog: MatDialog,
    injector: Injector
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getData();
  }

  Admin_ConfigITs_View = PERMISSIONS_CONSTANT.Admin_ConfigITs_View;
  Admin_ConfigITs_Create = PERMISSIONS_CONSTANT.Admin_ConfigITs_Create
  Admin_ConfigITs_Edit = PERMISSIONS_CONSTANT.Admin_ConfigITs_Edit;
  Admin_ConfigITs_Delete = PERMISSIONS_CONSTANT.Admin_ConfigITs_Delete;

  getData(): void {
    this.isLoading = true;
    this.configItService.getAll(this.searchText).subscribe(res => {
      this.configList = res.result || res || [];
      this.isLoading = false;
    }, () => this.isLoading = false);
  }

  create(): void {
    this.openDialog('create', {});
  }

  edit(item: ConfigItDto): void {
    this.openDialog('update', item);
  }

  delete(item: ConfigItDto): void {
    abp.message.confirm(`Delete config for ${item.fullName || item.emailAddress}?`, '', (result: boolean) => {
      if (result) {
        this.configItService.delete(item.id).subscribe(() => {
          abp.notify.success('Delete successfully!');
          this.getData();
        });
      }
    });
  }

  private openDialog(command: string, item: any): void {
    const ref = this.dialog.open(CreateEditConfigItComponent, {
      width: '700px',
      data: { command, item }
    });

    ref.afterClosed().subscribe((res) => {
      if (res) {
        this.getData();
      }
    });
  }
}
