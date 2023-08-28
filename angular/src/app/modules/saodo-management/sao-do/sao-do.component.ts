import { InputFilterDto } from '@shared/filter/filter.component';
import { PERMISSIONS_CONSTANT } from './../../../constant/permission.constant';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateEditSaodoComponent } from './create-edit-saodo/create-edit-saodo.component';
import { catchError, finalize } from 'rxjs/operators';
import { SaodoService } from './../../../service/api/saodo.service';
import { SaodoDto } from './../../../service/model/saodo.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit,  Injector } from '@angular/core';

import * as moment from 'moment';

@Component({
  selector: 'app-sao-do',
  templateUrl: './sao-do.component.html',
  styleUrls: ['./sao-do.component.css']
})
export class SaoDoComponent extends PagedListingComponentBase<SaodoDto> implements OnInit {
  // SaoDo_AuditSession = PERMISSIONS_CONSTANT.SaoDo_AuditSession;
  // SaoDo_AuditSession_AddAuditResult = PERMISSIONS_CONSTANT.SaoDo_AuditSession_AddAuditResult;
  // SaoDo_AuditSession_Create = PERMISSIONS_CONSTANT.SaoDo_AuditSession_Create;
  // SaoDo_AuditSession_Delete = PERMISSIONS_CONSTANT.SaoDo_AuditSession_Delete;
  // SaoDo_AuditSession_Update = PERMISSIONS_CONSTANT.SaoDo_AuditSession_Update;
  // SaoDo_AuditSession_View = PERMISSIONS_CONSTANT.SaoDo_AuditSession_View;
  // SaoDo_AuditSession_ViewAll = PERMISSIONS_CONSTANT.SaoDo_AuditSession_ViewAll;
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Đợt", },
    { propertyName: 'countFail', comparisions: [0, 1, 2, 3, 4], displayName: "Tổng sai phạm", },
  ];
  listSaoDo :SaodoDto[]=[];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.saodoService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.saodoService.handleError)).subscribe(data => {
      this.listSaoDo = data.result.items;
      this.showPaging(data.result, pageNumber);
    })
  }
  protected delete(item: SaodoDto): void {
    abp.message.confirm(
      "Delete Audit " + item.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.saodoService.deleteAuditSession(item.id).pipe(catchError(this.saodoService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Audit " + item.name);
            this.refresh()
          });
        }
      }
    );
    
  }
  constructor(injector:Injector,private saodoService : SaodoService,
    private dialog: MatDialog,
    private route: ActivatedRoute) {
    
    super(injector)
   }

  ngOnInit(): void {
    this.refresh();
    this.requestId = this.route.snapshot.queryParamMap.get("id");
  }
  showDialog(command: String, Saodo:any): void {
    let saodo = {} as SaodoDto
    
    if (command == "edit") {
      saodo={
        name: Saodo.name,
        startTime: Saodo.startTime,
        endTime: Saodo.endTime?Saodo.endTime:0,
        id:Saodo.id
      }
      if(!saodo.endTime){
        delete saodo["endTime"]
      }
    }

    const show=this.dialog.open(CreateEditSaodoComponent, {
      data: {
        item: saodo,
        command: command,
      },
      width: "700px",
      disableClose: true,
    });
    show.afterClosed().subscribe(result => {
      if(result){
        this.refresh()
      }
    });

  }
  createSaodo() {
    this.showDialog('create', {})
  }
  editSaodo(saodo: SaodoDto) {
    this.showDialog("edit",saodo);
  }
  showDetail(saodo:any){
    this.router.navigate(['app/saodoDetail'], {
      queryParams: {
        id: saodo.id,
        name:saodo.name
      }
    })
  }

}
