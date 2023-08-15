import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';
import { InputFilterDto } from './../../../../shared/filter/filter.component';
import { CreateEditCurrencyComponent } from './create-edit-currency/create-edit-currency.component';
import { MatDialog } from '@angular/material/dialog';
import { CurrencyDto } from './../../../service/model/currency.dto';
import { finalize, catchError } from 'rxjs/operators';
import { CurrencyService } from './../../../service/api/currency.service';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { Component, OnInit, Injector } from '@angular/core';

@Component({
  selector: 'app-currency',
  templateUrl: './currency.component.html',
  styleUrls: ['./currency.component.css']
})
export class CurrencyComponent extends PagedListingComponentBase<CurrencyComponent> implements OnInit {
  public readonly FILTER_CONFIG: InputFilterDto[] = [
    { propertyName: 'name', comparisions: [6, 0, 7, 8], displayName: "Tên", },
   
  ];
  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
      this.currencyService.getAllPaging(request).pipe(finalize(()=>{
        finishedCallback();
      }),catchError(this.currencyService.handleError)).subscribe(data=>{
        this.listCurrency=data.result.items;
        this.showPaging(data.result, pageNumber)
      })
  }
  protected delete(currency: CurrencyComponent): void {
    abp.message.confirm(
      "Delete Currency " + currency.name+ "?",
      "",
      (result:boolean)=>{
        if(result){
          this.currencyService.deleteCurrency(currency.id).pipe(catchError(this.currencyService.handleError)).subscribe((res)=>{
            abp.notify.success("Delele Currency "+ currency.name);
            this.refresh()
          })
        }
      }
    )
  }
  public Admin_Currencies_View = PERMISSIONS_CONSTANT.Admin_Currencies_View;
  public Admin_Currencies_Create = PERMISSIONS_CONSTANT.Admin_Currencies_Create;
  public Admin_Currencies_Edit = PERMISSIONS_CONSTANT.Admin_Currencies_Edit;
  public Admin_Currencies_Delete = PERMISSIONS_CONSTANT.Admin_Currencies_Delete;
 
  public searchText="";
  public listCurrency: CurrencyDto[]=[];
  constructor(public injector:Injector, private currencyService: CurrencyService , 
    public dialog: MatDialog) {super(injector) }

  ngOnInit(): void {
    this.refresh();
  }

  createCurrency(){
    this.showDialog("create",{});

  }
  editCurrency(currency){
    this.showDialog("edit",currency)
  }
  
  showDialog(command:String,Currency){
    let currency={
      name: Currency.name,
      code: Currency.code,
      id: Currency.id,
      invoicePaymentInfo: Currency.invoicePaymentInfo
    }
    const show= this.dialog.open(CreateEditCurrencyComponent, {
      data:{
        command: command,
        item:currency
      },
      width:"700px"
    })
    show.afterClosed().subscribe((res)=>{
      if(res){
        this.refresh();
      }
    })


  }

}
