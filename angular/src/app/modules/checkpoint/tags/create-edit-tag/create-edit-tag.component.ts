import { catchError } from 'rxjs/operators';
import { TagsService } from './../../../../service/api/tags.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from 'shared/app-component-base';
import { Component, Inject, OnInit, Injector } from '@angular/core';
import { TagsDto } from '@app/service/model/tags.dto';

@Component({
  selector: 'app-create-edit-tag',
  templateUrl: './create-edit-tag.component.html',
  styleUrls: ['./create-edit-tag.component.css']
})
export class CreateEditTagComponent extends AppComponentBase implements OnInit {

  public tag={} as TagsDto;
  constructor(@Inject(MAT_DIALOG_DATA) public data:any,public injector:Injector,
  public dialogRef: MatDialogRef<CreateEditTagComponent>,
  public tagService: TagsService
  ) {super(injector) }

  ngOnInit(): void {
    this.tag= this.data.item;
  }
  SaveAndClose(){
    if(this.data.command == "create"){
      this.tagService.create(this.tag).pipe(catchError(this.tagService.handleError)).subscribe((data)=>{
        abp.notify.success("Create Successfully!");
        this.dialogRef.close(this.tag);
      },()=>{this.isLoading= false})

    }else{
      this.tagService.update(this.tag).pipe(catchError(this.tagService.handleError)).subscribe((data)=>{
        abp.notify.success("Edit Successfully!");
        this.dialogRef.close(this.tag);
      },()=>{this.isLoading= false})
    }
  }



  

}
