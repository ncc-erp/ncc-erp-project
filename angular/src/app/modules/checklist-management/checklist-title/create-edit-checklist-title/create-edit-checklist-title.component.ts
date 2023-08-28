import { catchError } from 'rxjs/operators';
import { DialogDataDto } from './../../../../service/model/common-DTO';
import { ChecklistTitleDto } from './../../../../service/model/checklist.dto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { ChecklistCategoryService } from './../../../../service/api/checklist-category.service';
import { Component, OnInit, Injector, Inject } from '@angular/core';

@Component({
  selector: 'app-create-edit-checklist-title',
  templateUrl: './create-edit-checklist-title.component.html',
  styleUrls: ['./create-edit-checklist-title.component.css']
})
export class CreateEditChecklistTitleComponent extends AppComponentBase implements OnInit {
  isEdit: boolean;
  constructor(private checkListTitleService: ChecklistCategoryService, injector: Injector, @Inject(MAT_DIALOG_DATA) public data: DialogDataDto,
  public dialogRef: MatDialogRef<CreateEditChecklistTitleComponent>) {
    super(injector)
  }
  checklistTitle = {} as ChecklistTitleDto;
  ngOnInit(): void {
    if(this.data.command =="edit"){
      this.checklistTitle = this.data.dialogData
    }
  }
  public saveAndClose(): void {
    this.isLoading = true;
    if(this.data.command =="create"){
      this.checkListTitleService.create(this.checklistTitle).pipe(catchError(this.checkListTitleService.handleError)).subscribe(res => {
        abp.notify.success("created checklist title: "+this.checklistTitle.name);
        this.dialogRef.close(this.checklistTitle)
      }, () => this.isLoading = false);
    }
    else if(this.data.command == "edit"){
      this.checkListTitleService.update(this.checklistTitle).pipe(catchError(this.checkListTitleService.handleError)).subscribe(res => {
        abp.notify.success("Edited: "+this.checklistTitle.name);
        this.dialogRef.close(this.checklistTitle)
      }, () => this.isLoading = false);
    }
   
  }
}
