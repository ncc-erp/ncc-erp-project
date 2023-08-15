import { CreateEditTagComponent } from './create-edit-tag/create-edit-tag.component';
import { MatDialog } from '@angular/material/dialog';
import { catchError } from 'rxjs/operators';
import { TagsService } from './../../../service/api/tags.service';
import { TagsDto } from './../../../service/model/tags.dto';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.css']
})
export class TagsComponent extends PagedListingComponentBase<TagsComponent> implements OnInit {
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    this.pageSizeType=50;
    this.tagsService.getAllPaging(request).pipe(catchError(this.tagsService.handleError)).subscribe((data)=>{
      this.tagsList= data.result.items;
      this.showPaging(data.result, pageNumber);
    })
  }
  protected delete(tag): void {
    abp.message.confirm(
      "Delete Tag"+ tag.name +"?",
      "",
      (result:boolean)=>{
        if(result){
          this.tagsService.delete(tag.id).pipe(catchError(this.tagsService.handleError)).subscribe((res)=>{
            abp.notify.success("Delete"+ tag.name+ "successfully!");
            this.refresh();
          })
        }

      }
      
    )
  }
  public tagsList:TagsDto[]=[];

  constructor(public injector:Injector,
    public tagsService : TagsService,
    public dialog: MatDialog) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
    

  }
  showDialog(command:String, Tag){
    let tag= {} as TagsDto;
    if(command=="edit"){
      tag={
        name:Tag.name,
        id:Tag.id
      }
    }
    const show= this.dialog.open(CreateEditTagComponent,{
      data:{
        item:tag,
        width:"700px",
        command:command
      }
    });
    show.afterClosed().subscribe((res)=>{
      if(res){
        this.refresh();
      }
    })

  }
  
  public create(){
    this.showDialog("create",{});
  }
  public edit(tag){
    this.showDialog("edit",tag);
  }

}
