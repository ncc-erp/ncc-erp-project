import { CreateEditCriteriaCategoryComponent } from './create-edit-criteria-category/create-edit-criteria-category.component';
import { MatDialog } from '@angular/material/dialog';
import { result } from 'lodash-es';
import { catchError } from 'rxjs/operators';
import { CriteriaCategoryService } from './../../../../service/api/criteria-category.service';
import { CriteriaCategoryDto } from './../../../../service/model/criteria-category.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-category-criteria',
  templateUrl: './category-criteria.component.html',
  styleUrls: ['./category-criteria.component.css']
})
export class CategoryCriteriaComponent extends PagedListingComponentBase<CategoryCriteriaComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.pageSizeType=50;
    this.criteriaCategoryService.getAllPaging(request).pipe(catchError(this.criteriaCategoryService.handleError)).subscribe((data)=>{
      this.criteriaCategoryList= data.result.items;
      this.showPaging(data.result, pageNumber);
    })
  }
  protected delete(item): void {
    abp.message.confirm(
      "Delete criteria category"+ item.name +"?",
      "",
      (result:boolean)=>{
        if(result){
          this.criteriaCategoryService.delete(item.id).pipe(catchError(this.criteriaCategoryService.handleError)).subscribe((res)=>{
            abp.notify.success("Delete"+ item.name+ "successfully!");
            this.refresh();
          })
        }

      }
      
    )
  }

  public criteriaCategoryList:CriteriaCategoryDto[]=[];
  constructor(public injector:Injector,
    private criteriaCategoryService: CriteriaCategoryService,
    public dialog: MatDialog) {super(injector) }

  ngOnInit(): void {
    this.refresh();

  }
  showDialog(command:string, CriteriaCategory){
    let item= {} as CriteriaCategoryDto;
    item={
      name:CriteriaCategory.name,
      id:CriteriaCategory.id
    }
    const show= this.dialog.open(CreateEditCriteriaCategoryComponent,{
      data:{
        item:item,
        width:"700px",
        command:command
      }
    })
    show.afterClosed().subscribe((res)=>{
      if(res){
        this.refresh();
      }
    })

  }
  public create(){
    this.showDialog("create", {});
  }
  public edit(item){
    this.showDialog("edit", item);
  }
  showCriteria(item){
    this.router.navigate(['app/criteria'], {
      queryParams: {
        id: item.id,
      }
    })
  }
  

}
