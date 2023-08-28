import { catchError } from 'rxjs/operators';
import { result } from 'lodash-es';
import { ProjectCriteriaDto,CriteriaDto, CriteriaCategoryDto } from './../../../../../service/model/criteria-category.dto';
import { CriteriaService } from '@app/service/api/criteria.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';
import { CriteriaCategoryService } from '@app/service/api/criteria-category.service';

@Component({
  selector: 'app-create-edit-criteria',
  templateUrl: './create-edit-criteria.component.html',
  styleUrls: ['./create-edit-criteria.component.css']
})
export class CreateEditCriteriaComponent extends AppComponentBase implements OnInit {
  public criteria = {} as ProjectCriteriaDto;
  // public criteriaCategoryList: CriteriaCategoryDto
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateEditCriteriaComponent>,
    //private criteriaCategoryService: CriteriaCategoryService,
    private criteriaService: CriteriaService) { super(injector) }
  ngOnInit(): void {
    this.criteria = this.data.item;
    //this.getAllCriteriaCategory();
  }
  // public getAllCriteriaCategory(){
  //   this.criteriaCategoryService.GetAllNoPagging().subscribe((data)=>{
  //     //this.criteriaCategoryList=data.result;
  //   })
  // }
  SaveAndClose() {
    if (this.data.command == "create") {
      this.criteriaService.create(this.criteria).pipe(catchError(this.criteriaService.handleError)).subscribe((res) => {
        abp.notify.success("Create Successfully!");
        this.dialogRef.close(this.criteria);
      }, () => { this.isLoading = false })
    } else {
      this.criteriaService.update(this.criteria).pipe(catchError(this.criteriaService.handleError)).subscribe((res) => {
        abp.notify.success("Update Successfully!");
        this.dialogRef.close(this.criteria);
      }, () => { this.isLoading = false })
    }
  }
  checkValue(e) {
    if(e.checked == true){
      this.criteria.isActive = true;
    }
  }
}
