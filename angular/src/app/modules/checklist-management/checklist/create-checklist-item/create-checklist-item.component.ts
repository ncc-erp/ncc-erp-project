import { catchError } from 'rxjs/operators';
import { DialogDataDto } from './../../../../service/model/common-DTO';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChecklistDto } from './../../../../service/model/checklist.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { ChecklistService } from './../../../../service/api/checklist.service';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { ChecklistCategoryService } from '@app/service/api/checklist-category.service';
import { ChecklistTitleComponent } from '../../checklist-title/checklist-title.component';

@Component({
  selector: 'app-create-checklist-item',
  templateUrl: './create-checklist-item.component.html',
  styleUrls: ['./create-checklist-item.component.css']
})
export class CreateChecklistItemComponent extends AppComponentBase implements OnInit {
  public checklist = {} as ChecklistDto;
  public checklistTitleList: ChecklistTitleComponent[] = []
  public projectTypeList: string[] = Object.keys(this.APP_ENUM.ProjectType)
  public searchTitle: string = "";
  public submitted: boolean = false;
  public setManda: boolean = false;

  constructor(injector: Injector, private checkListService: ChecklistService, @Inject(MAT_DIALOG_DATA) public data: DialogDataDto,
    private checklisttitleService: ChecklistCategoryService,
    public dialogRef: MatDialogRef<CreateChecklistItemComponent>) {
    super(injector)
  }

  ngOnInit(): void {
    this.getChecklistTitle()
    if (this.data.command == "edit") {
      this.checklist = this.data.dialogData
    };
    console.log(this.data)
  }
  selectProjectType(e) {
    if (this.setManda && !(this.checklist.mandatorys.length > 0)) {
      this.submitted = true;
    } else {
      this.submitted = false;
    }

  }
  public saveAndClose(): void {
    this.isLoading = true;
    if (this.setManda && !(this.checklist?.mandatorys?.length > 0)) {
      this.submitted = true;
      abp.notify.error("Nhập đầy đủ các trường!")
    }
    if (!this.submitted)
      if (this.data.command == "create") {
        if (!this.checklist.mandatorys) {
          this.checklist.mandatorys = [];
        }
        this.checkListService.create(this.checklist).pipe(catchError(this.checkListService.handleError)).subscribe(res => {
          abp.notify.success(`create checklist ${this.checklist.name}`);
          this.dialogRef.close(this.checklist);
        });
      }
      else if (this.data.command == "edit") {
        this.checkListService.update(this.checklist).pipe(catchError(this.checkListService.handleError)).subscribe(res => {
          abp.notify.success(`edited checklist ${this.checklist.name}`);
          this.dialogRef.close(this.checklist);
        });
      }
  }
  private getChecklistTitle(): void {
    this.checklisttitleService.getAll().subscribe(data => {
      this.checklistTitleList = data.result
    })
  }
  public setMandatory(event): void {
    if (event.checked == false) {
      this.checklist.mandatorys = [];

    } else {
      this.setManda = true;
    }
  }
}
