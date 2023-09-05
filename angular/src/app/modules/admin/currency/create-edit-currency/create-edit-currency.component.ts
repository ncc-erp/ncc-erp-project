import { catchError } from 'rxjs/operators';
import { CurrencyService } from './../../../../service/api/currency.service';
import { CurrencyDto } from './../../../../service/model/currency.dto';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AppComponentBase } from 'shared/app-component-base';
import { Component, Inject, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-create-edit-currency',
  templateUrl: './create-edit-currency.component.html',
  styleUrls: ['./create-edit-currency.component.css']
})
export class CreateEditCurrencyComponent extends AppComponentBase implements OnInit {

  public currency= {} as CurrencyDto;
  public title="";
  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
    public injector:Injector,
    public dialogRef:MatDialogRef<CreateEditCurrencyComponent>,
    public currencyService: CurrencyService) {super(injector) }

  ngOnInit(): void {
    this.currency= this.data.item;
    this.title= this.data.command;
  }
  SaveAndClose(){
    if(this.data.command == "create"){
      this.currencyService.create(this.currency).pipe(catchError(this.currencyService.handleError)).subscribe((res)=>{
        abp.notify.success("Create Skill Successfully!");
        this.dialogRef.close(this.currency);
      },()=>{this.isLoading=false})
    }else{
      this.currencyService.update(this.currency).pipe(catchError(this.currencyService.handleError)).subscribe((res)=>{
        abp.notify.success("Create Skill Successfully!");
        this.dialogRef.close(this.currency);
      },()=>{this.isLoading=false})
    }

  }

}
