import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { PunishmentDto } from './../../service/model/punishment.dto';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { catchError, finalize } from 'rxjs/operators';
import { CreatePunishmentDialogComponent } from './create-punishment-dialog/create-punishment-dialog.component';
import { PunishmentService } from '@app/service/api/punishment.service'
import { FileHandlerService } from '@app/service/utility/file-handler.service';
import { InputFilterDto } from '@shared/filter/filter.component';

@Component({
  selector: 'app-punishment',
  templateUrl: './punishment.component.html',
  styleUrls: ['./punishment.component.css']
})
export class PunishmentComponent extends PagedListingComponentBase<PunishmentDto> implements OnInit {

  punishments: PunishmentDto[] = [];

  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'fileName', displayName: "File Name", comparisions: [0, 6, 7, 8] },
    { propertyName: 'month', displayName: "Month", comparisions: [0, 1, 2, 3, 4] },
    { propertyName: 'year', displayName: "Year", comparisions: [0, 1, 2, 3, 4] },
  ];

  constructor(
    injector: Injector,
    private _punishmentService: PunishmentService,
    private _modalService: BsModalService,
    private fileHandlerService: FileHandlerService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh()
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    request.sort = this.transDate;
    request.sortDirection = this.sortDrirect;
    this._punishmentService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this._punishmentService.handleError)).subscribe(data => {

      this.punishments = data.result.items;
      this.showPaging(data.result, pageNumber);
    })
  }

  protected delete(entity: PunishmentDto): void {
    abp.message.confirm(
      "Delete Punishment " + entity.note + "?",
      "",
      (result: boolean) => {
        if (result) {
          this._punishmentService.delete(entity.id).pipe(catchError(this._punishmentService.handleError)).subscribe(() => {
            abp.notify.success("Deleted Punishment " + entity.note);
            this.refresh()
          });
        }
      }
    );
  }

  createPunishment(): void {
    this.showCreateOrEditDialog();
  }

  downloadTemplate() {
    this._punishmentService.getTemplate().subscribe(res => {
      if (res.success) {
        this.fileHandlerService.downloadFile(
          res.result.base64,
          res.result.fileName
        );
      }
    });
  }

  editPunishment(item: PunishmentDto): void {
    this.showCreateOrEditDialog(item.id);
  }

  downloadFile(id: number) {
    this._punishmentService.downloadFile(id).subscribe(data => {
      this.fileHandlerService.downloadFile(data.result.data, data.result.fileName);
    });
  }

  private showCreateOrEditDialog(id?: number): void {
    let createOrEditDialog: BsModalRef;
    if (!id) {
      createOrEditDialog = this._modalService.show(
        CreatePunishmentDialogComponent,
        { class: 'modal-lg' }
      );
    } else {
      createOrEditDialog = this._modalService.show(
        CreatePunishmentDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}