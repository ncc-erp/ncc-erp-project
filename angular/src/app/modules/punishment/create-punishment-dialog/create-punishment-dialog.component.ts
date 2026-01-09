import { Component, Injector, Output, EventEmitter } from "@angular/core";
import { BsModalRef } from "ngx-bootstrap/modal";
import { AppComponentBase } from "@shared/app-component-base";
import { finalize } from "rxjs/operators";
import { PunishmentDto } from "../../../service/model/punishment.dto";
import { PunishmentService } from "@app/service/api/punishment.service";

@Component({
  templateUrl: "./create-punishment-dialog.component.html",
})
export class CreatePunishmentDialogComponent extends AppComponentBase {
  saving = false;
  punishment: PunishmentDto = new PunishmentDto();
  fileToUpload: File | null = null;
  selectedFileName: string = "";
  id?: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _punishmentService: PunishmentService
  ) {
    super(injector);
    const now = new Date();
    this.punishment.month = now.getMonth() + 1;
    this.punishment.year = now.getFullYear();
  }

  ngOnInit(): void {
    if (this.id) {
      this._punishmentService.getById(this.id).subscribe((r) => {
        this.punishment = r.result;
        console.log(this.punishment);
      });
    } else {
      const now = new Date();
      this.punishment.month = now.getMonth() + 1;
      this.punishment.year = now.getFullYear();
    }
  }

  onFileSelect(event: any) {
    if (event.target.files.length > 0) {
      this.fileToUpload = event.target.files[0];
      this.selectedFileName = this.fileToUpload.name;
    }
  }

  save(): void {
    this.saving = true;

    if (this.id) {
      this.doUpdate();
    } else {
      this.doCreate();
    }
  }

  private doCreate(): void {
    if (!this.fileToUpload) {
      this.notify.warn("Please select a file to upload.");
      return;
    }
    this.saving = true;
    this._punishmentService
      .importPunishment(this.punishment, this.fileToUpload)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(
        () => {
          this.notify.info("Record imported successfully");
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        (err) => {
          this.notify.error(err.error.message);
        }
      );
  }

  private doUpdate(): void {
    this._punishmentService
      .update(this.punishment, this.fileToUpload)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(
        () => {
          this.notify.info("Updated successfully");
          this.close();
        },
        (err) => {
           this.notify.error(err.error.message);
        }
      );
  }

  private close(): void {
    this.bsModalRef.hide();
    this.onSave.emit();
  }
}
