import { Component, Injector, OnInit } from "@angular/core";
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from "../../../shared/paged-listing-component-base";
import { catchError, finalize } from "rxjs/operators";
import { PMReportProjectContributionService } from "../../service/api/pmreport-project-contribution.service";
import { WeeklyContributionDto } from "../../service/model/weekly-contribution.dto";

@Component({
  selector: "app-contribution-reports",
  templateUrl: "./weekly-contribution.component.html",
  styleUrls: ["./weekly-contribution.component.css"],
})
export class WeeklyContributionComponent
  extends PagedListingComponentBase<WeeklyContributionDto>
  implements OnInit
{
  weeklyContributions: WeeklyContributionDto[] = [];
  constructor(
    injector: Injector,
    private PMReportProjectContributionService: PMReportProjectContributionService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }
  protected list(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    this.PMReportProjectContributionService.getAllWeeklyContribution(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        }),
        catchError(this.PMReportProjectContributionService.handleError),
      )
      .subscribe((data) => {
        this.weeklyContributions = data.result.items;
        this.showPaging(data.result, pageNumber);
      });
  }

   showDetail(item: any) {
    this.router.navigate(['app/detail-weekly-contribution'], {
      queryParams: {
        pmReportId: item.pmReportId
      }
    })

  }
  protected delete(entity: WeeklyContributionDto): void {}
}
