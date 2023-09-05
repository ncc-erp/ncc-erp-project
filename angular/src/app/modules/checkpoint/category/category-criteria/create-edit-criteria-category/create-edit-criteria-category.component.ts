import { catchError } from 'rxjs/operators';
import { CriteriaCategoryService } from './../../../../../service/api/criteria-category.service';
import { CriteriaCategoryDto } from './../../../../../service/model/criteria-category.dto';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Inject } from '@angular/core';

@Component({
  selector: 'app-create-edit-criteria-category',
  templateUrl: './create-edit-criteria-category.component.html',
  styleUrls: ['./create-edit-criteria-category.component.css']
})
export class CreateEditCriteriaCategoryComponent extends AppComponentBase implements OnInit {
  public criteriaCategory={} as CriteriaCategoryDto;

  constructor(@Inject(MAT_DIALOG_DATA) public data:any,public injector:Injector,
    public dialogRef: MatDialogRef<CreateEditCriteriaCategoryComponent>,
    private criteriaCategoryService: CriteriaCategoryService) {super(injector) }

  ngOnInit(): void {
    this.criteriaCategory=this.data.item;
  }
  SaveAndClose(){
    if(this.data.command == "create"){
      this.criteriaCategoryService.create(this.criteriaCategory).pipe(catchError(this.criteriaCategoryService.handleError)).subscribe((data)=>{
        abp.notify.success("Create Successfully!");
        this.dialogRef.close(this.criteriaCategory);
      },()=>{this.isLoading= false})

    }else{
      this.criteriaCategoryService.update(this.criteriaCategory).pipe(catchError(this.criteriaCategoryService.handleError)).subscribe((data)=>{
        abp.notify.success("Edit Successfully!");
        this.dialogRef.close(this.criteriaCategory);
      },()=>{this.isLoading= false})
    }
  }

}
