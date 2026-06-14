import { Component, OnInit, Injector } from "@angular/core";
import {
  PagedRequestDto,
  PagedListingComponentBase,
} from "@shared/paged-listing-component-base";
import { MatDialog } from "@angular/material/dialog";
import { PERMISSIONS_CONSTANT } from "@app/constant/permission.constant";
import { OnboardingTemplateChecklistService } from "@app/service/api/onboarding-template-checklist.service";
import { finalize, catchError } from "rxjs/operators";
import { OnboardingTemplateDto } from "@app/service/model/onboarding-template.dto";
import { MatMenuTrigger } from "@angular/material/menu";
import { ActivatedRoute } from "@angular/router";
import { CreateEditOnboardingChecklistComponent } from "./create-edit-onboarding-checklist/create-edit-onboarding-checklist.component";

@Component({
  selector: "app-onboarding-checklist",
  templateUrl: "./onboarding-checklist.component.html",
  styleUrls: ["./onboarding-checklist.component.css"],
})
export class OnboardingChecklistComponent
  extends PagedListingComponentBase<OnboardingChecklistComponent>
  implements OnInit
{
  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    this.onboardingTemplateChecklistService
      .getAllPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
      )
      .subscribe(
        (data) => {
          this.onboardingTemplateList = data.result.items;
          this.showPaging(data.result, pageNumber);
        },
        () => {},
      );
  }
  delete(item): void {
    abp.message.confirm(
        "Delete project item " + item.name + "?",
        "",
        (result: boolean) => {
          if (result) {
            this.onboardingTemplateChecklistService.delete(item.id).pipe(catchError(this.onboardingTemplateChecklistService.handleError)).subscribe((res) => {
              abp.notify.success("Delete " + item.name + " successfully!");
              this.refresh();
            })
          }
        }
      )
  }

  Admin_OnboardingChecklist_View =
    PERMISSIONS_CONSTANT.Admin_OnboardingChecklist_View;
  Admin_OnboardingChecklist_Create =
    PERMISSIONS_CONSTANT.Admin_OnboardingChecklist_Create;
  Admin_OnboardingChecklist_Edit =
    PERMISSIONS_CONSTANT.Admin_OnboardingChecklist_Edit;
  Admin_OnboardingChecklist_Delete =
    PERMISSIONS_CONSTANT.Admin_OnboardingChecklist_Delete;

  public onboardingTemplateList: OnboardingTemplateDto[] = [];
  public isShowMenu: boolean;
  menu: MatMenuTrigger;
  contextMenuPosition = { x: "0px", y: "0px" };

  constructor(
    public injector: Injector,
    private onboardingTemplateChecklistService: OnboardingTemplateChecklistService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
  ) {
    super(injector);
    this.isShowMenu =
      this.permission.isGranted(this.Admin_OnboardingChecklist_Edit) ||
      this.permission.isGranted(this.Admin_OnboardingChecklist_Delete) ||
      this.permission.isGranted(this.Admin_OnboardingChecklist_Create);
  }

  ngOnInit(): void {
    this.refresh();
  }
  public showDialog(command: string, onboardingTemplate) {
    let item = {
      label: onboardingTemplate.label,
      details: onboardingTemplate.details,
      id: onboardingTemplate.id,
      isDeleted: onboardingTemplate.isDeleted,
      order: onboardingTemplate.order,
    };
    const show = this.dialog.open(CreateEditOnboardingChecklistComponent, {
      data: {
        item: item,
        command: command,
      },
      width: "60%",
      height: "90%",
      panelClass: "create-edit-onboarding-checklist-dialog",
    });
    show.afterClosed().subscribe((res) => {
      if (res) {
        this.refresh();
      }
    });
  }
  public create() {
    this.showDialog("create", {});
  }
  public edit(item) {
    this.showDialog("edit", item);
  }

  showActions(e, item) {
    e.preventDefault();
    this.contextMenuPosition.x = e.clientX + "px";
    this.contextMenuPosition.y = e.clientY + "px";
    this.menu.openMenu();
  }
}
