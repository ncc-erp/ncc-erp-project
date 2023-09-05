import { PERMISSIONS_CONSTANT } from './../../../constant/permission.constant';
import { catchError, finalize } from 'rxjs/operators';
import { ChecklistService } from './../../../service/api/checklist.service';
import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { InputFilterDto } from '@shared/filter/filter.component';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { CreateChecklistItemComponent } from './create-checklist-item/create-checklist-item.component';
import { ChecklistDto } from '@app/service/model/checklist.dto';

@Component({
  selector: 'app-checklist',
  templateUrl: './checklist.component.html',
  styleUrls: ['./checklist.component.css']
})
export class ChecklistComponent extends PagedListingComponentBase<any> implements OnInit {


  checklistList: ChecklistDto[] = []
  public projectTypeList: any[] = Object.keys(this.APP_ENUM.ProjectType);
  
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.isTLoading = true
    this.checklistService.getAllPaging(request).pipe(finalize(() => {
      finishedCallback();
    }), catchError(this.checklistService.handleError)).subscribe(data => {
      this.isTLoading = false
      this.checklistList = data.result.items;
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
          this.checklistService.delete(entity.id).pipe(catchError(this.checklistService.handleError)).subscribe(() => {
            abp.notify.success("Deleted checklist: " + entity.name);
            this.refresh()
          });
        }
      }
    );
  }

  public projectTypeForFilter = 
  [{ displayName: "ODC", value: 0 },
  { displayName: "TimeAndMaterials", value: 1 }, { displayName: "FIXPRICE", value: 2 },
  { displayName: "PRODUCT", value: 3 },{ displayName: "NoBill", value: 4 },{ displayName: "TRAINING", value: 5 }

  ]
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [0, 6, 7, 8], displayName: "Name" },
    { propertyName: 'categoryName', comparisions: [0, 6, 7, 8], displayName: "Category Name" },
    { propertyName: 'code', comparisions: [0, 6, 7, 8], displayName: "Code" },
    { propertyName: 'mandatory', comparisions: [3], displayName: "Mandatory" , filterType : 3 , dropdownData: this.projectTypeForFilter },
    { propertyName: 'personInCharge', comparisions: [0, 6, 7, 8], displayName: "Person in Charge" },
    { propertyName: 'auditTarget', comparisions: [0, 6, 7, 8], displayName: "auditTarget" },
    { propertyName: 'description', comparisions: [0, 6, 7, 8], displayName: "description" },
    { propertyName: 'note', comparisions: [0, 6, 7, 8], displayName: "note" },
   

  ];
  

  constructor(injector: Injector, public dialog: MatDialog, private checklistService: ChecklistService) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh()
  }
  public createChecklistTitle() {
    this.showDialogChecklistTitle("create");
  }

  public getProjectTypefromEnum(projectType: number, enumObject: any) {
    for (const key in enumObject) {
      if (enumObject[key] == projectType) {
        return key;
      }
    }
  }
  public editCheckList(checklist: ChecklistDto) {
    this.showDialogChecklistTitle("edit", checklist)
  }

  private showDialogChecklistTitle(command: string, checkList?: ChecklistDto) {
    let dialogData = {} as ChecklistDto
    if (command == "edit") {
      dialogData = {
        id: checkList.id,
        name: checkList.name,
        code: checkList.code,
        title: checkList.title,
        auditTarget: checkList.auditTarget,
        categoryId: checkList.categoryId,
        description: checkList.description,
        mandatorys: checkList.mandatorys,
        personInCharge: checkList.personInCharge,
        note: checkList.note
      }
    }
    const dialogRef = this.dialog.open(CreateChecklistItemComponent, {
      width: '700px',
      maxHeight: '100vh',
      disableClose: true,
      data: {
        dialogData: dialogData,
        command: command,
      },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.refresh()
      }
    });
  }
}
