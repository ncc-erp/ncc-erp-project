import { catchError } from "rxjs/operators";
import { result } from "lodash-es";
import { OnboardingTemplateChecklistService } from "@app/service/api/onboarding-template-checklist.service";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { AppComponentBase } from "@shared/app-component-base";
import { Component, OnInit, Injector, Inject } from "@angular/core";
import { OnboardingTemplateDto } from "@app/service/model/onboarding-template.dto";
@Component({
  selector: "app-create-edit-onboarding-checklist",
  templateUrl: "./create-edit-onboarding-checklist.component.html",
  styleUrls: ["./create-edit-onboarding-checklist.component.css"],
})
export class CreateEditOnboardingChecklistComponent
  extends AppComponentBase
  implements OnInit
{
  public onboardingTemplate = {} as OnboardingTemplateDto;

  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: any,
    public injector: Injector,
    public dialogRef: MatDialogRef<CreateEditOnboardingChecklistComponent>,
    private onboardingService: OnboardingTemplateChecklistService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.onboardingTemplate = this.data.item;
  }

  SaveAndClose() {
    this.onboardingService
      .save(this.onboardingTemplate)
      .pipe(catchError(this.onboardingService.handleError))
      .subscribe(
        (res) => {
          abp.notify.success("Create Successfully!");
          this.dialogRef.close(this.onboardingTemplate);
        },
        () => {
          this.isLoading = false;
        },
      );
  }

  checkValue(e) {}
}
