import { AppConsts } from "@shared/AppConsts";
import { Ng2SearchPipeModule } from "ng2-search-filter";
import { IConfig, NgxMaskModule } from "ngx-mask";
import { ProjectChecklistComponent } from "./modules/pm-management/list-project/list-project-detail/project-checklist/project-checklist.component";
import { MilestoneComponent } from "./modules/pm-management/list-project/list-project-detail/milestone/milestone.component";
import { WeeklyReportComponent } from "./modules/pm-management/list-project/list-project-detail/weekly-report/weekly-report.component";
import { ResourceManagementComponent } from "./modules/pm-management/list-project/list-project-detail/resource-management/resource-management.component";
import { ListProjectDetailComponent } from "./modules/pm-management/list-project/list-project-detail/list-project-detail.component";
import { TimesheetDetailComponent } from "./modules/timesheet/timesheet-detail/timesheet-detail.component";
import { ListProjectComponent } from "./modules/pm-management/list-project/list-project.component";
import { CreateEditListProjectComponent } from "./modules/pm-management/list-project/create-edit-list-project/create-edit-list-project.component";
import { ChecklistComponent } from "./modules/checklist-management/checklist/checklist.component";
import { CreateChecklistItemComponent } from "./modules/checklist-management/checklist/create-checklist-item/create-checklist-item.component";
import { CreateEditChecklistTitleComponent } from "./modules/checklist-management/checklist-title/create-edit-checklist-title/create-edit-checklist-title.component";
import { ChecklistTitleComponent } from "./modules/checklist-management/checklist-title/checklist-title.component";
import { FilterComponent } from "./../shared/filter/filter.component";
import { NgModule  } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClient, HttpClientJsonpModule } from "@angular/common/http";
import { HttpClientModule } from "@angular/common/http";
import { ModalModule } from "ngx-bootstrap/modal";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { CollapseModule } from "ngx-bootstrap/collapse";
import { TabsModule } from "ngx-bootstrap/tabs";
import { NgxPaginationModule } from "ngx-pagination";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { ServiceProxyModule } from "@shared/service-proxies/service-proxy.module";
import { SharedModule } from "@shared/shared.module";
import { HomeComponent } from "@app/home/home.component";
import { AboutComponent } from "@app/about/about.component";
import { BrowserModule } from "@angular/platform-browser";
// tenants
import { TenantsComponent } from "@app/tenants/tenants.component";
import { CreateTenantDialogComponent } from "./tenants/create-tenant/create-tenant-dialog.component";
import { EditTenantDialogComponent } from "./tenants/edit-tenant/edit-tenant-dialog.component";
// roles
import { RolesComponent } from "@app/roles/roles.component";
import { CreateRoleDialogComponent } from "./roles/create-role/create-role-dialog.component";
import { EditRoleDialogComponent } from "./roles/edit-role/edit-role-dialog.component";
// users
import { UsersComponent } from "@app/users/users.component";
import { CreateUserDialogComponent } from "@app/users/create-user/create-user-dialog.component";
import { EditUserDialogComponent } from "@app/users/edit-user/edit-user-dialog.component";
import { ChangePasswordComponent } from "./users/change-password/change-password.component";
import { ResetPasswordDialogComponent } from "./users/reset-password/reset-password.component";
// layout
import { HeaderComponent } from "./layout/header.component";
import { HeaderLeftNavbarComponent } from "./layout/header-left-navbar.component";
import { HeaderLanguageMenuComponent } from "./layout/header-language-menu.component";
import { HeaderUserMenuComponent } from "./layout/header-user-menu.component";
import { FooterComponent } from "./layout/footer.component";
import { SidebarComponent } from "./layout/sidebar.component";
import { SidebarLogoComponent } from "./layout/sidebar-logo.component";
import { SidebarUserPanelComponent } from "./layout/sidebar-user-panel.component";
import { SidebarMenuComponent } from "./layout/sidebar-menu.component";
import { TranslateLoader, TranslateModule } from "@ngx-translate/core";
import {
  GoogleLoginProvider,
  SocialAuthServiceConfig,
  SocialLoginModule,
} from "angularx-social-login";
import { CriteriaManagementComponent } from "./modules/audit-management/criteria-management/criteria-management.component";
import { TailoringManagementComponent } from "./modules/audit-management/criteria-management/tailoring-management/tailoring-management/tailoring-management.component";
import { SelectProjectTailoringComponent } from "./modules/audit-management/criteria-management/tailoring-management/tailoring-management/select-project-tailoring/select-project-tailoring/select-project-tailoring.component";
import { CreateEditCriteriaAuditComponent } from "./modules/audit-management/criteria-management/create-edit-criteria-audit/create-edit-criteria-audit.component";
import { ImportTailoringComponent } from './modules/audit-management/criteria-management/tailoring-management/tailoring-management/import-tailoring/import-tailoring/import-tailoring.component';
import { ProjectCriteriaAuditComponent } from "./modules/audit-management/criteria-management/project-criteria-audit/project-criteria-audit.component";
import { AuditResultComponent } from './modules/audit-management/result-management/audit-result/audit-result/audit-result.component';
import { ImportAuditResultComponent } from './modules/audit-management/result-management/audit-result/audit-result/import-audit-result/import-audit-result/import-audit-result.component';
import { DownloadTemplateComponent } from './modules/audit-management/result-management/audit-result/audit-result/download-template/download-template/download-template.component';
import { TaloringProjectComponent } from './modules/audit-management/criteria-management/taloring-project/taloring-project.component';
import { ViewGuilineComponent } from './modules/audit-management/criteria-management/view-guiline/view-guiline.component';
import { AuditResultDetailComponent } from './modules/audit-management/result-management/audit-result/audit-result/audit-result-detail/audit-result-detail.component';
import { AddResultCriteriaComponent } from './modules/audit-management/result-management/audit-result/audit-result/audit-result-detail/add-result-criteria/add-result-criteria.component';
import { EditAuditResultDetailComponent } from './modules/audit-management/result-management/audit-result/audit-result/audit-result-detail/edit-audit-result-detail/edit-audit-result-detail.component';
import { ViewAuditResultDetailComponent } from './modules/audit-management/result-management/audit-result/audit-result/audit-result-detail/view-audit-result-detail/view-audit-result-detail.component';
import { PopupNoteComponent } from './modules/audit-management/criteria-management/tailoring-management/popup-note/popup-note.component';
import { ViewQaNoteComponent } from './modules/audit-management/result-management/audit-result/audit-result/view-note/view-qa-note/view-qa-note.component';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TimesheetComponent } from './modules/timesheet/timesheet.component';
import { SaoDoComponent } from './modules/saodo-management/sao-do/sao-do.component';
import { CreateEditTimesheetComponent } from './modules/timesheet/create-edit-timesheet/create-edit-timesheet.component';
import { ImportFileTimesheetDetailComponent } from './modules/timesheet/timesheet-detail/import-file-timesheet-detail/import-file-timesheet-detail.component';
import { CreateEditTimesheetDetailComponent } from './modules/timesheet/timesheet-detail/create-edit-timesheet-detail/create-edit-timesheet-detail.component';
import { DeliveryComponent } from './modules/delivery-management/delivery/delivery.component';
import { ListProjectGeneralComponent } from './modules/pm-management/list-project/list-project-detail/list-project-general/list-project-general.component';
import { ProjectGeneralComponent } from './modules/pm-management/project-detail/project-general/project-general.component';
import { CreateEditSaodoComponent } from './modules/saodo-management/sao-do/create-edit-saodo/create-edit-saodo.component';
import { SaoDoDetailComponent } from './modules/saodo-management/sao-do/sao-do-detail/sao-do-detail.component';
import { InvoiceComponent } from './modules/timesheet/invoice/invoice.component';
import { CreateEditInvoiceComponent } from './modules/timesheet/invoice/create-edit-invoice/create-edit-invoice.component';
import { CreateEditSaoDoProjectComponent } from './modules/saodo-management/sao-do/sao-do-detail/create-edit-sao-do-project/create-edit-sao-do-project.component';
import { SaoDoProjectDetailComponent } from './modules/saodo-management/sao-do/sao-do-detail/sao-do-project-detail/sao-do-project-detail.component';
import { WeeklyReportTabComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab.component';
import { WeeklyReportTabDetailComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/weekly-report-tab-detail.component';
import { CreateEditProjectChecklistComponent } from './modules/pm-management/list-project/list-project-detail/project-checklist/create-edit-project-checklist/create-edit-project-checklist.component';
import { RequestResourceTabComponent } from './modules/delivery-management/delivery/request-resource-tab/request-resource-tab.component';
import { ResourceRequestDetailComponent } from './modules/delivery-management/delivery/request-resource-tab/resource-request-detail/resource-request-detail.component';
import { AddUserToRequestComponent } from './modules/delivery-management/delivery/request-resource-tab/resource-request-detail/add-user-to-request/add-user-to-request.component';
import { CreateUpdateResourceRequestComponent } from './modules/delivery-management/delivery/request-resource-tab/create-update-resource-request/create-update-resource-request.component';
import { ApproveDialogComponent } from './modules/pm-management/list-project/list-project-detail/weekly-report/approve-dialog/approve-dialog.component';
import { ProjectTimesheetComponent } from './modules/pm-management/list-project/list-project-detail/project-timesheet/project-timesheet.component';
import { AvailableResourceTabComponent } from './modules/delivery-management/delivery/available-resource-tab/available-resource-tab.component';
import { PlanUserComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-user/plan-user.component';
import { PlanResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-resource.component';
import { FutureResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/future-resource/future-resource.component';
import { EditFutureResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/future-resource/edit-future-resource/edit-future-resource.component';
import { ClientComponent } from './modules/admin/client/client.component';
import { CreateUpdateClientComponent } from './modules/admin/client/create-update-client/create-update-client.component';
import { CategoryCriteriaComponent } from './modules/checkpoint/category/category-criteria/category-criteria.component';
import { CreateEditCriteriaCategoryComponent } from './modules/checkpoint/category/category-criteria/create-edit-criteria-category/create-edit-criteria-category.component';
import { CriteriaComponent } from './modules/checkpoint/category/criteria/criteria.component';
import { CreateEditCriteriaComponent } from './modules/checkpoint/category/criteria/create-edit-criteria/create-edit-criteria.component';
import { EditReportComponent } from './modules/delivery-management/delivery/weekly-report-tab/edit-report/edit-report.component';
import { ReportInfoComponent } from './modules/delivery-management/delivery/weekly-report-tab/report-info/report-info.component';
import { SkillComponent } from './modules/admin/skill/skill.component';
import { CreateUpdateSkillComponent } from './modules/admin/skill/create-update-skill/create-update-skill.component';
import { AddReportNoteComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/add-report-note/add-report-note.component';
import { UploadAvatarComponent } from './users/upload-avatar/upload-avatar.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { ConfigurationComponent } from './modules/admin/configuration/configuration.component';
import { GetTimesheetWorkingComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/get-timesheet-working/get-timesheet-working.component';
import { CreateInvoiceComponent } from './modules/timesheet/timesheet-detail/create-invoice/create-invoice.component';
import { PhaseComponent } from './modules/checkpoint/phase/phase.component';
import { CreateEditPhaseComponent } from './modules/checkpoint/phase/create-edit-phase/create-edit-phase.component';
import { TagsComponent } from './modules/checkpoint/tags/tags.component';
import { CreateEditTagComponent } from './modules/checkpoint/tags/create-edit-tag/create-edit-tag.component';
import { SetUpReviewerComponent } from './modules/checkpoint/set-up-reviewer/set-up-reviewer.component';
import { CreateEditSetupReviewerComponent } from './modules/checkpoint/set-up-reviewer/create-edit-setup-reviewer/create-edit-setup-reviewer.component';
import { ResultReviewerComponent } from './modules/checkpoint/set-up-reviewer/result-reviewer/result-reviewer.component';
import { ReviewUserComponent } from './modules/checkpoint/review-user/review-user.component';
import { CreateEditReviewUserComponent } from './modules/checkpoint/review-user/create-edit-review-user/create-edit-review-user.component';
import { ReviewYourselfComponent } from './modules/checkpoint/review-yourself/review-yourself.component';
import { EditResultReviewerComponent } from './modules/checkpoint/set-up-reviewer/result-reviewer/edit-result-reviewer/edit-result-reviewer.component';
import { ProjectDescriptionComponent } from './modules/pm-management/list-project/list-project-detail/project-description/project-description.component';
import { ProjectDetailComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-user/project-detail/project-detail.component';
import { CurrencyComponent } from './modules/admin/currency/currency.component';
import { CreateEditCurrencyComponent } from './modules/admin/currency/create-edit-currency/create-edit-currency.component';
import { CdTimerModule } from 'angular-cd-timer';
import { ViewBillComponent } from './modules/timesheet/timesheet-detail/view-bill/view-bill.component';
import { AllResourceComponent } from './modules/delivery-management/delivery/available-resource-tab/all-resource/all-resource.component';
import { TrainingProjectsComponent } from './modules/pm-management/training-projects/training-projects.component';
import { ProductProjectsComponent } from './modules/pm-management/product-projects/product-projects.component';
import { CreateEditTrainingProjectComponent } from './modules/pm-management/training-projects/create-edit-training-project/create-edit-training-project.component';
import { TrainingProjectDetailComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-detail.component';
import { TrainingProjectGeneralComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-general/training-project-general.component';
import { TrainingProjectChecklistComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-checklist/training-project-checklist.component';
import { TrainingProjectDescriptionComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-description/training-project-description.component';
import { TrainingProjectTimesheetComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-timesheet/training-project-timesheet.component';
import { TrainingResourceManagementComponent } from './modules/pm-management/training-projects/training-project-detail/training-resource-management/training-resource-management.component';
import { TrainingWeeklyReportComponent } from './modules/pm-management/training-projects/training-project-detail/training-weekly-report/training-weekly-report.component';
import { TrainingApprovedDialogComponent } from './modules/pm-management/training-projects/training-project-detail/training-weekly-report/training-approved-dialog/training-approved-dialog.component';
import { TrainingMilestoneComponent } from './modules/pm-management/training-projects/training-project-detail/training-milestone/training-milestone.component';
import { CreateEditTrainingProjectChecklistComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-checklist/create-edit-training-project-checklist/create-edit-training-project-checklist.component';
import { CreateEditProductProjectComponent } from './modules/pm-management/product-projects/create-edit-product-project/create-edit-product-project.component';
import { ProductProjectDetailComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-detail.component';
import { ProductProjectGeneralComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-general/product-project-general.component';
import { ProductProjectMilestoneComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-milestone/product-project-milestone.component';
import { ProductProjectChecklistComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-checklist/product-project-checklist.component';
import { ProductProjectDescriptionComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-description/product-project-description.component';
import { ProductProjectTimesheetComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-timesheet/product-project-timesheet.component';
import { ProductResourceManagementComponent } from './modules/pm-management/product-projects/product-project-detail/product-resource-management/product-resource-management.component';
import { ProductWeeklyReportComponent } from './modules/pm-management/product-projects/product-project-detail/product-weekly-report/product-weekly-report.component';
import { ProductApprovedDialogComponent } from './modules/pm-management/product-projects/product-project-detail/product-weekly-report/product-approved-dialog/product-approved-dialog.component';
import { CreateEditProductProjectChecklistComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-checklist/create-edit-product-project-checklist/create-edit-product-project-checklist.component';
import { ProjectHistoryByUserComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-user/project-history-by-user/project-history-by-user.component';
import { AddNoteDialogComponent } from '@app/modules/delivery-management/delivery/available-resource-tab/plan-resource/add-note-dialog/add-note-dialog.component';
import { ProjectFileComponent } from './modules/pm-management/list-project/list-project-detail/project-file/project-file.component';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core'
import { DATE_FORMATS } from '@shared/AppEnums';
import { CollectTimesheetDialogComponent } from './modules/delivery-management/delivery/weekly-report-tab/collect-timesheet-dialog/collect-timesheet-dialog.component';
import { ListTimesheetByProjectCodeDialogComponent } from './modules/delivery-management/delivery/weekly-report-tab/list-timesheet-by-project-code-dialog/list-timesheet-by-project-code-dialog.component';
import { UpdateUserSkillDialogComponent } from './users/update-user-skill-dialog/update-user-skill-dialog.component';
import { VendorComponent } from './modules/delivery-management/delivery/available-resource-tab/vendor/vendor.component';
import { UpdateUserRoleComponent } from './users/update-user-role/update-user-role.component';
import { ReleaseUserDialogComponent } from './modules/pm-management/list-project/list-project-detail/resource-management/release-user-dialog/release-user-dialog.component';
import { ProjectBillComponent } from './modules/pm-management/list-project/list-project-detail/project-bill/project-bill.component';
import { ConfirmPopupComponent } from './modules/pm-management/list-project/list-project-detail/resource-management/confirm-popup/confirm-popup.component';
import { FormPlanUserComponent } from './modules/delivery-management/delivery/request-resource-tab/form-plan-user/form-plan-user.component';
import { FormSetDoneComponent } from './modules/delivery-management/delivery/request-resource-tab/form-set-done/form-set-done.component';
import { ConfirmPlanDialogComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-user/confirm-plan-dialog/confirm-plan-dialog.component';
import { AddUserToTempProjectDialogComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/add-user-to-temp-project-dialog/add-user-to-temp-project-dialog.component';
import { AddFutureResourceDialogComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/add-future-resource-dialog/add-future-resource-dialog.component';
import { EditMeetingNoteDialogComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/edit-meeting-note-dialog/edit-meeting-note-dialog.component';
import { TrainingProjectFileComponent } from './modules/pm-management/training-projects/training-project-detail/training-project-file/training-project-file.component';
import { ProductProjectFileComponent } from './modules/pm-management/product-projects/product-project-detail/product-project-file/product-project-file.component';
import { AddUserInRoleComponent } from './roles/edit-role/add-user-in-role/add-user-in-role.component';
import { EditTimesheetProjectDialogComponent } from './modules/timesheet/timesheet-detail/edit-timesheet-project-dialog/edit-timesheet-project-dialog/edit-timesheet-project-dialog.component';
import { BranchComponent } from './modules/admin/branch/branch.component';
import { CreateUpdateBranchComponent } from './modules/admin/branch/create-update-branch/create-update-branch.component';
import { TechnologyComponent } from './modules/admin/technology/technology.component';
import { CreateUpdateTechnologyComponent } from './modules/admin/technology/create-update-technology/create-update-technology.component';
import { EditNoteDialogComponent } from './modules/pm-management/list-project/list-project-detail/project-bill/add-note-dialog/edit-note-dialog.component';
import { FormSendRecruitmentComponent } from './modules/delivery-management/delivery/request-resource-tab/form-send-recruitment/form-send-recruitment.component';
import { PositionComponent } from './modules/admin/position/position.component';
import { CreateUpdatePositionComponent } from './modules/admin/position/create-update-position/create-update-position.component';
import { AddSubInvoiceDialogComponent } from './modules/pm-management/list-project/list-project-detail/project-bill/add-sub-invoice-dialog/add-sub-invoice-dialog.component';
import { InvoiceSettingDialogComponent } from './modules/pm-management/list-project/list-project-detail/project-bill/invoice-setting-dialog/invoice-setting-dialog.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { TrainingRequestTabComponent } from './modules/delivery-management/delivery/training-request-tab/training-request-tab.component';
import { CreateUpdateTrainingRequestComponent } from './modules/delivery-management/delivery/training-request-tab/create-update-training-request/create-update-training-request.component';
import { RetroReviewHistoryByUserComponent } from './modules/delivery-management/delivery/available-resource-tab/plan-resource/plan-user/retro-review-history-by-user/retro-review-history-by-user.component';
import { AuditLogComponent } from './modules/admin/audit-log/audit-log.component';
import { CriteriasComponent } from './modules/admin/criterias/criterias.component';
import { ExchangeRateComponent } from './modules/timesheet/timesheet-detail/exchange-rate/exchange-rate/exchange-rate.component';
import { ExportInvoiceComponent } from './modules/timesheet/timesheet-detail/export-invoice/export-invoice.component';
import { GuideLineDialogComponent } from './modules/pm-management/list-project/list-project-detail/weekly-report/guide-line-dialog/guide-line-dialog/guide-line-dialog.component';
import { ReportGuidelineDetailComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/report-guideline-detail/report-guideline-detail.component';
import { EditNoteResourceComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/edit-note-resource/edit-note-resource.component';
import { AddRiskDialogComponent } from './modules/pm-management/list-project/list-project-detail/weekly-report/add-risk-dialog/add-risk-dialog.component';
import { UpdateConfirmModalComponent } from './modules/delivery-management/delivery/weekly-report-tab/weekly-report-tab-detail/update-confirm-modal/update-confirm-modal.component';
import { ProjectDescriptionPopupComponent } from './modules/delivery-management/delivery/request-resource-tab/project-description-popup/project-description-popup.component';
import { AddEditIssuseComponent } from './modules/pm-management/list-project/list-project-detail/weekly-report/add-edit-issuse/add-edit-issuse.component';
import { ActiveTimesheetProjectComponent } from './modules/timesheet/timesheet-detail/active-timesheet-project/active-timesheet-project.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, "./assets/i18n/", ".json");
}
export const options: Partial<IConfig> | (() => Partial<IConfig>) = null;
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    // tenants
    TenantsComponent,
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    RolesComponent,
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    AddUserInRoleComponent,
    // users
    UsersComponent,
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ChangePasswordComponent,
    ResetPasswordDialogComponent,
    // layout
    HeaderComponent,
    HeaderLeftNavbarComponent,
    HeaderLanguageMenuComponent,
    HeaderUserMenuComponent,
    FooterComponent,
    SidebarComponent,
    SidebarLogoComponent,
    SidebarUserPanelComponent,
    SidebarMenuComponent,

    FilterComponent,

    TimesheetComponent,

    ChecklistTitleComponent,
    CreateEditChecklistTitleComponent,
    ChecklistComponent,
    CreateChecklistItemComponent,
    SaoDoComponent,
    ListProjectComponent,
    CreateEditListProjectComponent,
    CreateEditTimesheetComponent,
    TimesheetDetailComponent,
    ListProjectDetailComponent,
    ResourceManagementComponent,
    MilestoneComponent,
    WeeklyReportComponent,
    ProjectChecklistComponent,
    ImportFileTimesheetDetailComponent,
    CreateEditTimesheetDetailComponent,
    DeliveryComponent,
    ListProjectGeneralComponent,
    ProjectGeneralComponent,
    CreateEditSaodoComponent,
    SaoDoDetailComponent,
    InvoiceComponent,
    CreateEditInvoiceComponent,
    CreateEditSaoDoProjectComponent,
    SaoDoProjectDetailComponent,
    WeeklyReportTabComponent,
    WeeklyReportTabDetailComponent,
    CreateEditProjectChecklistComponent,
    RequestResourceTabComponent,
    ResourceRequestDetailComponent,
    AddUserToRequestComponent,
    CreateUpdateResourceRequestComponent,
    ApproveDialogComponent,
    ProjectTimesheetComponent,
    AvailableResourceTabComponent,
    PlanUserComponent,
    PlanResourceComponent,
    FutureResourceComponent,
    EditFutureResourceComponent,
    ClientComponent,
    CreateUpdateClientComponent,
    CategoryCriteriaComponent,
    CreateEditCriteriaCategoryComponent,
    CriteriaComponent,
    CreateEditCriteriaComponent,
    EditReportComponent,
    ReportInfoComponent,
    SkillComponent,
    CreateUpdateSkillComponent,
    AddReportNoteComponent,
    UploadAvatarComponent,
    ConfigurationComponent,
    GetTimesheetWorkingComponent,
    CreateInvoiceComponent,
    PhaseComponent,
    CreateEditPhaseComponent,
    TagsComponent,
    CreateEditTagComponent,
    SetUpReviewerComponent,
    CreateEditSetupReviewerComponent,
    ResultReviewerComponent,
    ReviewUserComponent,
    CreateEditReviewUserComponent,
    ReviewYourselfComponent,
    EditResultReviewerComponent,
    ProjectDescriptionComponent,
    ProjectDetailComponent,
    CurrencyComponent,
    CreateEditCurrencyComponent,
    ViewBillComponent,
    AllResourceComponent,
    TrainingProjectsComponent,
    ProductProjectsComponent,
    CreateEditTrainingProjectComponent,
    TrainingProjectDetailComponent,
    TrainingProjectGeneralComponent,
    TrainingProjectChecklistComponent,
    TrainingProjectDescriptionComponent,
    TrainingProjectTimesheetComponent,
    TrainingResourceManagementComponent,
    TrainingWeeklyReportComponent,
    TrainingApprovedDialogComponent,
    TrainingMilestoneComponent,
    CreateEditTrainingProjectChecklistComponent,
    CreateEditProductProjectComponent,
    ProductProjectDetailComponent,
    ProductProjectGeneralComponent,
    ProductProjectMilestoneComponent,
    ProductProjectChecklistComponent,
    ProductProjectDescriptionComponent,
    ProductProjectTimesheetComponent,
    ProductResourceManagementComponent,
    ProductWeeklyReportComponent,
    ProductApprovedDialogComponent,
    CreateEditProductProjectChecklistComponent,
    ProjectHistoryByUserComponent,
    AddNoteDialogComponent,
    ProjectFileComponent,
    CollectTimesheetDialogComponent,
    ListTimesheetByProjectCodeDialogComponent,
    UpdateUserSkillDialogComponent,
    VendorComponent,
    UpdateUserRoleComponent,
    ReleaseUserDialogComponent,
    ProjectBillComponent,
    ConfirmPopupComponent,
    FormPlanUserComponent,
    FormSetDoneComponent,
    ConfirmPlanDialogComponent,
    AddUserToTempProjectDialogComponent,
    AddFutureResourceDialogComponent,
    EditMeetingNoteDialogComponent,
    TrainingProjectFileComponent,
    ProductProjectFileComponent,
    EditTimesheetProjectDialogComponent,
    BranchComponent,
    CreateUpdateBranchComponent,
    BranchComponent,
    CreateUpdateBranchComponent,
    TechnologyComponent,
    CreateUpdateTechnologyComponent,
    EditNoteDialogComponent,
    FormSendRecruitmentComponent,
    PositionComponent,
    CreateUpdatePositionComponent,
    AddSubInvoiceDialogComponent,
    InvoiceSettingDialogComponent,
    TrainingRequestTabComponent,
    CreateUpdateTrainingRequestComponent,
    RetroReviewHistoryByUserComponent,
    AuditLogComponent,
    RetroReviewHistoryByUserComponent,
    CriteriasComponent,
    ExportInvoiceComponent,
    ExchangeRateComponent,
    GuideLineDialogComponent,
    CriteriaManagementComponent,
    CreateEditCriteriaAuditComponent,
    TailoringManagementComponent,
    SelectProjectTailoringComponent,
    ImportTailoringComponent,
    ProjectCriteriaAuditComponent,
    AuditResultComponent,
    ImportAuditResultComponent,
    DownloadTemplateComponent,
    TaloringProjectComponent,
    ViewGuilineComponent,
    AuditResultDetailComponent,
    AddResultCriteriaComponent,
    EditAuditResultDetailComponent,
    ViewAuditResultDetailComponent,
    PopupNoteComponent,
    ViewQaNoteComponent,
    GuideLineDialogComponent,
    ReportGuidelineDetailComponent,
    GuideLineDialogComponent,
    EditNoteResourceComponent,
    AddRiskDialogComponent,
    UpdateConfirmModalComponent,
    ProjectDescriptionPopupComponent,
    AddEditIssuseComponent,
    ActiveTimesheetProjectComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule.forChild(),
    BsDropdownModule,
    CollapseModule,
    TabsModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
    SocialLoginModule,
    NgxMaskModule.forRoot(),
    Ng2SearchPipeModule,
    ImageCropperModule,
    CdTimerModule,
    NgxMatSelectSearchModule,

    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
  ],
  exports: [TranslateModule],
  providers: [
    {
      provide: "SocialAuthServiceConfig",
      useValue: {
        autoLogin: true,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              AppConsts.configURI.googleClientAppId
            ),
          },
        ],
      } as SocialAuthServiceConfig,
    },
    {
      provide: MAT_DATE_LOCALE,
      useValue: "en-GB",
    },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },
    { provide: MAT_DATE_FORMATS, useValue: DATE_FORMATS },
  ],
  entryComponents: [
    // tenants
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    AddUserInRoleComponent,
    // users
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ResetPasswordDialogComponent,
  ],
})
export class AppModule {}
