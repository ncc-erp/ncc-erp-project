import { Component, Inject, Injector, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { PMReportProjectContributionService } from "@app/service/api/pmreport-project-contribution.service";
import { ProjectUserBillService } from "@app/service/api/project-user-bill.service";
import { catchError } from "@node_modules/rxjs/operators";
import { Utils } from "@shared/Utils";
import { IGetUserInfo } from "@app/service/model/user.inteface";
import { ResourceManagerService } from "@app/service/api/resource-manager.service";

@Component({
  selector: "app-review-contribution",
  templateUrl: "./review-contribution.component.html",
  styleUrls: ["./review-contribution.component.css"],
})
export class ReviewContributionComponent implements OnInit {
  public Utils = Utils;
  isLoading: boolean = false;
  public listAllResource: any[] = [];
  public listAvailableResource: any[] = [];
  public searchResource: string = "";
  public selectedResource: any = null;

  constructor(
    public dialogRef: MatDialogRef<ReviewContributionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private pmReportProjectContributionService: PMReportProjectContributionService,
    private projectUserBillService: ProjectUserBillService,
    private resourceManagerService: ResourceManagerService,
  ) {}

  editingRows: { [key: string]: boolean } = {};

  tempContributeValues: { [key: string]: number } = {};

  ngOnInit(): void {
    this.getListUserAndResources();
  }

  private getListUserAndResources() {
    this.resourceManagerService.GetListAllUserShortInfo().subscribe((data) => {
      this.listAllResource = data.result.filter((item) => item.isActive);
    });
  }

  public addLinkResource(userBill: any): void {
    let listLinkedResourceId = userBill.linkedResources.map((item) => item.id);
    this.listAvailableResource = this.listAllResource.filter(
      (resource) => !listLinkedResourceId.includes(resource.id),
    );
    userBill.showAddLinkInPopup = true;
    userBill.tempContribute = 0;
  }

  public cancelLinkResource(userBill: any): void {
    userBill.showAddLinkInPopup = false;
    this.selectedResource = null;
    this.searchResource = "";
  }

  public saveLinkResource(userBill: any): void {
    const reqAdd = {
      projectUserBillId: userBill.id,
      userId: this.selectedResource,
      contribute: userBill.tempContribute || 0,
      pmReportId: this.data.pmReportId,
    };

    this.isLoading = true;
    this.projectUserBillService.LinkOneProjectUserBillAccount(reqAdd).subscribe(
      (data) => {
        abp.notify.success("Linked resource added successfully");
        const userInfo: IGetUserInfo = data.result;
        userBill.linkedResources.push(userInfo);
        this.cancelLinkResource(userBill);
        this.isLoading = false;
      },
      () => (this.isLoading = false),
    );
  }

  edit(res: any, puId: any) {
    const key = `${puId}_${res.id}`;
    this.tempContributeValues[key] = res.contribute;
    this.editingRows[key] = true;
  }

  cancelUpdate(res: any, puId: any) {
    const key = `${puId}_${res.id}`;
    res.contribute = this.tempContributeValues[key];
    this.editingRows[key] = false;
    delete this.tempContributeValues[key];
  }

  removeLinkResource(resId: any, puId: any) {
    const req = {
      projectUserBillId: puId,
      userIds: [resId],
      pmReportId: this.data.pmReportId,
    };

    abp.message.confirm("Remove linked resource?", "", (result: boolean) => {
      if (result) {
        this.isLoading = true;
        this.projectUserBillService.RemoveLinkedResource(req).subscribe(
          () => {
            abp.notify.success(`Removed successfully!`);
            const bill = this.data.projectUserBills.find((b) => b.id === puId);
            if (bill) {
              bill.linkedResources = bill.linkedResources.filter(
                (r) => r.id !== resId,
              );
            }
            const key = `${puId}_${resId}`;
            delete this.tempContributeValues[key];
            delete this.editingRows[key];
            this.isLoading = false;
          },
          () => (this.isLoading = false),
        );
      }
    });
  }

  saveWeeklyContribute(res: any, puId: any) {
    this.isLoading = true;
    const key = `${puId}_${res.id}`;
    const request = {
      userId: res.id,
      projectUserBillId: puId,
      contribute: res.contribute,
      projectId: this.data.projectId,
      pmReportId: this.data.pmReportId,
      ...(res.weeklyContributionHistoryId && {
        id: res.weeklyContributionHistoryId,
      }),
    };

    this.pmReportProjectContributionService
      .updateWeeklyHistory(request)
      .subscribe(
        () => {
          abp.notify.success(
            `Weekly contributions have been updated successfully!`,
          );
          this.editingRows[key] = false;
          delete this.tempContributeValues[key];
          this.isLoading = false;
        },
        () => {
          res.contribute = this.tempContributeValues[key];
          delete this.tempContributeValues[key];
          this.isLoading = false;
        },
      );
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.data.projectUserBills.forEach((pu) => {
      pu.showAddLinkInPopup = false;
      pu.createLinkResourceMode = false;
    });
    this.dialogRef.close(false);
  }
}
