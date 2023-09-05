import { result } from 'lodash-es';
import { AppConfigurationService } from './../../../../../../../service/api/app-configuration.service';
import { ConfigurationServiceProxy } from './../../../../../../../../shared/service-proxies/service-proxies';
import { ConfigurationComponent, ConfigurationDto } from './../../../../../../admin/configuration/configuration.component';
import { ReviewInternRetroHisotyDto } from './../../../../../../../service/model/delivery-management.dto';
import { RetroReviewInternHistoriesDto } from './../../../../../../../service/model/resource-plan.dto';
import { ResourceManagerService } from '@app/service/api/resource-manager.service';
import { Inject, Input } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ProjectUserService } from './../../../../../../../service/api/project-user.service';
import { PlanUserComponent } from './../plan-user.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { OnDestroy, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-retro-review-history-by-user',
  templateUrl: './retro-review-history-by-user.component.html',
  styleUrls: ['./retro-review-history-by-user.component.css']
})
export class RetroReviewHistoryByUserComponent extends AppComponentBase
  implements OnInit, OnDestroy {
  emailAddress: string;
  reviewInternRetroHisotyDto: ReviewInternRetroHisotyDto[] = [];
  subscription: Subscription[] = [];
  configuration = {} as ConfigurationDto;
  constructor(
    @Inject(MAT_DIALOG_DATA)
    public data: {
      item: {
        emailAddress: string;
      };
    },
    public injector: Injector,
    public dialogRef: MatDialogRef<PlanUserComponent>,
    private projectUserService: ResourceManagerService,
    private appConfigurationService: AppConfigurationService
  ) {
    super(injector);
    this.emailAddress = data.item.emailAddress
  }
  ngOnInit(): void {
    this.GetRetroAndReviewInternHistories();
    
  }
  public GetRetroAndReviewInternHistories() {
    this.isLoading = true
    this.projectUserService
      .GetTimesheetOfRetroReviewInternHistories({emails:[this.emailAddress]})
      .pipe(catchError(this.projectUserService.handleError))
      .subscribe((data) => {
        this.reviewInternRetroHisotyDto = data.result;
        this.isLoading = false
      });
  }
  ngOnDestroy() {
    this.subscription.forEach((sub) => {
      sub.unsubscribe();
    });
  }
  public getStarColorforReviewInternCapability(average, isClass) {
    if (average < 2.5) {
      return 'grey'
    }
    if (average < 3.5) {
      return 'yellow'
    }
    if (average < 4.5) {
      return 'orange'
    }
    else {
      return ''
    }
  }
}