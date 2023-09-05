import { PERMISSIONS_CONSTANT } from './../../../constant/permission.constant';
import { ChecklistTitleDto } from './../../../service/model/checklist.dto';
import { ChecklistCategoryService } from './../../../service/api/checklist-category.service';
import { finalize, catchError } from 'rxjs/operators';
import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { InputFilterDto } from '@shared/filter/filter.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { CreateEditChecklistTitleComponent } from './create-edit-checklist-title/create-edit-checklist-title.component';

@Component({
  selector: 'app-checklist-title',
  templateUrl: './checklist-title.component.html',
  styleUrls: ['./checklist-title.component.css']
})
export class ChecklistTitleComponent extends PagedListingComponentBase<ChecklistTitleDto> implements OnInit {



  public checkListTitleList: ChecklistTitleDto[] = []
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'Name', comparisions: [0, 6, 7, 8], displayName: "Name" },
  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.isLoading = true
    this.checklistTileService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.checklistTileService.handleError)).subscribe(data => {
      this.isLoading = false
      this.checkListTitleList = data.result.items;
      this.showPaging(data.result, pageNumber);
    },
      () => {
        this.isLoading = false
      })
  }
  protected delete(entity: any): void {
    abp.message.confirm(
      "Delete checklist: " + entity.name + "?",
      "",
      (result: boolean) => {
        if (result) {
          this.checklistTileService.delete(entity.id).pipe(catchError(this.checklistTileService.handleError)).subscribe(() => {
            abp.notify.success("Deleted check list title: " + entity.name);
            this.refresh()
          });
        }
      }
    );
  }

  constructor(injector: Injector, public dialog: MatDialog, private checklistTileService: ChecklistCategoryService) {
    super(injector);
  }
  public createChecklistTitle(): void {
    this.showDialogChecklistTitle("create");
  }
  public editchecklistTitle(checkList: ChecklistTitleDto): void {
    this.showDialogChecklistTitle("edit", checkList)
  }
  public showDialogChecklistTitle(command, checklist?: ChecklistTitleDto): void {
    let checkListTitle = {} as ChecklistTitleDto;
    if (command == "edit") {
      checkListTitle = {
        name: checklist.name,
        id: checklist.id
      }
    }
    const dialogRef = this.dialog.open(CreateEditChecklistTitleComponent, {
      width: '500px',
      disableClose: true,
      data: {
        dialogData: checkListTitle,
        command: command,
      },

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refresh()
      }
    });
  }

  ngOnInit(): void {
    this.refresh()
  }

}
