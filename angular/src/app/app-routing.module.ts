import { ProjectBillComponent } from './modules/pm-management/list-project/list-project-detail/project-bill/project-bill.component';
import { VendorComponent } from './modules/delivery-management/delivery/available-resource-tab/vendor/vendor.component';
import { ProductProjectDetailComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-detail.component';
import { ProductProjectDescriptionComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-description/product-project-description.component';
import { ProductProjectTimesheetComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-timesheet/product-project-timesheet.component';
import { ProductWeeklyReportComponent } from './modules/pm-management/product-projects/product-project-detail/product-weekly-report/product-weekly-report.component';
import { ProductResourceManagementComponent } from './modules/pm-management/product-projects/product-project-detail/product-resource-management/product-resource-management.component';
import { ProductProjectGeneralComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-general/product-project-general.component';
import { TrainingMilestoneComponent } from './modules/pm-management/training-projects/training-project-detail/training-milestone/training-milestone.component';
import { TrainingProjectTimesheetComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-timesheet/training-project-timesheet.component';
import { TrainingWeeklyReportComponent } from './modules/pm-management/training-projects/training-project-detail/training-weekly-report/training-weekly-report.component';
import { TrainingResourceManagementComponent } from './modules/pm-management/training-projects/training-project-detail/training-resource-management/training-resource-management.component';
import { ProductProjectsComponent } from './modules/pm-management/product-projects/product-projects.component';
import { TrainingProjectGeneralComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-general/training-project-general.component';
import { ProjectDescriptionComponent } from './modules/pm-management/list-project/list-project-detail/project-description/project-description.component';
import { ReviewYourselfComponent } from './modules/checkpoint/review-yourself/review-yourself.component';
import { ReviewUserComponent } from './modules/checkpoint/review-user/review-user.component';
import { ResultReviewerComponent } from './modules/checkpoint/set-up-reviewer/result-reviewer/result-reviewer.component';
import { SetUpReviewerComponent } from './modules/checkpoint/set-up-reviewer/set-up-reviewer.component';
import { TagsComponent } from './modules/checkpoint/tags/tags.component';
import { PhaseComponent } from './modules/checkpoint/phase/phase.component';
import { CriteriaComponent } from './modules/checkpoint/category/criteria/criteria.component';
import { CategoryCriteriaComponent } from './modules/checkpoint/category/category-criteria/category-criteria.component';
import { ClientComponent } from './modules/admin/client/client.component';
import { FutureResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/future-resource/future-resource.component';
import { PlanResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-resource.component';
import { ProjectTimesheetComponent } from './modules/pm-management/list-project/list-project-detail/project-timesheet/project-timesheet.component';
import { AvailableResourceTabComponent } from './modules/delivery-management/delivery/available-resource-tab/available-resource-tab.component';
import { ResourceRequestDetailComponent } from './modules/delivery-management/delivery/request-resource-tab/resource-request-detail/resource-request-detail.component';
import { RequestResourceTabComponent } from './modules/delivery-management/delivery/request-resource-tab/request-resource-tab.component';
import { WeeklyReportTabDetailComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/weekly-report-tab-detail.component';
import { WeeklyReportTabComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab.component';
import { SaoDoProjectDetailComponent } from './modules/saodo-management/sao-do/sao-do-detail/sao-do-project-detail/sao-do-project-detail.component';
import { InvoiceComponent } from './modules/timesheet/invoice/invoice.component';
import { SaoDoDetailComponent } from './modules/saodo-management/sao-do/sao-do-detail/sao-do-detail.component';
import { DeliveryComponent } from './modules/delivery-management/delivery/delivery.component';
import { ProjectChecklistComponent } from './modules/pm-management/list-project/list-project-detail/project-checklist/project-checklist.component';
import { WeeklyReportComponent } from './modules/pm-management/list-project/list-project-detail/weekly-report/weekly-report.component';
import { MilestoneComponent } from './modules/pm-management/list-project/list-project-detail/milestone/milestone.component';
import { ResourceManagementComponent } from './modules/pm-management/list-project/list-project-detail/resource-management/resource-management.component';
import { ListProjectDetailComponent } from './modules/pm-management/list-project/list-project-detail/list-project-detail.component';
import { TimesheetDetailComponent } from './modules/timesheet/timesheet-detail/timesheet-detail.component';
import { ListProjectComponent } from './modules/pm-management/list-project/list-project.component';
import { ChecklistComponent } from './modules/checklist-management/checklist/checklist.component';
import { ChecklistTitleComponent } from './modules/checklist-management/checklist-title/checklist-title.component';
import { TimesheetComponent } from './modules/timesheet/timesheet.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { SaoDoComponent } from './modules/saodo-management/sao-do/sao-do.component';
import { ListProjectGeneralComponent } from './modules/pm-management/list-project/list-project-detail/list-project-general/list-project-general.component';
import { ProjectGeneralComponent } from './modules/pm-management/project-detail/project-general/project-general.component';
import { SkillComponent } from './modules/admin/skill/skill.component';
import { ConfigurationComponent } from './modules/admin/configuration/configuration.component';
import { CurrencyComponent } from './modules/admin/currency/currency.component';
import { AllResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/all-resource/all-resource.component';
import { TrainingProjectsComponent } from './modules/pm-management/training-projects/training-projects.component';
import { TrainingProjectDetailComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-detail.component';
import { TrainingProjectChecklistComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-checklist/training-project-checklist.component';
import { TrainingProjectDescriptionComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-description/training-project-description.component';
import { ProductProjectMilestoneComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-milestone/product-project-milestone.component';
import { ProductProjectChecklistComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-checklist/product-project-checklist.component';
import { ProjectFileComponent } from './modules/pm-management/list-project/list-project-detail/project-file/project-file.component';
import * as path from 'path';
import { EditRoleDialogComponent } from './roles/edit-role/edit-role-dialog.component';
import { TrainingProjectFileComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-file/training-project-file.component';
import { ProductProjectFileComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-file/product-project-file.component';
import { BranchComponent } from './modules/admin/branch/branch.component';
import { TechnologyComponent } from './modules/admin/technology/technology.component';
import { PositionComponent } from './modules/admin/position/position.component';
import { TrainingRequestTabComponent } from './modules/delivery-management/delivery/training-request-tab/training-request-tab.component';
import { AuditLogComponent } from './modules/admin/audit-log/audit-log.component';
import { CriteriaManagementComponent } from './modules/audit-management/criteria-management/criteria-management.component';
import { TailoringManagementComponent } from './modules/audit-management/criteria-management/tailoring-management/tailoring-management/tailoring-management.component';
import {AuditResultComponent} from './modules/audit-management/result-management/audit-result/audit-result/audit-result.component'
import { ProjectCriteriaAuditComponent } from './modules/audit-management/criteria-management/project-criteria-audit/project-criteria-audit.component';
import { TaloringProjectComponent } from './modules/audit-management/criteria-management/taloring-project/taloring-project.component';
import { AuditResultDetailComponent } from './modules/audit-management/result-management/audit-result/audit-result/audit-result-detail/audit-result-detail.component';
@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: "",
        component: AppComponent,
        children: [
          {
            path: "home",
            component: HomeComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "users",
            component: UsersComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "roles",
            component: RolesComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "audit-logs",
            component: AuditLogComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: 'criterias',
            component: CriteriaComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "edit-role",
            component: EditRoleDialogComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "tenants",
            component: TenantsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "clients",
            component: ClientComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "branchs",
            component: BranchComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "positions",
            component: PositionComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "technologies",
            component: TechnologyComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "configurations",
            component: ConfigurationComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "currency",
            component: CurrencyComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "skills",
            component: SkillComponent,
            canActivate: [AppRouteGuard],
          },
          { path: "about", component: AboutComponent },
          { path: "update-password", component: ChangePasswordComponent },
          // timesheet
          {
            path: "timesheet",
            component: TimesheetComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "invoice",
            component: InvoiceComponent,
            canActivate: [AppRouteGuard],
          },

          {
            path: "checklist-title",
            component: ChecklistTitleComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "checklist",
            component: ChecklistComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "sao-do",
            component: SaoDoComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "list-project",
            component: ListProjectComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "training-projects",
            component: TrainingProjectsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "product-projects",
            component: ProductProjectsComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "timesheetDetail",
            component: TimesheetDetailComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "list-project-detail",
            component: ListProjectDetailComponent,
            canActivate: [AppRouteGuard],
            children: [
              {
                path: "list-project-general",
                component: ListProjectGeneralComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "resourcemanagement",
                component: ResourceManagementComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "milestone",
                component: MilestoneComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "weeklyreport",
                component: WeeklyReportComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "projectchecklist",
                component: ProjectChecklistComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "timesheet-tab",
                component: ProjectTimesheetComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "description-tab",
                component: ProjectDescriptionComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "project-file-tab",
                component: ProjectFileComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "project-bill-tab",
                component: ProjectBillComponent,
                canActivate: [AppRouteGuard]
              }
            ],
          },
          {
            path: "training-project-detail",
            component: TrainingProjectDetailComponent,
            canActivate: [AppRouteGuard],
            children: [
              {
                path: "training-project-general",
                component: TrainingProjectGeneralComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "training-resource-management",
                component: TrainingResourceManagementComponent
              },
              {
                path: "training-milestone",
                component: TrainingMilestoneComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "training-weekly-report",
                component: TrainingWeeklyReportComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "training-project-checklist",
                component: TrainingProjectChecklistComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "training-timesheet-tab",
                component: TrainingProjectTimesheetComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "training-description-tab",
                component: TrainingProjectDescriptionComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "training-project-file-tab",
                component: TrainingProjectFileComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "project-bill-tab",
                component: ProjectBillComponent,
                canActivate: [AppRouteGuard]
              }
            ],
          },
          {
            path: "product-project-detail",
            component: ProductProjectDetailComponent,
            canActivate: [AppRouteGuard],
            children: [
              {
                path: "product-project-general",
                component: ProductProjectGeneralComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "product-resource-management",
                component: ProductResourceManagementComponent
              },
              {
                path: "product-milestone",
                component: ProductProjectMilestoneComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "product-weekly-report",
                component: ProductWeeklyReportComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "product-project-checklist",
                component: ProductProjectChecklistComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "product-timesheet-tab",
                component: ProductProjectTimesheetComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "product-description-tab",
                component: ProductProjectDescriptionComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "product-project-file-tab",
                component: ProductProjectFileComponent,
                canActivate: [AppRouteGuard]
              },
              {
                path: "project-bill-tab",
                component: ProjectBillComponent,
                canActivate: [AppRouteGuard]
              }
            ],
          },
          {
            path: "weekly-report",
            component: WeeklyReportTabComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "resource-request",
            component: RequestResourceTabComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "training-request",
            component: TrainingRequestTabComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "available-resource",
            component: AvailableResourceTabComponent,
            canActivate: [AppRouteGuard],
            children: [
              {
                path: "pool",
                component: PlanResourceComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "future-resource",
                component: FutureResourceComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "all-resource",
                component: AllResourceComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: "vendor",
                component: VendorComponent,
                canActivate: [AppRouteGuard],
              }
            ]
          },
          {
            path: "tags",
            component: TagsComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "phase",
            component: PhaseComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "setup-reviewer",
            component: SetUpReviewerComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "result-reviewer",
            component: ResultReviewerComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "review-user",
            component: ReviewUserComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "review-yourself",
            component: ReviewYourselfComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "category-criteria",
            component: CategoryCriteriaComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "criteria",
            component: CriteriaComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "resourceRequestDetail",
            component: ResourceRequestDetailComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "weeklyReportTabDetail",
            component: WeeklyReportTabDetailComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "saodoDetail",
            component: SaoDoDetailComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "saodoProjectDetail",
            component: SaoDoProjectDetailComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "criteria-management",
            component: CriteriaManagementComponent,
            canActivate: [AppRouteGuard]
          },
          {
            path: "tailoring-project-edit",
            component: ProjectCriteriaAuditComponent,
            canActivate: [AppRouteGuard]
          }
          ,
          {
            path: "tailoring-project",
            component: TaloringProjectComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path: "tailoring-management",
            component: TailoringManagementComponent,
            canActivate: [AppRouteGuard],
          },
          {
            path:"audit-result",
            component:AuditResultComponent,
            canActivate:[AppRouteGuard]
          },
          {
            path:"auditResultDetail",
            component:AuditResultDetailComponent,
            canActivate:[AppRouteGuard]
          },
        ]
      }
    ])
  ],
  exports: [RouterModule],
})
export class AppRoutingModule { }
