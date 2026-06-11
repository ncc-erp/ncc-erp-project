import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { OffboardUserService } from '@app/service/api/offboard-user.service';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';
import { catchError, finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { OffboardDialogComponent } from './offboard-dialog/offboard-dialog.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';


@Component({
  selector: 'app-offboard-history-tab',
  templateUrl: './offboard-history-tab.component.html',
  styleUrls: ['./offboard-history-tab.component.css']
})
export class OffboardHistoryTabComponent extends PagedListingComponentBase<any> implements OnInit {
  OffboardHistory = PERMISSIONS_CONSTANT.OffboardHistory;
  OffboardHistory_View = PERMISSIONS_CONSTANT.OffboardHistory_View;
  OffboardHistory_Edit = PERMISSIONS_CONSTANT.OffboardHistory_Edit;
  OffboardHistory_CheckList = PERMISSIONS_CONSTANT.OffboardHistory_CheckList;
  OffboardHistory_CheckList_PM = PERMISSIONS_CONSTANT.OffboardHistory_CheckList_PM;
  OffboardHistory_CheckList_IT = PERMISSIONS_CONSTANT.OffboardHistory_CheckList_IT;

  public offboardHistoryList: any[] = [];
  public listProject: any[] = [];
  public projectId = -1;
  public searchProject: string = '';
  public selectedStatus: number | null = null;
  public statusOptions = [
    { value: null, displayName: 'All' },
    { value: 0, displayName: 'Todo' },
    { value: 1, displayName: 'PMOffboard' },
    { value: 2, displayName: 'ITOffboard' },
    { value: 3, displayName: 'Complete' },
  ];

  constructor(
    private offboardUserService: OffboardUserService,
    private resourceManagerService: ResourceManagerService,
    injector: Injector,
    private dialog: MatDialog
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.pageSize = this.pageSizeType;
    this.getProjectOptions();
    super.ngOnInit();
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    const requestBody = {
      ...request,
      projectId: this.projectId === -1 ? null : this.projectId,
      offboardStatus: this.selectedStatus,
      searchText: this.searchText,
    };

    this.offboardUserService
      .GetAllOffboardHistory(requestBody)
      .pipe(
        catchError(this.offboardUserService.handleError),
        finalize(() => finishedCallback())
      )
      .subscribe((data) => {
        this.offboardHistoryList = data?.result?.items || [];
        this.showPaging(data?.result, pageNumber);
      });
  }

  protected delete(entity: any): void {
    // No delete action required for history table
  }

  public getProjectOptions(): void {
    this.resourceManagerService
      .getProjectsForAllResource()
      .pipe(catchError(this.resourceManagerService.handleError))
      .subscribe((data) => {
        this.listProject = data?.result || [];
      });
  }

  public showProjectDetail(project: any): void {
    let routingToUrl = '/app/list-project-detail/list-project-general';

    if (project.projectType === 5) {
      routingToUrl = '/app/training-project-detail/training-project-general';
    } else if (project.projectType === 3) {
      routingToUrl = '/app/product-project-detail/product-project-general';
    }

    const url = this.router.serializeUrl(
      this.router.createUrlTree([routingToUrl], {
        queryParams: {
          id: project.projectId,
          type: project.projectType,
          projectName: project.projectName,
          projectCode: project.projectCode,
        },
      })
    );

    window.open(url, '_blank');
  }

  public getOffboardStatusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Todo';
      case 1:
        return 'PMOffboard';
      case 2:
        return 'ITOffboard';
      case 3:
        return 'Complete';
      default:
        return 'Unknown';
    }
  }

  public getOffboardStatusClass(status: number): string {
    switch (status) {
      case 0:
        return 'status-todo';
      case 1:
        return 'status-pmoffboard';
      case 2:
        return 'status-itoffboard';
      case 3:
        return 'status-complete';
      default:
        return 'status-unknown';
    }
  }

  public onUpdateOffboardStatus(item: any, needOffboard: boolean): void {
    const actionText = needOffboard ? 'Need Offboard' : "Don't need Offboard";
    abp.message.confirm(
      `${actionText} for ${item.fullName}?`,
      '',
      (result: boolean) => {
        if (result) {
          const requestBody = {
            offboardHistoryId: item.id,
            needOffboard,
          };

          this.offboardUserService
            .UpdateOffboardStatus(requestBody)
            .pipe(catchError(this.offboardUserService.handleError))
            .subscribe(() => {
              abp.notify.success(`${actionText} successfully`);
              this.getDataPage(1);
            });
        }
      }
    );
  }

  public onSearch(): void {
    this.getDataPage(1);
  }

  public onProjectChanged(): void {
    this.getDataPage(1);
  }

  public onStatusChanged(): void {
    this.getDataPage(1);
  }

  public openOffboardDialog(item: any): void {
    const isReadOnly =
      item.offboardStatus === 2 &&
      this.permission.isGranted(this.OffboardHistory_CheckList_PM) &&
      !this.permission.isGranted(this.OffboardHistory_CheckList_IT);

    const dialogRef = this.dialog.open(OffboardDialogComponent, {
      width: '720px',
      data: { offboardHistoryId: item.id, fullName: item.fullName, viewOnly: isReadOnly }
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getDataPage(1);
      }
    });
  }

  public moveToIT(item: any): void {
    abp.message.confirm(
      `Move to IT for ${item.fullName}?`,
      '',
      (result: boolean) => {
        if (result) {
          this.offboardUserService
            .MoveToIT(item.id)
            .pipe(catchError(this.offboardUserService.handleError))
            .subscribe(() => {
              abp.notify.success('Moved to IT successfully');
              this.getDataPage(1);
            });
        }
      }
    );
  }

  public moveToComplete(item: any): void {
    abp.message.confirm(
      `Move to Complete for ${item.fullName}?`,
      '',
      (result: boolean) => {
        if (result) {
          this.offboardUserService
            .MoveToComplete(item.id)
            .pipe(catchError(this.offboardUserService.handleError))
            .subscribe(() => {
              abp.notify.success('Moved to Complete successfully');
              this.getDataPage(1);
            });
        }
      }
    );
  }

  public parseHistoryAsset(historyAsset: string): any[] {
    if (!historyAsset) {
      return [];
    }

    try {
      const parsed = JSON.parse(historyAsset);
      return Array.isArray(parsed) ? parsed : [];
    } catch {
      return [];
    }
  }

  public parseHistoryAccountResource(historyAccountResource: string): any[] {
    if (!historyAccountResource) {
      return [];
    }

    try {
      const parsed = JSON.parse(historyAccountResource);
      return Array.isArray(parsed) ? parsed : [];
    } catch {
      return [];
    }
  }

  public getHistoryDisplayValue(item: any): string {
    if (typeof item === 'string') {
      return item || '-';
    }

    if (item?.Value != null && item.Value !== '') {
      return item.Value;
    }

    if (item?.value != null && item.value !== '') {
      return item.value;
    }

    if (item?.AssetName || item?.assetName) {
      return item.AssetName || item.assetName;
    }

    return item?.name || '-';
  }
}
