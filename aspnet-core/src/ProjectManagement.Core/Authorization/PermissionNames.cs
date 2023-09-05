using Abp.MultiTenancy;
using System.Collections.Generic;
using static ProjectManagement.Authorization.Roles.StaticRoleNames;

namespace ProjectManagement.Authorization
{
    public static class PermissionNames
    {
        #region Admin

        public const string Admin = "Admin";
        #endregion Admin

        #region Tenants

        public const string Admin_Tenants = "Admin.Tenants";
        public const string Admin_Tenants_View = "Admin.Tenants.View";
        public const string Admin_Tenants_Create = "Admin.Tenants.Create";
        public const string Admin_Tenants_Edit = "Admin.Tenants.Edit";
        public const string Admin_Tenants_Delete = "Admin.Tenants.Delete";
        #endregion Tenants

        #region Clients

        public const string Admin_Clients = "Admin.Clients";
        public const string Admin_Clients_View = "Admin.Clients.View";
        public const string Admin_Clients_Create = "Admin.Clients.Create";
        public const string Admin_Clients_Edit = "Admin.Clients.Edit";
        public const string Admin_Clients_Delete = "Admin.Clients.Delete";
        #endregion Clients

        #region Branchs

        public const string Admin_Branchs = "Admin.Branchs";
        public const string Admin_Branchs_View = "Admin.Branchs.View";
        public const string Admin_Branchs_Create = "Admin.Branchs.Create";
        public const string Admin_Branchs_Edit = "Admin.Branchs.Edit";
        public const string Admin_Branchs_Delete = "Admin.Branchs.Delete";
        #endregion Branchs

        #region Positions

        public const string Admin_Positions = "Admin.Positions";
        public const string Admin_Positions_View = "Admin.Positions.View";
        public const string Admin_Positions_Create = "Admin.Positions.Create";
        public const string Admin_Positions_Edit = "Admin.Positions.Edit";
        public const string Admin_Positions_Delete = "Admin.Positions.Delete";
        #endregion Positions

        #region Technologies

        public const string Admin_Technologies = "Admin.Technologies";
        public const string Admin_Technologies_View = "Admin.Technologies.View";
        public const string Admin_Technologies_Create = "Admin.Technologies.Create";
        public const string Admin_Technologies_Edit = "Admin.Technologies.Edit";
        public const string Admin_Technologies_Delete = "Admin.Technologies.Delete";
        #endregion Technologies

        #region Configuration

        public const string Admin_Configuartions = "Admin.Configuartions";
        public const string Admin_Configuartions_Edit = "Admin.Configuartions.Edit";
        public const string Admin_Configuartions_ViewKomuSetting = "Admin.Configuartions.ViewKomuSetting";
        public const string Admin_Configuartions_ViewProjectSetting = "Admin.Configuartions.ViewProjectSetting";
        public const string Admin_Configuartions_ViewHrmSetting = "Admin.Configuartions.ViewHrmSetting";
        public const string Admin_Configuartions_ViewTimesheetSetting = "Admin.Configuartions.ViewTimesheetSetting";
        public const string Admin_Configuartions_ViewFinanceSetting = "Admin.Configuartions.ViewFinanceSetting";
        public const string Admin_Configuartions_ViewTalentSetting = "Admin.Configuartions.ViewTalentSetting";
        public const string Admin_Configuartions_ViewSendReportSetting = "Admin.Configuartions.ViewSendReportSetting";
        public const string Admin_Configuartions_ViewGoogleClientAppSetting = "Admin.Configuartions.ViewGoogleClientAppSetting";
        public const string Admin_Configuartions_ViewDefaultWorkingHourPerDaySetting = "Admin.Configuartions.ViewDefaultWorkingHourPerDaySetting";
        public const string Admin_Configuartions_ViewMaxCountHistoryOfRetroAndReviewPoint = "Admin.Configuartions.ViewMaxCountHistoryOfRetroAndReviewPoint";
        public const string Admin_Configuartions_ViewAuditScoreSetting = "Admin.Configuartions.ViewAuditScoreSetting";
        public const string Admin_Configuartions_WeeklyReportTime_Edit = "Admin.Configuartions.WeeklyReportTimeEdit";
        public const string Admin_Configurations_ViewGuideLineSetting = "Admin.Configurations.ViewGuideLineSetting";
        public const string Admin_Configurations_ViewInformPmSetting = "Admin.Configurations.ViewInformPmSetting";

        #endregion Configuration

        #region Skill

        public const string Admin_Skills = "Admin.Skills";
        public const string Admin_Skills_View = "Admin.Skills.View";
        public const string Admin_Skills_Create = "Admin.Skills.Create";
        public const string Admin_Skills_Edit = "Admin.Skills.Edit";
        public const string Admin_Skills_Delete = "Admin.Skills.Delete";
        #endregion Skill

        #region Currencies

        public const string Admin_Currencies = "Admin.Currencies";
        public const string Admin_Currencies_View = "Admin.Currencies.View";
        public const string Admin_Currencies_Create = "Admin.Currencies.Create";
        public const string Admin_Currencies_Edit = "Admin.Currencies.Edit";
        public const string Admin_Currencies_Delete = "Admin.Currencies.Delete";
        #endregion Currencies

        #region User

        public const string Admin_Users = "Admin.Users";
        public const string Admin_Users_View = "Admin.Users.View";
        public const string Admin_Users_Create = "Admin.Users.Create";
        public const string Admin_Users_SyncDataFromHrm = "Admin.Users.SyncDataFromHrm";
        public const string Admin_Users_ViewProjectHistory = "Admin.Users.ViewProjectHistory";
        public const string Admin_Users_Edit = "Admin.Users.Edit";
        public const string Admin_Users_UpdateSkill = "Admin.Users.UpdateSkill";
        public const string Admin_Users_UpdateRole = "Admin.Users.UpdateRole";
        public const string Admin_Users_ActiveAndDeactive = "Admin.Users.ActiveAndDeactive";
        public const string Admin_Users_UploadAvatar = "Admin.Users.UploadAvatar";
        public const string Admin_Users_ResetPassword = "Admin.Users.ResetPassword";
        public const string Admin_Users_DeleteFakeUser = "Admin.Users.DeleteFakeUser";
        #endregion User

        #region Role

        public const string Admin_Roles = "Admin.Roles";
        public const string Admin_Roles_View = "Admin.Roles.View";
        public const string Admin_Roles_Create = "Admin.Roles.Create";
        public const string Admin_Roles_Edit = "Admin.Roles.Edit";
        public const string Admin_Roles_Delete = "Admin.Roles.Delete";

        #endregion Role

        #region AuditLog

        public const string Admin_AuditLogs = "Admin.AuditLogs";
        public const string Admin_AuditLogs_View = "Admin.AuditLogs.View";

        #endregion AuditLog

        #region Project

        #region Projects > Outsourcing Project

        public const string Projects = "Projects";
        public const string Projects_OutsourcingProjects = "Projects.OutsourcingProjects";
        public const string Projects_OutsourcingProjects_ViewAllProject = "Projects.OutsourcingProjects.ViewAllProject";
        public const string Projects_OutsourcingProjects_ViewMyProjectOnly = "Projects.OutsourcingProjects.ViewMyProjectOnly";
        public const string Projects_OutsourcingProjects_ViewBillInfo = "Projects.OutsourcingProjects.ViewBillInfo";
        public const string Projects_OutsourcingProjects_ViewBillAccount = "Projects.OutsourcingProjects.ViewBillAccount";
        public const string Projects_OutsourcingProjects_Create = "Projects.OutsourcingProjects.Create";
        public const string Projects_OutsourcingProjects_Edit = "Projects.OutsourcingProjects.Edit";
        public const string Projects_OutsourcingProjects_Delete = "Projects.OutsourcingProjects.Delete";
        public const string Projects_OutsourcingProjects_Close = "Projects.OutsourcingProjects.Close";
        public const string Projects_OutsourcingProjects_ProjectDetail = "Projects.OutsourcingProjects.ProjectDetail";
        public const string Projects_OutsourcingProjects_CheckProjectInvoiceSetting = "Projects.OutsourcingProjects.CheckProjectInvoiceSetting";
        public const string Projects_OutsourcingProjects_ViewRequireWeeklyReport = "Projects.OutsourcingProjects.ViewRequireWeeklyReport";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabGeneral = "Projects.OutsourcingProjects.ProjectDetail.TabGeneral";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabGeneral_View = "Projects.OutsourcingProjects.ProjectDetail.TabGeneral.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabGeneral_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabGeneral.Edit";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource = "Projects.OutsourcingProjects.ProjectDetail.CurrentResource";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.ViewHistory";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.AddNewResourceFromPool";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.AddNewResourceFromOtherProject";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.Release";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.CurrentResource.UpdateUserSkill";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.CreateNewPlan";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmPickEmployeeFromPoolToOther";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmOut";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.CancelPlan";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.PlannedResource.UpdateUserSkill";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.CreateNewRequest";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.PlanNewResourceForRequest";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.SetDone";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.CancelRequest";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.Delete";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment = "Projects.OutsourcingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.SendRecruitment";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateNote = "Projects.OutsourcingProjects.ProjectDetail.UpdateNote";
        //public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth = "Projects.OutsourcingProjects.ProjectDetail.UpdateProjectHealth";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport = "Projects.OutsourcingProjects.ProjectDetail.SendWeeklyReport";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.AddNewIssue";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.Delete";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.SetDone";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectRisk";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectRisk.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_AddNewRisk = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectRisk.AddNewRisk";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectRisk.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Delete = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectRisk.Delete";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_SetDone = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PMProjectRisk.SetDone";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.CurrentResource";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.CurrentResource.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.CurrentResource.Release";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Update_Note = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.CurrentResource.Update.Note";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.CreateNewPlan";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmPickEmployeeFromPoolToProject";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmOut";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.CancelPlan";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.ChangedResource";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.ChangedResource.View";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.ChangeStatus";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.Edit";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.Create";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.Delete";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_View = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.InvoiceSetting_View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.InvoiceSetting_Edit";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.Rate_View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabBillInfo.Note_Edit";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabTimesheet = "Projects.OutsourcingProjects.ProjectDetail.TabTimesheet";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription = "Projects.OutsourcingProjects.ProjectDetail.TabProjectDescription";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_View = "Projects.OutsourcingProjects.ProjectDetail.TabProjectDescription.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_Edit = "Projects.OutsourcingProjects.ProjectDetail.TabProjectDescription.Edit";

        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectFile = "Projects.OutsourcingProjects.ProjectDetail.TabProjectFile";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_View = "Projects.OutsourcingProjects.ProjectDetail.TabProjectFile.View";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_UploadFile = "Projects.OutsourcingProjects.ProjectDetail.TabProjectFile.UploadFile";
        public const string Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_DeleteFile = "Projects.OutsourcingProjects.ProjectDetail.TabProjectFile.DeleteFile";
        #endregion Projects > Outsourcing Project

        #region Projects > Product Project

        public const string Projects_ProductProjects = "Projects.ProductProjects";
        public const string Projects_ProductProjects_ViewAllProject = "Projects.ProductProjects.ViewAllProject";
        public const string Projects_ProductProjects_ViewMyProjectOnly = "Projects.ProductProjects.ViewMyProjectOnly";
        public const string Projects_ProductProjects_Create = "Projects.ProductProjects.Create";
        public const string Projects_ProductProjects_Edit = "Projects.ProductProjects.Edit";
        public const string Projects_ProductProjects_Delete = "Projects.ProductProjects.Delete";
        public const string Projects_ProductProjects_Close = "Projects.ProductProjects.Close";
        public const string Projects_ProductProjects_ProjectDetail = "Projects.ProductProjects.ProjectDetail";
        public const string Projects_ProductProjects_ViewRequireWeeklyReport = "Projects.ProductProjects.ViewRequireWeeklyReport";

        public const string Projects_ProductProjects_ProjectDetail_TabGeneral = "Projects.ProductProjects.ProjectDetail.TabGeneral";
        public const string Projects_ProductProjects_ProjectDetail_TabGeneral_View = "Projects.ProductProjects.ProjectDetail.TabGeneral.View";
        public const string Projects_ProductProjects_ProjectDetail_TabGeneral_Edit = "Projects.ProductProjects.ProjectDetail.TabGeneral.Edit";

        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement = "Projects.ProductProjects.ProjectDetail.TabResourceManagement";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource = "Projects.ProductProjects.ProjectDetail.CurrentResource";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_View = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.View";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.ViewHistory";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.AddNewResourceFromPool";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.AddNewResourceFromOtherProject";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.Edit";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.Release";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.CurrentResource.UpdateUserSkill";

        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_View = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.View";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.CreateNewPlan";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmPickEmployeeFromPoolToOther";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmOut";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.CancelPlan";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.Edit";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.PlannedResource.UpdateUserSkill";

        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.View";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.CreateNewRequest";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.PlanNewResourceForRequest";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.SetDone";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.CancelRequest";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.Edit";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.Delete";
        public const string Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment = "Projects.ProductProjects.ProjectDetail.TabResourceManagement.ResourceRequest.SendRecruitment";

        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.View";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote = "Projects.ProductProjects.ProjectDetail.UpdateNote";
        //public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth = "Projects.ProductProjects.ProjectDetail.UpdateProjectHealth";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport = "Projects.ProductProjects.ProjectDetail.SendWeeklyReport";

        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.View";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.AddNewIssue";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.Edit";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.Delete";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.SetDone";

        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.CurrentResource";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.CurrentResource.View";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.CurrentResource.Release";

        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.View";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.CreateNewPlan";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.Edit";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmPickEmployeeFromPoolToProject";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmOut";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.PlannedResource.CancelPlan";

        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.ChangedResource";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.ChangedResource.View";

        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.View";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.ChangeStatus";
        public const string Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit = "Projects.ProductProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.Edit";

        public const string Projects_ProductProjects_ProjectDetail_TabBillInfo = "Projects.ProductProjects.ProjectDetail.TabBillInfo";
        public const string Projects_ProductProjects_ProjectDetail_TabBillInfo_View = "Projects.ProductProjects.ProjectDetail.TabBillInfo.View";
        public const string Projects_ProductProjects_ProjectDetail_TabBillInfo_Create = "Projects.ProductProjects.ProjectDetail.TabBillInfo.Create";
        public const string Projects_ProductProjects_ProjectDetail_TabBillInfo_Edit = "Projects.ProductProjects.ProjectDetail.TabBillInfo.Edit";
        public const string Projects_ProductProjects_ProjectDetail_TabBillInfo_Delete = "Projects.ProductProjects.ProjectDetail.TabBillInfo.Delete";

        public const string Projects_ProductProjects_ProjectDetail_TabTimesheet = "Projects.ProductProjects.ProjectDetail.TabTimesheet";

        public const string Projects_ProductProjects_ProjectDetail_TabProjectDescription = "Projects.ProductProjects.ProjectDetail.TabProjectDescription";
        public const string Projects_ProductProjects_ProjectDetail_TabProjectDescription_View = "Projects.ProductProjects.ProjectDetail.TabProjectDescription.View";
        public const string Projects_ProductProjects_ProjectDetail_TabProjectDescription_Edit = "Projects.ProductProjects.ProjectDetail.TabProjectDescription.Edit";

        public const string Projects_ProductProjects_ProjectDetail_TabProjectFile = "Projects.ProductProjects.ProjectDetail.TabProjectFile";
        public const string Projects_ProductProjects_ProjectDetail_TabProjectFile_View = "Projects.ProductProjects.ProjectDetail.TabProjectFile.View";
        public const string Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile = "Projects.ProductProjects.ProjectDetail.TabProjectFile.UploadFile";
        public const string Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile = "Projects.ProductProjects.ProjectDetail.TabProjectFile.DeleteFile";
        #endregion Projects > Product Project

        #region Projects > Training Project

        public const string Projects_TrainingProjects = "Projects.TrainingProjects";
        public const string Projects_TrainingProjects_ViewAllProject = "Projects.TrainingProjects.ViewAllProject";
        public const string Projects_TrainingProjects_ViewMyProjectOnly = "Projects.TrainingProjects.ViewMyProjectOnly";
        public const string Projects_TrainingProjects_Create = "Projects.TrainingProjects.Create";
        public const string Projects_TrainingProjects_Edit = "Projects.TrainingProjects.Edit";
        public const string Projects_TrainingProjects_Delete = "Projects.TrainingProjects.Delete";
        public const string Projects_TrainingProjects_Close = "Projects.TrainingProjects.Close";
        public const string Projects_TrainingProjects_ProjectDetail = "Projects.TrainingProjects.ProjectDetail";
        public const string Projects_TrainingProjects_ViewRequireWeeklyReport = "Projects.TrainingProjects.ViewRequireWeeklyReport";

        public const string Projects_TrainingProjects_ProjectDetail_TabGeneral = "Projects.TrainingProjects.ProjectDetail.TabGeneral";
        public const string Projects_TrainingProjects_ProjectDetail_TabGeneral_View = "Projects.TrainingProjects.ProjectDetail.TabGeneral.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabGeneral_Edit = "Projects.TrainingProjects.ProjectDetail.TabGeneral.Edit";

        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource = "Projects.TrainingProjects.ProjectDetail.CurrentResource";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.ViewHistory";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.AddNewResourceFromPool";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.AddNewResourceFromOtherProject";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.Edit";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.Release";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.CurrentResource.UpdateUserSkill";

        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.CreateNewPlan";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmPickEmployeeFromPoolToOther";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.ConfirmOut";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.CancelPlan";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.Edit";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.PlannedResource.UpdateUserSkill";

        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.CreateNewRequest";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.PlanNewResourceForRequest";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.SetDone";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.CancelRequest";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.Edit";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.Delete";
        public const string Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment = "Projects.TrainingProjects.ProjectDetail.TabResourceManagement.ResourceRequest.SendRecruitment";

        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateNote = "Projects.TrainingProjects.ProjectDetail.UpdateNote";
        //public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth = "Projects.TrainingProjects.ProjectDetail.UpdateProjectHealth";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport = "Projects.TrainingProjects.ProjectDetail.SendWeeklyReport";

        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.AddNewIssue";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.Edit";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.Delete";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PMProjectIssue.SetDone";

        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.CurrentResource";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.CurrentResource.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.CurrentResource.Release";

        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.CreateNewPlan";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.Edit";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmPickEmployeeFromPoolToProject";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.ConfirmOut";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.PlannedResource.CancelPlan";

        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.ChangedResource";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.ChangedResource.View";

        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.ChangeStatus";
        public const string Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit = "Projects.TrainingProjects.ProjectDetail.TabWeeklyReport.ProjectHealthCriteria.Edit";

        public const string Projects_TrainingProjects_ProjectDetail_TabBillInfo = "Projects.TrainingProjects.ProjectDetail.TabBillInfo";
        public const string Projects_TrainingProjects_ProjectDetail_TabBillInfo_View = "Projects.TrainingProjects.ProjectDetail.TabBillInfo.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabBillInfo_Create = "Projects.TrainingProjects.ProjectDetail.TabBillInfo.Create";
        public const string Projects_TrainingProjects_ProjectDetail_TabBillInfo_Edit = "Projects.TrainingProjects.ProjectDetail.TabBillInfo.Edit";
        public const string Projects_TrainingProjects_ProjectDetail_TabBillInfo_Delete = "Projects.TrainingProjects.ProjectDetail.TabBillInfo.Delete";

        public const string Projects_TrainingProjects_ProjectDetail_TabTimesheet = "Projects.TrainingProjects.ProjectDetail.TabTimesheet";

        public const string Projects_TrainingProjects_ProjectDetail_TabProjectDescription = "Projects.TrainingProjects.ProjectDetail.TabProjectDescription";
        public const string Projects_TrainingProjects_ProjectDetail_TabProjectDescription_View = "Projects.TrainingProjects.ProjectDetail.TabProjectDescription.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabProjectDescription_Edit = "Projects.TrainingProjects.ProjectDetail.TabProjectDescription.Edit";

        public const string Projects_TrainingProjects_ProjectDetail_TabProjectFile = "Projects.TrainingProjects.ProjectDetail.TabProjectFile";
        public const string Projects_TrainingProjects_ProjectDetail_TabProjectFile_View = "Projects.TrainingProjects.ProjectDetail.TabProjectFile.View";
        public const string Projects_TrainingProjects_ProjectDetail_TabProjectFile_UploadFile = "Projects.TrainingProjects.ProjectDetail.TabProjectFile.UploadFile";
        public const string Projects_TrainingProjects_ProjectDetail_TabProjectFile_DeleteFile = "Projects.TrainingProjects.ProjectDetail.TabProjectFile.DeleteFile";

        #endregion Projects > Training Project

        #endregion Project

        #region Weekly report


        public const string WeeklyReport = "WeeklyReport";
        public const string WeeklyReport_View = "WeeklyReport.View";
        public const string WeeklyReport_CloseAndAddNew = "WeeklyReport.CloseAndAddNew";
        public const string WeeklyReport_CollectTimesheet = "WeeklyReport.CollectTimesheet";
        public const string WeeklyReport_Rename = "WeeklyReport.Rename";
        public const string WeeklyReport_ViewInfo = "WeeklyReport.ViewInfo";

        public const string WeeklyReport_InformPM = "WeeklyReport.InformPM";
        public const string WeeklyReport_InformPM_View = "WeeklyReport.InformPM.View";

        public const string WeeklyReport_ReportDetail = "WeeklyReport.ReportDetail";
        public const string WeeklyReport_ReportDetail_View = "WeeklyReport.ReportDetail.View";
        public const string WeeklyReport_ReportDetail_UpdateNote = "WeeklyReport.ReportDetail.UpdateNote";
        //public const string WeeklyReport_ReportDetail_UpdateProjectHealth = "WeeklyReport.ReportDetail.UpdateProjectHealth";

        public const string WeeklyReport_ReportDetail_ProjectHealthCriteria = "WeeklyReport.ReportDetail.ProjectHealthCriteria";
        public const string WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus = "WeeklyReport.ReportDetail.ProjectHealthCriteria.ChangeStatus";
        public const string WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit = "WeeklyReport.ReportDetail.ProjectHealthCriteria.Edit";
        public const string WeeklyReport_ReportDetail_ProjectHealthCriteria_View_Guideline = "WeeklyReport.ReportDetail.ProjectHealthCriteria.View.Guideline";
        public const string WeeklyReport_ReportDetail_ProjectHealthCriteria_Update_Guideline = "WeeklyReport.ReportDetail.ProjectHealthCriteria.Update.Guideline";

        public const string WeeklyReport_ReportDetail_PMIssue = "WeeklyReport.ReportDetail.PMIssue";
        public const string WeeklyReport_ReportDetail_PMIssue_View = "WeeklyReport.ReportDetail.PMIssue.View";
        public const string WeeklyReport_ReportDetail_PMIssue_AddMeetingNote = "WeeklyReport.ReportDetail.PMIssue.AddMeetingNote";
        public const string WeeklyReport_ReportDetail_PMIssue_SetDone = "WeeklyReport.ReportDetail.PMIssue.SetDone";

        public const string WeeklyReport_ReportDetail_PMRisk = "WeeklyReport.ReportDetail.PMRisk";
        public const string WeeklyReport_ReportDetail_PMRisk_View = "WeeklyReport.ReportDetail.PMRisk.View";
        public const string WeeklyReport_ReportDetail_PMRisk_SetDone = "WeeklyReport.ReportDetail.PMRisk.SetDone";

        public const string WeeklyReport_ReportDetail_CurrentResource = "WeeklyReport.ReportDetail.CurrentResource";
        public const string WeeklyReport_ReportDetail_CurrentResource_View = "WeeklyReport.ReportDetail.CurrentResource.View";
        public const string WeeklyReport_ReportDetail_CurrentResource_Release = "WeeklyReport.ReportDetail.CurrentResource.Release";
        public const string WeeklyReport_ReportDetail_CurrentResource_Update_Note = "WeeklyReport.ReportDetail.CurrentResource.Update.Note";

        public const string WeeklyReport_ReportDetail_PlannedResource = "WeeklyReport.ReportDetail.PlannedResource";
        public const string WeeklyReport_ReportDetail_PlannedResource_View = "WeeklyReport.ReportDetail.PlannedResource.View";
        public const string WeeklyReport_ReportDetail_PlannedResource_CreateNewPlan = "WeeklyReport.ReportDetail.PlannedResource.CreateNewPlan";
        public const string WeeklyReport_ReportDetail_PlannedResource_Edit = "WeeklyReport.ReportDetail.PlannedResource.Edit";
        public const string WeeklyReport_ReportDetail_PlannedResource_ConfirmPickEmployeeFromPoolToProject = "WeeklyReport.ReportDetail.PlannedResource.ConfirmPickEmployeeFromPoolToProject";
        public const string WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "WeeklyReport.ReportDetail.PlannedResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string WeeklyReport_ReportDetail_PlannedResource_ConfirmOut = "WeeklyReport.ReportDetail.PlannedResource.ConfirmOut";
        public const string WeeklyReport_ReportDetail_PlannedResource_CancelPlan = "WeeklyReport.ReportDetail.PlannedResource.CancelPlan";

        public const string WeeklyReport_ReportDetail_ChangedResource = "WeeklyReport.ReportDetail.ChangedResource";
        public const string WeeklyReport_ReportDetail_ChangedResource_View = "WeeklyReport.ReportDetail.ChangedResource.View";
        public const string WeeklyReport_ReportDetail_GuideLine_View = "WeeklyReport.ReportDetail.GuideLine.View";
        public const string WeeklyReport_ReportDetail_GuideLine_Update = "WeeklyReport.ReportDetail.GuideLine.Update";

        public const string WeeklyReport_ReportDetail_LastReviewDate_Check = "WeeklyReport.ReportDetail.LastReviewDate.Check";
        public const string WeeklyReport_ReportDetail_PrioritizeReview_Check = "WeeklyReport.ReportDetail.PrioritizeReview.Check";
        #endregion Weekly report

        #region Resource Request

        public const string ResourceRequest = "ResourceRequest";
        public const string ResourceRequest_View = "ResourceRequest.View";
        public const string ResourceRequest_CreateNewRequestForAllProject = "ResourceRequest.CreateNewRequest";
        public const string ResourceRequest_CreateNewRequestByPM = "ResourceRequest.CreateNewRequestByPM";
        public const string ResourceRequest_PlanNewResourceForRequest = "ResourceRequest.PlanNewResourceForRequest";
        //public const string ResourceRequest_UpdateResourceRequestPlan = "ResourceRequest.UpdateResourceRequestPlan";
        //public const string ResourceRequest_RemoveResouceRequestPlan = "ResourceRequest.RemoveResouceRequestPlan";
        public const string ResourceRequest_SetDone = "ResourceRequest.SetDone";
        public const string ResourceRequest_CancelAllRequest = "ResourceRequest.CancelAllRequest";
        public const string ResourceRequest_CancelMyRequest = "ResourceRequest.CancelMyRequest";
        public const string ResourceRequest_EditPmNote = "ResourceRequest.EditPmNote";
        public const string ResourceRequest_EditDmNote = "ResourceRequest.EditDmNote";
        public const string ResourceRequest_Edit = "ResourceRequest.Edit";
        public const string ResourceRequest_Delete = "ResourceRequest.Delete";
        public const string ResourceRequest_SendRecruitment = "ResourceRequest.SendRecruitment";
        #endregion Resource Request

        #region Training Request

        public const string TrainingRequest = "TrainingRequest";
        public const string TrainingRequest_View = "TrainingRequest.View";
        public const string TrainingRequest_CreateNewRequestForAllProject = "TrainingRequest.CreateNewRequest";
        public const string TrainingRequest_CreateNewRequestByPM = "TrainingRequest.CreateNewRequestByPM";
        public const string TrainingRequest_PlanNewTrainingForRequest = "TrainingRequest.PlanNewTrainingForRequest";
        //public const string TrainingRequest_UpdateTrainingRequestPlan = "TrainingRequest.UpdateTrainingRequestPlan";
        //public const string TrainingRequest_RemoveTrainingRequestPlan = "TrainingRequest.RemoveTrainingRequestPlan";
        public const string TrainingRequest_SetDone = "TrainingRequest.SetDone";
        public const string TrainingRequest_CancelAllRequest = "TrainingRequest.CancelAllRequest";
        public const string TrainingRequest_CancelMyRequest = "TrainingRequest.CancelMyRequest";
        public const string TrainingRequest_EditPmNote = "TrainingRequest.EditPmNote";
        public const string TrainingRequest_EditDmNote = "TrainingRequest.EditDmNote";
        public const string TrainingRequest_Edit = "TrainingRequest.Edit";
        public const string TrainingRequest_Delete = "TrainingRequest.Delete";
        public const string TrainingRequest_SendRecruitment = "TrainingRequest.SendRecruitment";
        #endregion Training Request

        #region Resource

        public const string Resource = "Resource";
        public const string Resource_TabPool = "Resource.TabPool";
        public const string Resource_TabPool_View = "Resource.TabPool.View";
        public const string Resource_TabPool_ViewHistory = "Resource.TabPool.ViewHistory";
        public const string Resource_TabPool_CreatePlan = "Resource.TabPool.CreatePlan";
        public const string Resource_TabPool_EditPlan = "Resource.TabPool.EditPlan";
        public const string Resource_TabPool_ConfirmPickEmployeeFromPoolToProject = "Resource.TabPool.ConfirmPickEmployeeFromPoolToProject";
        public const string Resource_TabPool_Release = "Resource.TabPool.Release";
        public const string Resource_TabPool_ConfirmOut = "Resource.TabPool.ConfirmOut";
        public const string Resource_TabPool_CancelMyPlan = "Resource.TabPool.CancelMyPlan";
        public const string Resource_TabPool_CancelAnyPlan = "Resource.TabPool.CancelAnyPlan";
        public const string Resource_TabPool_EditTempProject = "Resource.TabPool.EditTempProject";
        public const string Resource_TabPool_AddTempProject = "Resource.TabPool.AddTempProject";
        public const string Resource_TabPool_UpdateSkill = "Resource.TabPool.UpdateSkill";
        public const string Resource_TabPool_EditNote = "Resource.TabPool.EditNote";
        public const string Resource_TabPool_ProjectDetail = "Resource.TabPool.ProjectDetail";

        public const string Resource_TabAllResource = "Resource.TabAllResource";
        public const string Resource_TabAllResource_View = "Resource.TabAllResource.View";
        public const string Resource_TabAllResource_ViewHistory = "Resource.TabAllResource.ViewHistory";
        public const string Resource_TabAllResource_CreatePlan = "Resource.TabAllResource.CreatePlan";
        public const string Resource_TabAllResource_EditPlan = "Resource.TabAllResource.EditPlan";
        public const string Resource_TabAllResource_ConfirmPickEmployeeFromPoolToProject = "Resource.TabAllResource.ConfirmPickEmployeeFromPoolToProject";
        public const string Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Resource.TabAllResource.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Resource_TabAllResource_ConfirmOut = "Resource.TabAllResource.ConfirmOut";
        public const string Resource_TabAllResource_CancelMyPlan = "Resource.TabAllResource.CancelMyPlan";
        public const string Resource_TabAllResource_CancelAnyPlan = "Resource.TabAllResource.CancelAnyPlan";
        public const string Resource_TabAllResource_UpdateSkill = "Resource.TabAllResource.UpdateSkill";
        public const string Resource_TabAllResource_ProjectDetail = "Resource.TabAllResource.ProjectDetail";

        public const string Resource_TabVendor = "Resource.TabVendor";
        public const string Resource_TabVendor_View = "Resource.TabVendor.View";
        public const string Resource_TabVendor_ViewHistory = "Resource.TabVendor.ViewHistory";
        public const string Resource_TabVendor_CreatePlan = "Resource.TabVendor.CreatePlan";
        public const string Resource_TabVendor_EditPlan = "Resource.TabVendor.EditPlan";
        public const string Resource_TabVendor_ConfirmPickEmployeeFromPoolToProject = "Resource.TabVendor.ConfirmPickEmployeeFromPoolToProject";
        public const string Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther = "Resource.TabVendor.ConfirmMoveEmployeeWorkingOnAProjectToOther";
        public const string Resource_TabVendor_ConfirmOut = "Resource.TabVendor.ConfirmOut";
        public const string Resource_TabVendor_CancelMyPlan = "Resource.TabVendor.CancelMyPlan";
        public const string Resource_TabVendor_CancelAnyPlan = "Resource.TabVendor.CancelAnyPlan";
        public const string Resource_TabVendor_UpdateSkill = "Resource.TabVendor.UpdateSkill";
        public const string Resource_TabVendor_ProjectDetail = "Resource.TabVendor.ProjectDetail";

        public const string Resource_TabPlanningBillAcccount = "Resource.TabPlanning.BillAccount";
        public const string Resource_ViewUserLevel = "Resource.ViewUserLevel";
        #endregion Resource

        #region Timesheet

        public const string Timesheets = "Timesheets";
        public const string Timesheets_View = "Timesheets.View";
        public const string Timesheets_Create = "Timesheets.Create";
        public const string Timesheets_Edit = "Timesheets.Edit";
        public const string Timesheets_Delete = "Timesheets.Delete";
        public const string Timesheets_ForceDelete = "Timesheets.ForceDelete";
        public const string Timesheets_CloseAndActive = "Timesheets.CloseOrActive";

        public const string Timesheets_TimesheetDetail = "Timesheets.TimesheetDetail";
        public const string Timesheets_TimesheetDetail_View = "Timesheets.TimesheetDetail.View";
        public const string Timesheets_TimesheetDetail_ViewAll = "Timesheets.TimesheetDetail.ViewAll";
        public const string Timesheets_TimesheetDetail_ViewMyProjectOnly = "Timesheets.TimesheetDetail.ViewMyProjectOnly";

        public const string Timesheets_TimesheetDetail_ViewBillRate = "Timesheets.TimesheetDetail.ViewBillRate";
        public const string Timesheets_TimesheetDetail_AddProjectToTimesheet = "Timesheets.TimesheetDetail.AddProjectToTimesheet";
        public const string Timesheets_TimesheetDetail_UploadTimesheetFile = "Timesheets.TimesheetDetail.UploadTimesheetFile";
        public const string Timesheets_TimesheetDetail_ExportInvoiceAllProject = "Timesheets.TimesheetDetail.ExportInvoiceAllProject";
        public const string Timesheets_TimesheetDetail_ExportInvoice = "Timesheets.TimesheetDetail.ExportInvoice";
        public const string Timesheets_TimesheetDetail_ExportInvoiceForTax = "Timesheets.TimesheetDetail.ExportInvoiceForTax";
        public const string Timesheets_TimesheetDetail_UpdateNote = "Timesheets.TimesheetDetail.UpdateNote";
        public const string Timesheets_TimesheetDetail_Delete = "Timesheets.TimesheetDetail.Delete";
        public const string Timesheets_TimesheetDetail_EditInvoiceInfo = "Timesheets.TimesheetDetail.EditInvoiceInfo";
        public const string Timesheets_TimesheetDetail_SendInvoiceToFinfast = "Timesheets.TimesheetDetail.SendInvoiceToFinfast";

        public const string Timesheets_TimesheetDetail_UpdateBill = "Timesheets.TimesheetDetail.UpdateBill";
        public const string Timesheets_TimesheetDetail_UpdateBill_Edit = "Timesheets.TimesheetDetail.UpdateBill.Edit";
        public const string Timesheets_TimesheetDetail_UpdateBill_SetDone = "Timesheets.TimesheetDetail.UpdateBill.SetDone";

        public const string Timesheets_TimesheetDetail_UpdateTimsheet = "Timesheets.TimesheetDetail.UpdateTimsheet";
        public const string Timesheets_TimesheetDetail_RemoveAccount = "Timesheets.TimesheetDetail.RemoveAccount";

        #endregion Timesheet

        #region Criteria

        public const string Admin_Criteria = "Admin.Criteria";
        public const string Admin_Criteria_View = "Admin.Criteria.View";
        public const string Admin_Criteria_Create = "Admin.Criteria.Create";
        public const string Admin_Criteria_Edit = "Admin.Criteria.Edit";
        public const string Admin_Criteria_Delete = "Admin.Criteria.Delete";
        public const string Admin_Criteria_Active_DeActive = "Admin.Criteria.Active/DeActive";

        #endregion Criteria

        #region Audit

        public const string Audits = "Audits";
        public const string Audits_Criteria = "Audits.Criteria";
        public const string Audits_Criteria_Create = "Audits.Criteria.Create";
        public const string Audits_Criteria_Edit = "Audits.Criteria.Edit";
        public const string Audits_Criteria_Delete = "Audits.Criteria.Delete";
        public const string Audits_Criteria_Active = "Audits.Criteria.Active";
        public const string Audits_Criteria_DeActive = "Audits.Criteria.DeActive";
        public const string Audits_Criteria_ChangeApplicable = "Audits.Criteria.ChangeApplicable";

        public const string Audits_Tailoring = "Audits.Tailoring";
        public const string Audits_Tailoring_DownLoadTemplate = "Audits.Tailoring.DownLoadTemplate";
        public const string Audits_Tailoring_Import = "Audits.Tailoring.Import";
        public const string Audits_Tailoring_Create = "Audits.Tailoring.Create";
        public const string Audits_Tailoring_Delete = "Audits.Tailoring.Delete";
        public const string Audits_Tailoring_DownLoadTailoringTemplate = "Audits.Tailoring.DownLoadTailoringTemplate";
        public const string Audits_Tailoring_Detail = "Audits.Tailoring.Detail";
        public const string Audits_Tailoring_Detail_ViewNote = "Audits.Tailoring.Detail.ViewNote";
        public const string Audits_Tailoring_Detail_Update = "Audits.Tailoring.Detail.Update";
        public const string Audits_Tailoring_Detail_Detele = "Audits.Tailoring.Detail.Delete";
        public const string Audits_Tailoring_Update_Project = "Audits.Tailoring.Update.Project";
        public const string Audits_Tailoring_Update_Project_Tailoring = "Audits.Tailoring.Update.Project.Tailoring";

        public const string Audits_Results = "Audits.Results";
        public const string Audits_Results_DownLoad_Template = "Audits.Results.DownLoadTemplate";
        public const string Audits_Results_Import_Result = "Audits.Results.ImportResult";
        public const string Audits_Results_Edit = "Audits.Results.Edit";
        public const string Audits_Results_Delete = "Audits.Results.Delete";
        public const string Audits_Results_Detail = "Audits.Results.Detail";
        public const string Audits_Results_Detail_View = "Audits.Results.Detail.View";
        public const string Audits_Results_Detail_ViewNote = "Audits.Results.Detail.ViewNote";

        #endregion Audit
    }

    public class GrantPermissionRoles
    {
        public static Dictionary<string, List<string>> PermissionRoles = new Dictionary<string, List<string>>()
        {
            {
                Host.Admin,
                new List<string>()
                {
                    #region Tenant

                    PermissionNames.Admin,
                    PermissionNames.Admin_Tenants,
                    PermissionNames.Admin_Tenants_View,
                    PermissionNames.Admin_Tenants_Create,
                    PermissionNames.Admin_Tenants_Edit,
                    PermissionNames.Admin_Tenants_Delete,

                    #endregion Tenant

                    #region Clients

                    PermissionNames.Admin_Clients,
                    PermissionNames.Admin_Clients_View,
                    PermissionNames.Admin_Clients_Create,
                    PermissionNames.Admin_Clients_Edit,
                    PermissionNames.Admin_Clients_Delete,

                    #endregion Clients

                    #region Branchs

                    PermissionNames.Admin_Branchs,
                    PermissionNames.Admin_Branchs_View,
                    PermissionNames.Admin_Branchs_Create,
                    PermissionNames.Admin_Branchs_Edit,
                    PermissionNames.Admin_Branchs_Delete,

                    #endregion Branchs

                    #region Positions

                    PermissionNames.Admin_Positions,
                    PermissionNames.Admin_Positions_View,
                    PermissionNames.Admin_Positions_Create,
                    PermissionNames.Admin_Positions_Edit,
                    PermissionNames.Admin_Positions_Delete,

                    #endregion Positions

                    #region Technologies

                    PermissionNames.Admin_Technologies,
                    PermissionNames.Admin_Technologies_View,
                    PermissionNames.Admin_Technologies_Create,
                    PermissionNames.Admin_Technologies_Edit,
                    PermissionNames.Admin_Technologies_Delete,

                    #endregion Technologies

                    #region Configuration

                    PermissionNames.Admin_Configuartions,
                    PermissionNames.Admin_Configuartions_Edit,
                    PermissionNames.Admin_Configuartions_ViewKomuSetting,
                    PermissionNames.Admin_Configuartions_ViewProjectSetting ,
                    PermissionNames.Admin_Configuartions_ViewHrmSetting ,
                    PermissionNames.Admin_Configuartions_ViewTimesheetSetting ,
                    PermissionNames.Admin_Configuartions_ViewFinanceSetting ,
                    PermissionNames.Admin_Configuartions_ViewTalentSetting,
                    PermissionNames.Admin_Configuartions_ViewSendReportSetting ,
                    PermissionNames.Admin_Configuartions_ViewGoogleClientAppSetting ,
                    PermissionNames.Admin_Configuartions_ViewDefaultWorkingHourPerDaySetting ,
                    PermissionNames.Admin_Configuartions_ViewMaxCountHistoryOfRetroAndReviewPoint ,
                    PermissionNames.Admin_Configuartions_ViewAuditScoreSetting,
                    PermissionNames.Admin_Configuartions_WeeklyReportTime_Edit,
                    PermissionNames.Admin_Configurations_ViewGuideLineSetting,
                    PermissionNames.Admin_Configurations_ViewInformPmSetting,

                    #endregion Configuration

                    #region Skill

                    PermissionNames.Admin_Skills,
                    PermissionNames.Admin_Skills_View,
                    PermissionNames.Admin_Skills_Create ,
                    PermissionNames.Admin_Skills_Edit ,
                    PermissionNames.Admin_Skills_Delete ,

                    #endregion Skill

                    #region Currencies

                    PermissionNames.Admin_Currencies ,
                    PermissionNames.Admin_Currencies_View ,
                    PermissionNames.Admin_Currencies_Create ,
                    PermissionNames.Admin_Currencies_Edit ,
                    PermissionNames.Admin_Currencies_Delete ,

                    #endregion Currencies

                    #region User

                    PermissionNames.Admin_Users ,
                    PermissionNames.Admin_Users_View ,
                    PermissionNames.Admin_Users_Create ,
                    PermissionNames.Admin_Users_SyncDataFromHrm ,
                    PermissionNames.Admin_Users_ViewProjectHistory ,
                    PermissionNames.Admin_Users_Edit ,
                    PermissionNames.Admin_Users_UpdateSkill ,
                    PermissionNames.Admin_Users_UpdateRole ,
                    PermissionNames.Admin_Users_ActiveAndDeactive ,
                    PermissionNames.Admin_Users_UploadAvatar ,
                    PermissionNames.Admin_Users_ResetPassword ,
                    PermissionNames.Admin_Users_DeleteFakeUser ,

                    #endregion User

                    #region Role

                    PermissionNames.Admin_Roles,
                    PermissionNames.Admin_Roles_View,
                    PermissionNames.Admin_Roles_Create ,
                    PermissionNames.Admin_Roles_Edit ,
                    PermissionNames.Admin_Roles_Delete,

                    #endregion Role

                    #region AuditLogs

		            PermissionNames.Admin_AuditLogs,
                    PermissionNames.Admin_AuditLogs_View,

	                #endregion AuditLogs

                    #region Project

                    #region Projects > Outsourcing Project

                    PermissionNames.Projects ,
                    PermissionNames.Projects_OutsourcingProjects ,
                    PermissionNames.Projects_OutsourcingProjects_ViewAllProject ,
                    PermissionNames.Projects_OutsourcingProjects_ViewMyProjectOnly ,
                    PermissionNames.Projects_OutsourcingProjects_ViewBillInfo ,
                    PermissionNames.Projects_OutsourcingProjects_ViewBillAccount ,
                    PermissionNames.Projects_OutsourcingProjects_Create ,
                    PermissionNames.Projects_OutsourcingProjects_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_Delete ,
                    PermissionNames.Projects_OutsourcingProjects_Close ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail ,
                    PermissionNames.Projects_OutsourcingProjects_CheckProjectInvoiceSetting,
                    PermissionNames.Projects_OutsourcingProjects_ViewRequireWeeklyReport,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_Edit ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateNote ,
                    //PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_AddNewRisk ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Delete ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_SetDone ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Update_Note ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabTimesheet ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_Edit ,

                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_View ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_UploadFile ,
                    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_DeleteFile ,

                    #endregion Projects > Outsourcing Project

                    #region Projects > Product Project

                    PermissionNames.Projects ,
                    PermissionNames.Projects_ProductProjects ,
                    PermissionNames.Projects_ProductProjects_ViewAllProject ,
                    PermissionNames.Projects_ProductProjects_ViewMyProjectOnly ,
                    PermissionNames.Projects_ProductProjects_ViewRequireWeeklyReport,

                    PermissionNames.Projects_ProductProjects_Create ,
                    PermissionNames.Projects_ProductProjects_Edit ,
                    PermissionNames.Projects_ProductProjects_Delete ,
                    PermissionNames.Projects_ProductProjects_Close ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral_Edit ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote ,
                    //PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Create ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Edit ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Delete ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabTimesheet ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription_Edit ,

                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_View ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile ,
                    PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile ,

                    #endregion Projects > Product Project

                    #region Projects > training Project

                    PermissionNames.Projects ,
                    PermissionNames.Projects_TrainingProjects ,
                    PermissionNames.Projects_TrainingProjects_ViewAllProject ,
                    PermissionNames.Projects_TrainingProjects_ViewMyProjectOnly ,
                    PermissionNames.Projects_TrainingProjects_ViewRequireWeeklyReport,

                    PermissionNames.Projects_TrainingProjects_Create ,
                    PermissionNames.Projects_TrainingProjects_Edit ,
                    PermissionNames.Projects_TrainingProjects_Delete ,
                    PermissionNames.Projects_TrainingProjects_Close ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral_Edit ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateNote ,
                    //PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Create ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Edit ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Delete ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabTimesheet ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_Edit ,

                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_View ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_UploadFile ,
                    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_DeleteFile ,

                    #endregion Projects > training Project

                    #endregion Project

                    #region Weekly report
                    
                    PermissionNames.WeeklyReport ,
                    PermissionNames.WeeklyReport_View ,
                    PermissionNames.WeeklyReport_CloseAndAddNew ,
                    PermissionNames.WeeklyReport_CollectTimesheet ,
                    PermissionNames.WeeklyReport_Rename ,
                    PermissionNames.WeeklyReport_ViewInfo ,

                    PermissionNames.WeeklyReport_InformPM ,
                    PermissionNames.WeeklyReport_InformPM_View ,

                    PermissionNames.WeeklyReport_ReportDetail ,
                    PermissionNames.WeeklyReport_ReportDetail_View ,
                    PermissionNames.WeeklyReport_ReportDetail_UpdateNote ,
                    //PermissionNames.WeeklyReport_ReportDetail_UpdateProjectHealth ,

                    PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria,
                    PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus,
                    PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit,
                    PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_View_Guideline,
                    PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Update_Guideline,

                    PermissionNames.WeeklyReport_ReportDetail_PMIssue ,
                    PermissionNames.WeeklyReport_ReportDetail_PMIssue_View ,
                    PermissionNames.WeeklyReport_ReportDetail_PMIssue_AddMeetingNote ,
                    PermissionNames.WeeklyReport_ReportDetail_PMIssue_SetDone ,

                    PermissionNames.WeeklyReport_ReportDetail_PMRisk ,
                    PermissionNames.WeeklyReport_ReportDetail_PMRisk_View ,
                    PermissionNames.WeeklyReport_ReportDetail_PMRisk_SetDone ,

                    PermissionNames.WeeklyReport_ReportDetail_CurrentResource ,
                    PermissionNames.WeeklyReport_ReportDetail_CurrentResource_View ,
                    PermissionNames.WeeklyReport_ReportDetail_CurrentResource_Release ,
                    PermissionNames.WeeklyReport_ReportDetail_CurrentResource_Update_Note ,

                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_View ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_CreateNewPlan ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_Edit ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmOut ,
                    PermissionNames.WeeklyReport_ReportDetail_PlannedResource_CancelPlan ,

                    PermissionNames.WeeklyReport_ReportDetail_ChangedResource ,
                    PermissionNames.WeeklyReport_ReportDetail_ChangedResource_View ,
                    PermissionNames.WeeklyReport_ReportDetail_GuideLine_View ,
                    PermissionNames.WeeklyReport_ReportDetail_GuideLine_Update ,
                    PermissionNames.WeeklyReport_ReportDetail_LastReviewDate_Check ,
                    PermissionNames.WeeklyReport_ReportDetail_PrioritizeReview_Check ,

                    PermissionNames.ResourceRequest ,
                    PermissionNames.ResourceRequest_View ,
                    PermissionNames.ResourceRequest_CreateNewRequestForAllProject ,
                    PermissionNames.ResourceRequest_CreateNewRequestByPM ,
                    PermissionNames.ResourceRequest_PlanNewResourceForRequest ,
                    //PermissionNames.ResourceRequest_UpdateResourceRequestPlan ,
                    //PermissionNames.ResourceRequest_RemoveResouceRequestPlan ,
                    PermissionNames.ResourceRequest_SetDone ,
                    PermissionNames.ResourceRequest_CancelAllRequest ,
                    PermissionNames.ResourceRequest_CancelMyRequest ,
                    PermissionNames.ResourceRequest_EditPmNote ,
                    PermissionNames.ResourceRequest_EditDmNote ,
                    PermissionNames.ResourceRequest_Edit ,
                    PermissionNames.ResourceRequest_Delete ,
                    PermissionNames.ResourceRequest_SendRecruitment ,

                    #endregion Weekly report

                    #region TrainingRequest

                    PermissionNames.TrainingRequest ,
                    PermissionNames.TrainingRequest_View ,
                    PermissionNames.TrainingRequest_CreateNewRequestForAllProject ,
                    PermissionNames.TrainingRequest_CreateNewRequestByPM ,
                    PermissionNames.TrainingRequest_PlanNewTrainingForRequest ,
                    //PermissionNames.TrainingRequest_UpdateTrainingRequestPlan ,
                    //PermissionNames.TrainingRequest_RemoveTrainingRequestPlan ,
                    PermissionNames.TrainingRequest_SetDone ,
                    PermissionNames.TrainingRequest_CancelAllRequest ,
                    PermissionNames.TrainingRequest_CancelMyRequest ,
                    PermissionNames.TrainingRequest_EditPmNote ,
                    PermissionNames.TrainingRequest_EditDmNote ,
                    PermissionNames.TrainingRequest_Edit ,
                    PermissionNames.TrainingRequest_Delete ,
                    PermissionNames.TrainingRequest_SendRecruitment ,

                    #endregion TrainingRequest

                    #region Resource

                    PermissionNames.Resource ,
                    PermissionNames.Resource_TabPool ,
                    PermissionNames.Resource_TabPool_View ,
                    PermissionNames.Resource_TabPool_ViewHistory ,
                    PermissionNames.Resource_TabPool_CreatePlan ,
                    PermissionNames.Resource_TabPool_EditPlan ,
                    PermissionNames.Resource_TabPool_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Resource_TabPool_Release ,
                    PermissionNames.Resource_TabPool_ConfirmOut ,
                    PermissionNames.Resource_TabPool_CancelMyPlan ,
                    PermissionNames.Resource_TabPool_CancelAnyPlan ,
                    PermissionNames.Resource_TabPool_EditTempProject ,
                    PermissionNames.Resource_TabPool_AddTempProject ,
                    PermissionNames.Resource_TabPool_UpdateSkill ,
                    PermissionNames.Resource_TabPool_EditNote ,
                    PermissionNames.Resource_TabPool_ProjectDetail,

                    PermissionNames.Resource_TabAllResource ,
                    PermissionNames.Resource_TabAllResource_View ,
                    PermissionNames.Resource_TabAllResource_ViewHistory ,
                    PermissionNames.Resource_TabAllResource_CreatePlan ,
                    PermissionNames.Resource_TabAllResource_EditPlan ,
                    PermissionNames.Resource_TabAllResource_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Resource_TabAllResource_ConfirmOut ,
                    PermissionNames.Resource_TabAllResource_CancelMyPlan ,
                    PermissionNames.Resource_TabAllResource_CancelAnyPlan ,
                    PermissionNames.Resource_TabAllResource_UpdateSkill ,
                    PermissionNames.Resource_TabAllResource_ProjectDetail,

                    PermissionNames.Resource_TabVendor ,
                    PermissionNames.Resource_TabVendor_View ,
                    PermissionNames.Resource_TabVendor_ViewHistory ,
                    PermissionNames.Resource_TabVendor_CreatePlan ,
                    PermissionNames.Resource_TabVendor_EditPlan ,
                    PermissionNames.Resource_TabVendor_ConfirmPickEmployeeFromPoolToProject ,
                    PermissionNames.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther ,
                    PermissionNames.Resource_TabVendor_ConfirmOut ,
                    PermissionNames.Resource_TabVendor_CancelMyPlan ,
                    PermissionNames.Resource_TabVendor_CancelAnyPlan ,
                    PermissionNames.Resource_TabVendor_UpdateSkill ,
                    PermissionNames.Resource_TabVendor_ProjectDetail,

                    PermissionNames.Resource_TabPlanningBillAcccount,
                    PermissionNames.Resource_ViewUserLevel,

                    #endregion Resource

                    #region Timesheet

                    PermissionNames.Timesheets ,
                    PermissionNames.Timesheets_View ,
                    PermissionNames.Timesheets_Create ,
                    PermissionNames.Timesheets_Edit ,
                    PermissionNames.Timesheets_Delete ,
                    PermissionNames.Timesheets_ForceDelete ,
                    PermissionNames.Timesheets_CloseAndActive ,

                    PermissionNames.Timesheets_TimesheetDetail ,
                    PermissionNames.Timesheets_TimesheetDetail_View ,
                    PermissionNames.Timesheets_TimesheetDetail_ViewAll ,
                    PermissionNames.Timesheets_TimesheetDetail_ViewMyProjectOnly ,

                    PermissionNames.Timesheets_TimesheetDetail_ViewBillRate ,
                    PermissionNames.Timesheets_TimesheetDetail_AddProjectToTimesheet ,
                    PermissionNames.Timesheets_TimesheetDetail_UploadTimesheetFile ,
                    PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceAllProject ,
                    PermissionNames.Timesheets_TimesheetDetail_ExportInvoice ,
                    PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceForTax ,
                    PermissionNames.Timesheets_TimesheetDetail_UpdateNote ,
                    PermissionNames.Timesheets_TimesheetDetail_Delete ,
                    PermissionNames.Timesheets_TimesheetDetail_EditInvoiceInfo ,
                    PermissionNames.Timesheets_TimesheetDetail_SendInvoiceToFinfast,

                    PermissionNames.Timesheets_TimesheetDetail_UpdateBill ,
                    PermissionNames.Timesheets_TimesheetDetail_UpdateBill_Edit ,
                    PermissionNames.Timesheets_TimesheetDetail_UpdateBill_SetDone ,

                    PermissionNames.Timesheets_TimesheetDetail_UpdateTimsheet ,
                    PermissionNames.Timesheets_TimesheetDetail_RemoveAccount ,

                    #endregion Timesheet

                    #region Criteria

		            PermissionNames.Admin_Criteria,
                    PermissionNames.Admin_Criteria_View,
                    PermissionNames.Admin_Criteria_Create,
                    PermissionNames.Admin_Criteria_Edit,
                    PermissionNames.Admin_Criteria_Delete,
                    PermissionNames.Admin_Criteria_Active_DeActive,

                    #endregion Criteria

                    #region Audit

                    PermissionNames.Audits,
                    PermissionNames.Audits_Criteria,
                    PermissionNames.Audits_Criteria_Create ,
                    PermissionNames.Audits_Criteria_Edit ,
                    PermissionNames.Audits_Criteria_Delete ,
                    PermissionNames.Audits_Criteria_Active ,
                    PermissionNames.Audits_Criteria_DeActive ,
                    PermissionNames.Audits_Criteria_ChangeApplicable ,


                    PermissionNames.Audits_Tailoring,
                    PermissionNames.Audits_Tailoring_DownLoadTemplate ,
                    PermissionNames.Audits_Tailoring_Import ,
                    PermissionNames.Audits_Tailoring_Create ,
                    PermissionNames.Audits_Tailoring_Delete ,
                    PermissionNames.Audits_Tailoring_DownLoadTailoringTemplate ,
                    PermissionNames.Audits_Tailoring_Detail ,
                    PermissionNames.Audits_Tailoring_Detail_ViewNote ,
                    PermissionNames.Audits_Tailoring_Detail_Update ,
                    PermissionNames.Audits_Tailoring_Detail_Detele ,
                    PermissionNames.Audits_Tailoring_Update_Project ,
                    PermissionNames.Audits_Tailoring_Update_Project_Tailoring ,


                    PermissionNames.Audits_Results ,
                    PermissionNames.Audits_Results_DownLoad_Template,
                    PermissionNames.Audits_Results_Import_Result ,
                    PermissionNames.Audits_Results_Edit ,
                    PermissionNames.Audits_Results_Delete ,
                    PermissionNames.Audits_Results_Detail ,
                    PermissionNames.Audits_Results_Detail_View ,
                    PermissionNames.Audits_Results_Detail_ViewNote ,

                    #endregion Audit
                }
            }
        };

        public class SystemPermission
        {
            public string Name { get; set; }
            public MultiTenancySides MultiTenancySides { get; set; }
            public string DisplayName { get; set; }
            public bool IsConfiguration { get; set; }
            public List<SystemPermission> Childrens { get; set; }
            public static List<SystemPermission> ListPermissions = new List<SystemPermission>()
            {
                 #region Admin

		         new SystemPermission{ Name =  PermissionNames.Admin, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Admin" },
                 #endregion Admin

                 #region Tenant

		         new SystemPermission{ Name =  PermissionNames.Admin_Tenants, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tenants" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Tenants_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Tenants_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Tenants_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Tenants_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

	            #endregion Tenant

                 #region Client

		         new SystemPermission{ Name =  PermissionNames.Admin_Clients, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Clients" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Clients_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Clients_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Clients_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Clients_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

	             #endregion Client

                 #region Branch

		         new SystemPermission{ Name =  PermissionNames.Admin_Branchs, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Branchs" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Branchs_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Branchs_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Branchs_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Branchs_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

	             #endregion Branch

                 #region Position

		         new SystemPermission{ Name =  PermissionNames.Admin_Positions, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Positions" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Positions_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Positions_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Positions_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Positions_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

	             #endregion Position

                 #region Technology

		         new SystemPermission{ Name =  PermissionNames.Admin_Technologies, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Technologies" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Technologies_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Technologies_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Technologies_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Technologies_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

                 #endregion Technology

                 #region Configuration

		         new SystemPermission{ Name =  PermissionNames.Admin_Configuartions, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Configuartions" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewKomuSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Komu Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewProjectSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Project Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewHrmSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view HRM Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewTimesheetSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Timesheet Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewFinanceSetting , MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Finance Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewTalentSetting , MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Talent Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewSendReportSetting , MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Send Report Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewGoogleClientAppSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Google Client App Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewDefaultWorkingHourPerDaySetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view Default Working Hour Per Day Setting" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewMaxCountHistoryOfRetroAndReviewPoint, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Max Count History Of Retro And Review Point" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_ViewAuditScoreSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Audit Score For Criteria" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configuartions_WeeklyReportTime_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Time Count Down Weekly Report" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configurations_ViewGuideLineSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Guide Line" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Configurations_ViewInformPmSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Inform PM Setting" },

	             #endregion Configuration

                 #region Skill

		         new SystemPermission{ Name =  PermissionNames.Admin_Skills, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "skills" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Skills_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Skills_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Skills_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Skills_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

	             #endregion Skill

                 #region Currency

		         new SystemPermission{ Name =  PermissionNames.Admin_Currencies, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Currencies" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Currencies_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Currencies_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Currencies_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Currencies_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

                 #endregion Currency

                 #region User

		         new SystemPermission{ Name =  PermissionNames.Admin_Users, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Users" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_SyncDataFromHrm, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Sync Data From HRM" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_ViewProjectHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Project History" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit User Info" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_UpdateSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_UpdateRole, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Role" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_ActiveAndDeactive, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Active/Deactive User" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_UploadAvatar, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload Avatar" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_ResetPassword, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Reset Password" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Users_DeleteFakeUser, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete Fake User" },

                 #endregion User

                 #region Role

		         new SystemPermission{ Name =  PermissionNames.Admin_Roles, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Roles" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Roles_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Roles_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Roles_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Admin_Roles_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

	             #endregion Role

                 #region AuditLogs

		         new SystemPermission{ Name =  PermissionNames.Admin_AuditLogs, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "AuditLogs" },
                 new SystemPermission{ Name =  PermissionNames.Admin_AuditLogs_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },

	             #endregion AuditLogs

                 #region Project

		         new SystemPermission{ Name =  PermissionNames.Projects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Projects" },

                 #region Project > OutSourcing

		         new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Outsourcing Projects" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ViewAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ViewMyProjectOnly ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ViewBillInfo ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Bill Info" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ViewBillAccount ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Bill Account" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_Close, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Close" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_CheckProjectInvoiceSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Require WeeklyReport" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ViewRequireWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Check Project Invoice Setting" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab General" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Resource Management" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource From Pool" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource From Other Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Weekly Report" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update note" },
                 //new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Report" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Issues" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Issue" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Risk" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_AddNewRisk, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Risk" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },
                  new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Update_Note, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From Pool To Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Bill Info" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "InvoiceSetting: View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "InvoiceSetting: Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Rate" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Note" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab timesheet" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project Description" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project File" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_UploadFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload File" },
                 new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_DeleteFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete File" },

	             #endregion Project > OutSourcing

                 #region Project > Product Project

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Product Projects" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ViewAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ViewMyProjectOnly ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_Close, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Close" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ViewRequireWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Require WeeklyReport" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab General" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Resource Management" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource From Pool" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource From Other Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Weekly Report" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update note" },
                 //new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Report" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Issues" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Issue" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From Pool To Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Bill Info" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Timesheet" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project Description" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project File" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload File" },
                 new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete File" },

                 #endregion Project > Product Project

                 #region Project > Training Projects

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Training Projects" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ViewAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ViewMyProjectOnly ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_Close, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Close" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ViewRequireWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Require WeeklyReport" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab General" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Resource Management" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource From Pool" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource From Other Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete Request" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Weekly Report" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update note" },
                 //new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Report" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Issues" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Issue" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From Pool To Project" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resource" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Bill Info" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Timesheet" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project Description" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit" },

                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project File" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_UploadFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload File" },
                 new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_DeleteFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete File" },

                 #endregion Project > Training Projects

                 #endregion Project

                 #region Weekly Report

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Weekly Report" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Wiew" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_CloseAndAddNew, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Clost And Add New" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_CollectTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Collect Timesheet" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_Rename, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Rename" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ViewInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Wiew Info" },

                new SystemPermission{ Name =  PermissionNames.WeeklyReport_InformPM, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Inform Pm" },
                new SystemPermission{ Name =  PermissionNames.WeeklyReport_InformPM_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Inform Pm" },

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Report Detail" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note" },
                 //new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health" },

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PMIssues" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMIssue_AddMeetingNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add Meeting Note" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMRisk, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PMRisks" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMRisk_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PMRisk_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done" },

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_CurrentResource_Update_Note, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note" },

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_CreateNewPlan,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmPickEmployeeFromPoolToProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmOut ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out" },
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_PlannedResource_CancelPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan" },

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ChangedResource ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resource"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ChangedResource_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"},

                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_GuideLine_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View GuideLine"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_GuideLine_Update ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update GuideLine"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_View_Guideline ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Guideline (Criteria status)"},
                 new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Update_Guideline ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Guideline (Criteria status)"},

                 new SystemPermission{ Name = PermissionNames.WeeklyReport_ReportDetail_LastReviewDate_Check, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Check Last Review Date"},
                 new SystemPermission{ Name = PermissionNames.WeeklyReport_ReportDetail_PrioritizeReview_Check, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Check Prioritize Review"},

                 #endregion Weekly Report


                 #region ResourceRequest

                 new SystemPermission{ Name =  PermissionNames.ResourceRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View list Resource request"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_CreateNewRequestForAllProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request For All Project"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_CreateNewRequestByPM ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM create request for his/her project only"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_PlanNewResourceForRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request"},
                 //new SystemPermission{ Name =  PermissionNames.ResourceRequest_UpdateResourceRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Plan"},
                 //new SystemPermission{ Name =  PermissionNames.ResourceRequest_RemoveResouceRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Remove Plan"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_SetDone ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_CancelAllRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel All Request"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_CancelMyRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Cancel Request for My Project"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_EditPmNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit PM Note"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_EditDmNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit DM Note"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.ResourceRequest_SendRecruitment ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"},

                 #endregion ResourceRequest

                 #region TrainingRequest

                 new SystemPermission{ Name =  PermissionNames.TrainingRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Training Request"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View list Training request"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_CreateNewRequestForAllProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request For All Project"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_CreateNewRequestByPM ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM create request for his/her project only"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_PlanNewTrainingForRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Training For Request"},
                 //new SystemPermission{ Name =  PermissionNames.TrainingRequest_UpdateTrainingRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Plan"},
                 //new SystemPermission{ Name =  PermissionNames.TrainingRequest_RemoveTrainingRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Remove Plan"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_SetDone ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_CancelAllRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel All Request"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_CancelMyRequest ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Cancel Request for My Project"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_EditPmNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit PM Note"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_EditDmNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit DM Note"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.TrainingRequest_SendRecruitment ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"},

                 #endregion TrainingRequest

                 #region Resource

                 new SystemPermission{ Name =  PermissionNames.Resource ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Pool"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_ViewHistory ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_CreatePlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_EditPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_ConfirmPickEmployeeFromPoolToProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL to Project"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_Release ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release Employee Working Temp On A Project"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_ConfirmOut ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_CancelMyPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel My Plan Only"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_CancelAnyPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Any Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_EditTempProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Temp Project"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_AddTempProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add Temp Project"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_UpdateSkill ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_EditNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Note"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabPool_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail"},

                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "All Resource"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_ViewHistory ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_CreatePlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_EditPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_ConfirmPickEmployeeFromPoolToProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_ConfirmOut ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_CancelMyPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel My Plan Only"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_CancelAnyPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Any Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_UpdateSkill ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabAllResource_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail"},

                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "All Resource"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_ViewHistory ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_CreatePlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_EditPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_ConfirmPickEmployeeFromPoolToProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_ConfirmOut ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_CancelMyPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel My Plan Only"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_CancelAnyPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Any Plan"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_UpdateSkill ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"},
                 new SystemPermission{ Name =  PermissionNames.Resource_TabVendor_ProjectDetail ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail"},

                 new SystemPermission{ Name =  PermissionNames.Resource_TabPlanningBillAcccount ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planning Bill Account"},
                 new SystemPermission{ Name =  PermissionNames.Resource_ViewUserLevel ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View User Level"},
                 #endregion Resource

                 #region Timesheet

                 new SystemPermission{ Name =  PermissionNames.Timesheets ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Timesheet"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_Create ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_ForceDelete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Force Delete"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_CloseAndActive ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Close/Active"},

                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Timesheet Detail"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_ViewAll ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Timesheet View All Project Timesheet"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_ViewMyProjectOnly ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Timesheet View My Project Only"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_ViewBillRate ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Bill Rate"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_AddProjectToTimesheet ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add Project To Timesheet"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_UploadTimesheetFile ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload Timesheet File"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_ExportInvoice ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Export Invoice"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceAllProject ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Export Invoice All Project"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceForTax ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Export Invoice For Tax"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_UpdateNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_EditInvoiceInfo ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Invoice Info (Invoice Number,Working Day,Transfer Fee,Discount) of project"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_SendInvoiceToFinfast, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Invoice to Finfast"},

                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_UpdateBill ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Bill"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_UpdateBill_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_UpdateBill_SetDone ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"},

                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_UpdateTimsheet ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Timesheet"},
                 new SystemPermission{ Name =  PermissionNames.Timesheets_TimesheetDetail_RemoveAccount ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Remove Account Timesheet"},

                 #endregion Timesheet

                 #region Criteria

		         new SystemPermission{ Name =  PermissionNames.Admin_Criteria ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Criteria"},
                 new SystemPermission{ Name =  PermissionNames.Admin_Criteria_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"},
                 new SystemPermission{ Name =  PermissionNames.Admin_Criteria_Create ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"},
                 new SystemPermission{ Name =  PermissionNames.Admin_Criteria_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.Admin_Criteria_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.Admin_Criteria_Active_DeActive ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Active/DeActive"},

                #endregion Criteria

                 #region Audit

                 new SystemPermission{ Name =  PermissionNames.Audits ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Audits"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Audits Criteria"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Create ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Active ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Active"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria_DeActive ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "DeActive"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Criteria_ChangeApplicable ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Applicable"},

                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Audits Tailoring"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_DownLoadTemplate,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Download template"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Import,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Import"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Create,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Delete,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_DownLoadTailoringTemplate,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "DownLoad Tailoring Template"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Detail"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail_ViewNote,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Note"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail_Update,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail_Detele,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Detele"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Update_Project,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Update_Project_Tailoring,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Tailoring"},

                 new SystemPermission{ Name =  PermissionNames.Audits_Results ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Audits Result"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_DownLoad_Template,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "DownLoad Template"},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_Import_Result ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Import Result "},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Edit "},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Delete "},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_Detail ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Detail "},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_Detail_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " View "},
                 new SystemPermission{ Name =  PermissionNames.Audits_Results_Detail_ViewNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ViewNote "},
                 #endregion Audit
            };
            public static List<SystemPermission> TreePermissions = new List<SystemPermission>() {

                #region Admin

                new SystemPermission {
                    Name = PermissionNames.Admin, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Admin",
                        Childrens = new List < SystemPermission > () {

                            #region Tenant

                            new SystemPermission {
                                Name = PermissionNames.Admin_Tenants, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tenants",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Tenants_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Tenants_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Tenants_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Tenants_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    }
                            },

                            #endregion Tenant

                            #region Client

                            new SystemPermission {
                                Name = PermissionNames.Admin_Clients, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Clients",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Clients_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Clients_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Clients_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Clients_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    }
                            },

                            #endregion Client

                            #region Branch

                            new SystemPermission {
                                Name = PermissionNames.Admin_Branchs, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Branchs",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Branchs_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Branchs_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Branchs_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Branchs_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    },
                            },

                            #endregion Branch

                            #region Position

                            new SystemPermission {
                                Name = PermissionNames.Admin_Positions, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Positions",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Positions_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Positions_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Positions_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Positions_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    }
                            },

                            #endregion Position

                            #region Technology

                            new SystemPermission {
                                Name = PermissionNames.Admin_Technologies, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Technologies",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Technologies_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Technologies_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Technologies_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Technologies_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    },
                            },

                            #endregion Technology

                            #region Configuration

                            new SystemPermission {
                                Name = PermissionNames.Admin_Configuartions, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Configuration",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewKomuSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Komu Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewProjectSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Project Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewHrmSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Hrm Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewTimesheetSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Timesheet Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewFinanceSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Finance Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewTalentSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Talent Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewSendReportSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Send Report Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewGoogleClientAppSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Google Client App Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewDefaultWorkingHourPerDaySetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Default Working Hour Per Day Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewMaxCountHistoryOfRetroAndReviewPoint, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Max Count History Of Retro And Review Point"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_ViewAuditScoreSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Audit Score For Criteria"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Configuartions_WeeklyReportTime_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Time Count Down Weekly Report"
                                        },
                                         new SystemPermission {
                                            Name = PermissionNames.Admin_Configurations_ViewGuideLineSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Guide Line"
                                        },
                                          new SystemPermission {
                                            Name = PermissionNames.Admin_Configurations_ViewInformPmSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Inform PM Setting"
                                        },
                                    }
                            },

                            #endregion Configuration

                            #region Skill

                            new SystemPermission {
                                Name = PermissionNames.Admin_Skills, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Skills",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Skills_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Skills_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Skills_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Skills_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    }
                            },

                            #endregion Skill

                            #region Currency

                            new SystemPermission {
                                Name = PermissionNames.Admin_Currencies, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Currencies",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Currencies_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Currencies_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Currencies_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Currencies_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    }
                            },

                            #endregion Currency

                            #region User

                            new SystemPermission {
                                Name = PermissionNames.Admin_Users, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Users",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_SyncDataFromHrm, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Sync User From HRM"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_ViewProjectHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Project History"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_UpdateSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_UpdateRole, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Role"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_ActiveAndDeactive, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Active/Deactive"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_UploadAvatar, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload Avatar"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_ResetPassword, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Reset Password"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Users_DeleteFakeUser, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete Fake User"
                                        },
                                    }
                            },

                            #endregion User

                            #region Role

                            new SystemPermission {
                                Name = PermissionNames.Admin_Roles, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Roles",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Roles_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Roles_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Roles_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Roles_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                    }
                            },

                            #endregion Role

                            #region AuditLogs

		                    new SystemPermission {
                                Name = PermissionNames.Admin_AuditLogs, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "AuditLogs",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_AuditLogs_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                    }
                            },

	                        #endregion AuditLogs

                            #region Criteria

                            new SystemPermission {
                                Name = PermissionNames.Admin_Criteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Criteria",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Criteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Criteria_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Criteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Criteria_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Admin_Criteria_Active_DeActive, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Active/DeActive"
                                        },
                                    }
                            },

                            #endregion Criteria
                        },
                },

                #endregion Admin

                #region Projects

                #region Project > OutSourcing

                new SystemPermission {
                    Name = PermissionNames.Projects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Projects",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.Projects_OutsourcingProjects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Outsourcing Projects",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_ViewAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_ViewMyProjectOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_ViewBillInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Bill Info"
                                        },
                                          new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_ViewBillAccount, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Bill Account"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_Close, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "close"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_CheckProjectInvoiceSetting, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Check Project Invoice Setting"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_ViewRequireWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Require WeeklyReport"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab General",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabGeneral_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                }
                                                            },
                                                    },

                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Resource Management",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resources",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource: From POOL"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource: From Other Project"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                                                            },
                                                                        }
                                                                },

                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resources",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ConfirmOut"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                                                            },
                                                                        }
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Request"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"
                                                                            },
                                                                        }
                                                                },
                                                            },
                                                    },

                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Weekly report",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                                                },
                                                                //new SystemPermission{ Name =  PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health"},
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Report"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status"
                                                                            },
                                                                        }
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Issues",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit "
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                            },
                                                                        },
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Risks",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_AddNewRisk, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit "
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                            },
                                                                        },
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resources",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                                            },
                                                                             new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Update_Note, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                                                            },
                                                                        },
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resources",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ConfirmOut"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                                            },
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                            },
                                                                        }
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resources",
                                                                        Childrens = new List < SystemPermission > () {
                                                                            new SystemPermission {
                                                                                Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                            },
                                                                        }
                                                                },
                                                            },
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Bill Info",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "view"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "InvoiceSetting: View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "InvoiceSetting: Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Rate"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Note"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Timsheet"
                                                    },

                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project Description",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectDescription_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project File",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_UploadFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload File"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_DeleteFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete File"
                                                                },
                                                            }
                                                    }
                                                },
                                        }
                                    },
                            },

                            #endregion Project > OutSourcing

                #region Project > ProductProject

                new SystemPermission {
                    Name = PermissionNames.Projects_ProductProjects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Product Projects",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_ViewAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Project"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_ViewMyProjectOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_Close, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "close"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_ViewRequireWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Require WeeklyReport"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_ProductProjects_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab General",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabGeneral_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                    },
                                                },
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Resource Management",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource: From POOL"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource: From Other Project"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                                                },
                                                            }
                                                    },

                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ConfirmOut"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Request"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"
                                                                },
                                                            }
                                                    },
                                                },
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Weekly report",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                                    },
                                                    //new SystemPermission{ Name =  PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health"},
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Report"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status"
                                                                },
                                                            }
                                                    },

                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Issues",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit "
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                },
                                                            },
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                                },
                                                            },
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ConfirmOut"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                            }
                                                    },
                                                },
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Timesheet"
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project Description",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectDescription_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                    },
                                                }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project File",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload File"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete File"
                                                    },
                                                }
                                        }
                                    },
                            }
                        },
                },

                #endregion Project > ProductProject

                #region Project > TrainingProject

                new SystemPermission {
                    Name = PermissionNames.Projects_TrainingProjects, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Training Projects",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_ViewAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Project"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_ViewMyProjectOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_Close, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "close"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_ViewRequireWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Require WeeklyReport"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Projects_TrainingProjects_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab General",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabGeneral_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                    },
                                                },
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Resource Management",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource: From POOL"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add New Resource: From Other Project"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                                                },
                                                            }
                                                    },

                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ConfirmOut"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_UpdateUserSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Skill"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CreateNewRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Request"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan New Resource For Request"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_CancelRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Request"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"
                                                                },
                                                            }
                                                    },
                                                },
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Weekly report",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                                    },
                                                    //new SystemPermission{ Name =  PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health"},
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Report"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Project Issues",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_AddNewIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit "
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PMProjectIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                                },
                                                            },
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                                },
                                                            },
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ConfirmOut"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                                },
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                                },
                                                            }
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resources",
                                                            Childrens = new List < SystemPermission > () {
                                                                new SystemPermission {
                                                                    Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                                },
                                                            }
                                                    },
                                                },
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Timesheet"
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project Description",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectDescription_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                    },
                                                }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Project File",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_UploadFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload File"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_DeleteFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete File"
                                                    },
                                                }
                                        }
                                    },
                            }
                        },
                },
            },
    },

    #endregion Project > TrainingProject

                #endregion Projects

                #region WeeklyReport

                new SystemPermission {
                    Name = PermissionNames.WeeklyReport, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Weekly Report",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                            },
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_CloseAndAddNew, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Close And Add New"
                            },
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_CollectTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Collect Timesheet"
                            },
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_Rename, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Rename"
                            },
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_ViewInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Info"
                            },
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_InformPM, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Inform Pm",
                                Childrens = new List<SystemPermission>()
                                {
                                    new SystemPermission {Name = PermissionNames.WeeklyReport_InformPM_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Inform Pm"},
                                }
                            },
                            new SystemPermission {
                                Name = PermissionNames.WeeklyReport_ReportDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Report Detail",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                        },
                                        //new SystemPermission{ Name =  PermissionNames.WeeklyReport_ReportDetail_UpdateProjectHealth, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project Health" },
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Health Criteria",
                                                Childrens = new List < SystemPermission > () {

                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Status"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_View_Guideline, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Guideline (Criteria status)"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Update_Guideline, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Guideline (Criteria status)"
                                                    },
                                                }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_PMIssue, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Issues",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PMIssue_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PMIssue_AddMeetingNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add Meeting Note"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PMIssue_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                    },
                                                }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_PMRisk, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Risks",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PMRisk_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PMRisk_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                                    },
                                                }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_CurrentResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Current Resource",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_CurrentResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_CurrentResource_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release"
                                                    },
                                                      new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_CurrentResource_Update_Note, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                                    },
                                                }
                                        }, new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Planned Resource",
                                                Childrens = new List < SystemPermission > () {
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_CreateNewPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create New Plan"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On A Project To Other"
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out "
                                                    },
                                                    new SystemPermission {
                                                        Name = PermissionNames.WeeklyReport_ReportDetail_PlannedResource_CancelPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Plan"
                                                    },
                                                }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_ChangedResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Changed Resource"
                                        },
                                         new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_GuideLine_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View GuideLine"
                                        },
                                          new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_GuideLine_Update, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update GuideLine"
                                        },
                                           new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_LastReviewDate_Check, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Last Review Date Check"
                                        },
                                           new SystemPermission {
                                            Name = PermissionNames.WeeklyReport_ReportDetail_PrioritizeReview_Check, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Prioritize Review Check"
                                        },
                                    }
                            },
                        },
                },

                #endregion WeeklyReport

                #region ResourceRequest

                new SystemPermission {
                    Name = PermissionNames.ResourceRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource Request",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View list Resource request"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_CreateNewRequestByPM, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM create new request for his/her project only"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_CreateNewRequestForAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create new request for all project"
                            },

                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_PlanNewResourceForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan Resource For Request"
                            },
                            //new SystemPermission { Name =  PermissionNames.ResourceRequest_UpdateResourceRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Plan"},
                            //new SystemPermission { Name =  PermissionNames.ResourceRequest_RemoveResouceRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Remove Plan"},
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_CancelAllRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel All Request"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_CancelMyRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Cancel Request for his/her Project only"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_EditPmNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit PM Note"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_EditDmNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit DM Note"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                            },
                            new SystemPermission {
                                Name = PermissionNames.ResourceRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"
                            },
                        },
                },

                #endregion ResourceRequest

                #region Training Request

                new SystemPermission {
                    Name = PermissionNames.TrainingRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Training Request",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View list Training request"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_CreateNewRequestByPM, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM create new request for his/her project only"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_CreateNewRequestForAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create new request for all project"
                            },

                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_PlanNewTrainingForRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Plan Training For Request"
                            },
                            //new SystemPermission { Name =  PermissionNames.TrainingRequest_UpdateTrainingRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Plan"},
                            //new SystemPermission { Name =  PermissionNames.TrainingRequest_RemoveTrainingRequestPlan ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Remove Plan"},
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_CancelAllRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel All Request"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_CancelMyRequest, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "PM Cancel Request for his/her Project only"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_EditPmNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit PM Note"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_EditDmNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit DM Note"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                            },
                            new SystemPermission {
                                Name = PermissionNames.TrainingRequest_SendRecruitment, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Recruitment"
                            },
                        },
                },

                #endregion Training Request

                #region Resource

                new SystemPermission {
                    Name = PermissionNames.Resource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Resource",
                        Childrens = new List < SystemPermission > () {
                            // Tab Pool
                            new SystemPermission {
                                Name = PermissionNames.Resource_TabPool, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Pool",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_CreatePlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_EditPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_Release, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Release Employee Working Temp On A Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_CancelMyPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel My Plan Only"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_CancelAnyPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Any Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_EditTempProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Temp Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_AddTempProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add Temp Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_UpdateSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update skill"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_EditNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Note"
                                        },
                                         new SystemPermission {
                                            Name = PermissionNames.Resource_TabPool_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail"
                                        },
                                    },
                            },

                            // Tab All Resource
                            new SystemPermission {
                                Name = PermissionNames.Resource_TabAllResource, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab All Resource",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_CreatePlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_EditPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On Project To Other"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_CancelMyPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel My Plan Only"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_CancelAnyPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Any Plan"
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_UpdateSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update skill"
                                        },

                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabAllResource_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail"
                                        },
                                    },
                            },
                            // Tab Vendor
                            new SystemPermission {
                                Name = PermissionNames.Resource_TabVendor, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Vendor",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_ViewHistory, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View History"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_CreatePlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_EditPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_ConfirmPickEmployeeFromPoolToProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Pick Employee From POOL To Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_ConfirmMoveEmployeeWorkingOnAProjectToOther, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Join: Move Employee Working On Project To Other"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_ConfirmOut, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Confirm Out"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_CancelMyPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel My Plan Only"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_CancelAnyPlan, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Cancel Any Plan"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_UpdateSkill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update skill"
                                        },
                                          new SystemPermission {
                                            Name = PermissionNames.Resource_TabVendor_ProjectDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Project Detail"
                                        },
                                    },
                            },
                            // Tab Planning Bill Account
                            new SystemPermission {
                                Name = PermissionNames.Resource_TabPlanningBillAcccount, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tab Planning Bill Account",
                            },
                            new SystemPermission {
                                Name = PermissionNames.Resource_ViewUserLevel, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View User Level",
                            }
                        }
                },

                #endregion Resource

                #region Timesheet

                new SystemPermission {
                    Name = PermissionNames.Timesheets, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Timesheet",
                        Childrens = new List < SystemPermission > () {
                            new SystemPermission {
                                Name = PermissionNames.Timesheets_View, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Timesheets_Create, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Timesheets_Edit, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Timesheets_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Timesheets_ForceDelete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Force Delete"
                            },
                            new SystemPermission {
                                Name = PermissionNames.Timesheets_CloseAndActive, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Close/Active"
                            },

                            new SystemPermission {
                                Name = PermissionNames.Timesheets_TimesheetDetail, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Timesheet Detail",
                                    Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_ViewAll, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View All Timesheet Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_ViewMyProjectOnly, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View My Project Only"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_ViewBillRate, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Bill Rate"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_AddProjectToTimesheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Add Project To Timesheet"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_UploadTimesheetFile, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Upload Timesheet File"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceAllProject, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Export Invoice All Project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_ExportInvoice, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Export Invoice"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceForTax, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Export Invoice For Tax"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_UpdateNote, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Note"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_Delete, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_UpdateBill, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Bill"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_UpdateTimsheet, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Timesheet"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_RemoveAccount, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Remove Account Timesheet"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_UpdateBill_SetDone, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Set Done"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_EditInvoiceInfo, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit Invoice Info (Invoice Number,Working Day,Transfer Fee,Discount) of project"
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Timesheets_TimesheetDetail_SendInvoiceToFinfast, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Send Invoice To Finfast"
                                        },
                                    }
                            },
                        },
                },

                #endregion Timesheet

                #region Audit

                new SystemPermission
                {
                     Name = PermissionNames.Audits, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Audits",
                      Childrens = new List < SystemPermission > () {
                                        new SystemPermission {
                                            Name = PermissionNames.Audits_Criteria, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Criteria Management",
                                            Childrens = new List<SystemPermission>
                                            {
                                                new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Create ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Edit"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Criteria_Active ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Active"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Criteria_DeActive ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "DeActive"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Criteria_ChangeApplicable ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Change Applicable"},
                                            }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Audits_Tailoring, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Tailoring Management"
                                            ,Childrens  = new List < SystemPermission > ()
                                            {
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_DownLoadTemplate,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Download template"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Import,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Import"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Create,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Create"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Delete,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Delete"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_DownLoadTailoringTemplate,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "DownLoad Tailoring Template"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Detail" ,
                                                    Childrens= new List<SystemPermission>
                                                    {
                                                        new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail_ViewNote,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "View Note"},
                                                        new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail_Update,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update"},
                                                        new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Detail_Detele,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Detele"},
                                                    }
                                                },
                                                new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Update_Project,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Project",
                                                    Childrens = new List<SystemPermission>
                                                    {
                                                        new SystemPermission{ Name =  PermissionNames.Audits_Tailoring_Update_Project_Tailoring,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Update Tailoring"},
                                                    }
                                                },
                                            }
                                        },
                                        new SystemPermission {
                                            Name = PermissionNames.Audits_Results, MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "Result Management"
                                            ,Childrens=  new List<SystemPermission>
                                            {
                                                new SystemPermission{ Name =  PermissionNames.Audits_Results_DownLoad_Template,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "DownLoad Template"},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Results_Import_Result ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Import Result "},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Results_Edit ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Edit "},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Results_Delete ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Delete "},
                                                new SystemPermission{ Name =  PermissionNames.Audits_Results_Detail ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " Detail ",
                                                    Childrens= new List<SystemPermission>
                                                    {
                                                        new SystemPermission{ Name =  PermissionNames.Audits_Results_Detail_View ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = " View "},
                                                        new SystemPermission{ Name =  PermissionNames.Audits_Results_Detail_ViewNote ,MultiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant, DisplayName = "ViewNote "},
                                                    }
                                                },
                                            }
                                        },
                      }
                },

                #endregion Audit
            };
        }
    }
}