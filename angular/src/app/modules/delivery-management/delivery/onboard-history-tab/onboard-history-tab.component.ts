import { Component, Injector, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { catchError, finalize } from 'rxjs/operators';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { AppSessionService } from './../../../../../shared/session/app-session.service';
import { ProjectUserOnboardingService } from '@app/service/api/project-user-onboarding.service';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';
import { OffboardUserService } from '@app/service/api/offboard-user.service';
import { OnboardingDialogComponent } from '../../../pm-management/list-project/list-project-detail/resource-management/onboarding-dialog/onboarding-dialog.component';
import { PERMISSIONS_CONSTANT } from '@app/constant/permission.constant';

@Component({
  selector: 'app-onboard-history-tab',
  templateUrl: './onboard-history-tab.component.html',
  styleUrls: ['./onboard-history-tab.component.css']
})
export class OnboardHistoryTabComponent extends PagedListingComponentBase<any> implements OnInit {
  OnboardHistory = PERMISSIONS_CONSTANT.OnboardHistory;
  OnboardHistory_Edit = PERMISSIONS_CONSTANT.OnboardHistory_Edit;
  OnboardHistory_View = PERMISSIONS_CONSTANT.OnboardHistory_View;

  public onboardHistoryList: any[] = [];
  public listProject: any[] = [];
  public listPm: any[] = [];
  public projectId = -1;
  public selectedPmId = -1;
  public searchProject = '';
  public searchPm = '';
  public selectedStatus: number | null = null;
  public statusOptions = [
    { value: null, displayName: 'All' },
    { value: 0, displayName: 'Not Started' },
    { value: 1, displayName: 'In Progress' },
    { value: 2, displayName: 'Pending Employee' },
    { value: 3, displayName: 'Done' },
    { value: 4, displayName: 'Pending' },
  ];

  constructor(
    injector: Injector,
    private projectUserOnboardingService: ProjectUserOnboardingService,
    private resourceManagerService: ResourceManagerService,
    private offboardUserService: OffboardUserService,
    private dialog: MatDialog,
    public sessionService: AppSessionService
  ) {
    super(injector);
    this.selectedPmId = Number(this.sessionService.userId);
  }

  ngOnInit(): void {
    this.pageSize = this.pageSizeType;
    this.getProjectOptions();
    this.getPmOptions();
    super.ngOnInit();
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    const requestBody = {
      ...request,
      projectId: this.projectId === -1 ? null : this.projectId,
      pmId: this.selectedPmId === -1 ? null : this.selectedPmId,
      status: this.selectedStatus,
      searchText: this.searchText,
    };

    this.projectUserOnboardingService
      .GetAllOnboardHistory(requestBody)
      .pipe(
        catchError(this.projectUserOnboardingService.handleError),
        finalize(() => finishedCallback())
      )
      .subscribe((data) => {
        this.onboardHistoryList = data?.result?.items || [];
        this.showPaging(data?.result, pageNumber);
      });
  }

  protected delete(entity: any): void {
    // History tab has no delete action.
  }

  public getProjectOptions(): void {
    this.resourceManagerService
      .getProjectsForAllResource()
      .pipe(catchError(this.resourceManagerService.handleError))
      .subscribe((data) => {
        this.listProject = data?.result || [];
      });
  }

  public getPmOptions(): void {
    this.offboardUserService
      .GetAllPM()
      .pipe(catchError(this.offboardUserService.handleError))
      .subscribe((data) => {
        const rawList = data?.result || data || [];
        this.listPm = rawList.map((pm: any) => ({
          id: pm?.id ?? pm?.Id ?? pm?.PMId,
          fullName: pm?.fullName ?? pm?.FullName ?? pm?.PMName ?? '',
          emailAddress: pm?.emailAddress ?? pm?.EmailAddress ?? '',
        }));
      });
  }

  public onSearch(): void {
    this.getDataPage(1);
  }

  public onProjectChanged(): void {
    this.getDataPage(1);
  }

  public onPmChanged(): void {
    this.getDataPage(1);
  }

  public onStatusChanged(): void {
    this.getDataPage(1);
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

  public getOnboardStatusLabel(status: number): string {
    switch (status) {
      case 0:
        return 'Not Started';
      case 1:
        return 'In Progress';
      case 2:
        return 'Pending Employee';
      case 3:
        return 'Done';
      case 4:
        return 'Pending';
      default:
        return 'Unknown';
    }
  }

  public getOnboardStatusClass(status: number): string {
    switch (status) {
      case 0:
        return 'status-not-started';
      case 1:
        return 'status-in-progress';
      case 2:
        return 'status-pending-employee';
      case 3:
        return 'status-done';
      case 4:
        return 'status-pending';
      default:
        return 'status-unknown';
    }
  }

  public getPmDisplayName(pm: any): string {
    return pm?.fullName || pm?.emailAddress || '-';
  }

  public getProjectUserId(item: any): number {
    return item?.projectUserId ?? item?.id;
  }

  public openOnboardDialog(item: any): void {
    const projectUserId = this.getProjectUserId(item);

    const dialogRef = this.dialog.open(OnboardingDialogComponent, {
      width: '700px',
      maxHeight: '90vh',
      maxWidth: '95vw',
      data: {
        projectUserId,
        fullName: item.fullName,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getDataPage(1);
      }
    });
  }

  public remind(item: any): void {
    const projectUserId = this.getProjectUserId(item);

    abp.message.confirm(
      `Send reminder to ${item.fullName}?`,
      '',
      (result: boolean) => {
        if (result) {
          this.projectUserOnboardingService
            .remind(projectUserId)
            .pipe(catchError(this.projectUserOnboardingService.handleError))
            .subscribe(() => {
              abp.notify.success('Reminder sent successfully');
              this.getDataPage(1);
            });
        }
      }
    );
  }

}
