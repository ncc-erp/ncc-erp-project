import { Component, Inject, Injector, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PositionService } from '@app/service/api/position.service';
import { PositionDto } from '@app/service/model/position.dto';
import { AppComponentBase } from '@shared/app-component-base';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-create-update-position',
  templateUrl: './create-update-position.component.html',
  //styleUrls: ['./create-update-position.component.css']
})
export class CreateUpdatePositionComponent  extends AppComponentBase implements OnInit  {

  title:string =""
  public position = {} as PositionDto;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
    public injector: Injector,
    public positionService: PositionService,
    public dialogRef: MatDialogRef<CreateUpdatePositionComponent>,) { super(injector) }

  ngOnInit(): void {
    if(this.data.command == "update"){
      this.position = this.data.item;
      this.title = this.data.item.name ? this.data.item.name : ''
    }
    else{
      var randomColor = Math.floor(Math.random() * 16777215).toString(16)
      this.position.color = "#" + randomColor;
    }
  }
  
  nameChange(event){
    if(!this.data.item?.id){
      this.position.shortName = event;
      this.position.code = event;
    }
  }

  SaveAndClose() {
    if (this.data.command == "create") {
      this.positionService.create(this.position).pipe(catchError(this.positionService.handleError)).subscribe((res) => {
        abp.notify.success("Create position Successfully!");
        this.dialogRef.close(this.position);
      }, () => { this.isLoading = false })
    }
    else {
      this.positionService.update(this.position).pipe(catchError(this.positionService.handleError)).subscribe((res) => {
        abp.notify.success("Update position Successfully!");
        this.dialogRef.close(this.position);
      }, () => { this.isLoading = false })
    }
  }

}
