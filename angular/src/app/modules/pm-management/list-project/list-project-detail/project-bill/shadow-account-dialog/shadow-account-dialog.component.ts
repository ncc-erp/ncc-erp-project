import { startWith, map } from "rxjs/operators";
import { ShadowAccountService } from "./../../../../../../service/api/shadow-account.service";
import { AppComponentBase } from "@shared/app-component-base";
import { Component, OnInit, Injector } from "@angular/core";
import { FormControl } from "@angular/forms";
import { BsModalRef } from "ngx-bootstrap/modal";
import { ResourceManagerService } from "@app/service/api/resource-manager.service";

@Component({
  selector: "app-shadow-account-dialog",
  templateUrl: "./shadow-account-dialog.component.html",
  styleUrls: ["./shadow-account-dialog.component.css"],
})
export class ShadowAccountDialogComponent
  extends AppComponentBase
  implements OnInit
{
  projectId: number;
  userId: number;
  shadowAccount = new FormControl("");
  filteredOptions;
  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private shadowAccountService: ShadowAccountService,
    private resourceManagerService:ResourceManagerService
  ) {
    super(injector);
  }

  handleChangeShadowAccount()
  {
    console.log('mess');
  }

  ngOnInit() {
    this.filteredOptions = this.shadowAccount.valueChanges.pipe(
      startWith(""),
      map((value) => {
        return value;
      })
    ).subscribe(value=>{
      console.log(value);
    })
  }
}
