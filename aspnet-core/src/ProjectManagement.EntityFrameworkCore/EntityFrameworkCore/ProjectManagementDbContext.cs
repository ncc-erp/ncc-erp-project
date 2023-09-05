using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Authorization.Roles;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.MultiTenancy;

namespace ProjectManagement.EntityFrameworkCore
{
    public class ProjectManagementDbContext : AbpZeroDbContext<Tenant, Role, User, ProjectManagementDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<AuditResult> AuditResults { get; set; }

        public DbSet<AuditResultPeople> AuditResultPeoples { get; set; }

        public DbSet<AuditSession> AuditSessions { get; set; }

        public DbSet<CheckListCategory> CheckListCategories { get; set; }

        public DbSet<CheckListItem> CheckListItems { get; set; }

        public DbSet<CheckListItemMandatory> CheckListItemMandatorys { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<PMReport> PMReports { get; set; }

        public DbSet<PMReportProject> PMReportProjects { get; set; }

        public DbSet<PMReportProjectIssue> PMReportProjectIssues { get; set; }

        public DbSet<PMReportProjectRisk> PMReportProjectRisks { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectFile> ProjectFiles { get; set; }

        public DbSet<ProjectCheckList> ProjectCheckLists { get; set; }

        public DbSet<ProjectMilestone> ProjectMilestones { get; set; }

        public DbSet<ProjectUser> ProjectUsers { get; set; }

        public DbSet<ProjectUserBill> ProjectUserBills { get; set; }

        public DbSet<ResourceRequest> ResourceRequests { get; set; }

        public DbSet<Timesheet> Timesheets { get; set; }

        public DbSet<TimesheetProject> TimesheetProjects { get; set; }

        public DbSet<CriteriaCategory> CriteriaCategories { get; set; }

        public DbSet<Criteria> Criterias { get; set; }

        public DbSet<Phase> Phases { get; set; }

        public DbSet<CheckPointUser> CheckPointUsers { get; set; }

        public DbSet<CheckPointUserDetail> CheckPointUserDetails { get; set; }

        public DbSet<CheckPointUserResult> CheckPointUserResults { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<CheckPointUserResultTag> CheckPointUserResultTags { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<UserSkill> UserSkills { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<TimesheetProjectBill> TimesheetProjectBills { get; set; }

        public DbSet<ResourceRequestSkill> ResourceRequestSkills { get; set; }

        public DbSet<Branch> Branchs { get; set; }

        public DbSet<Technology> Technologies { get; set; }

        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<ProjectCriteria> ProjectCriterias { get; set; }

        public DbSet<ProjectCriteriaResult> ProjectCriteriaResults { get; set; }
        public DbSet<ProcessCriteria> ProcessCriterias { get; set; }
        public DbSet<ProjectProcessCriteria> ProjectProcessCriterias { get; set; }
        public DbSet<ProjectProcessResult> ProjectProcessResults { get; set; }
        public DbSet<ProjectProcessCriteriaResult> ProjectProcessCriteriaResults { get; set; }
        public ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options)
            : base(options)
        {
        }
    }
}