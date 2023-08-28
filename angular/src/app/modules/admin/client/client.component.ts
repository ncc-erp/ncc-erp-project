import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { CreateUpdateClientComponent } from './create-update-client/create-update-client.component';
import { MatDialog } from '@angular/material/dialog';
import { result } from 'lodash-es';
import { finalize, catchError } from 'rxjs/operators';
import { ClientDto } from '@app/service/model/list-project.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { ClientService } from './../../../service/api/client.service';
import { Component, OnInit, inject, Injector } from '@angular/core';
import { isNgTemplate } from '@angular/compiler';
import { InputFilterDto } from '@shared/filter/filter.component';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})

export class ClientComponent extends PagedListingComponentBase<ClientComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: "Tên", },

  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.clientService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.clientService.handleError)).subscribe(data => {
      this.clientList = data.result.items;
      this.showPaging(data.result, pageNumber)
    })
  }

  protected delete(client: ClientComponent): void {
    abp.message.confirm(
      "Delete Client" + client.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.clientService.deleteClient(client.id).pipe(catchError(this.clientService.handleError)).subscribe((res) => {
            abp.notify.success("Delele Client" + client.name);
            this.refresh()
          })
        }
      }
    )
  }

  public clientList: ClientDto[] = [];
  Admin_Clients_View = PERMISSIONS_CONSTANT.Admin_Clients_View;
  Admin_Clients_Create = PERMISSIONS_CONSTANT.Admin_Clients_Create;
  Admin_Clients_Edit = PERMISSIONS_CONSTANT.Admin_Clients_Edit;
  Admin_Clients_Delete = PERMISSIONS_CONSTANT.Admin_Clients_Delete;

  constructor(private clientService: ClientService,
    injector: Injector,
    private dialog: MatDialog) { super(injector) }

  ngOnInit(): void {
    this.refresh();
  }
  public showDialog(command: string, Client: any) {
    let client = {
      name: Client.name,
      code: Client.code,
      id: Client.id,
      address: Client.address,
      invoiceDateSetting: Client.invoiceDateSetting,
      paymentDueBy: Client.paymentDueBy,
      transferFee: Client.transferFee,
    }
    const show = this.dialog.open(CreateUpdateClientComponent, {
      data: {
        item: client,
        command: command
      },
      width: "700px"
    })
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    })

  }
  public createClient() {
    this.showDialog("create", {});
  }
  public editClient(client) {
    this.showDialog("update", client);

  }
  public getByEnum(enumValue: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == enumValue) {
        return key;
      }
    }
  }
}
